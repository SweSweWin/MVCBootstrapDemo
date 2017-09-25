using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBootstrapDemo.Models;
using MVCBootstrapDemo.Models.DAL;
using MVCBootstrapDemo.App_Start;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace MVCBootstrapDemo.Controllers
{
    public class EmployeesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        EmployeeDAL dal = new EmployeeDAL();

        // GET: Employees
        [HttpGet]       
        public async Task<ActionResult> Employees(int? page, int? E_Department, int? E_Position, string E_Username)
        {
            EmployeeModel model = new EmployeeModel();
            model.PageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int currentpage = model.PageNumber;
            int pagesize = Common.pagesize;
            int totalrowcount = 0;
            model.ddlDepartmant = GetDepartment();
            model.ddlPosition = GetPosition();
            int dept = (E_Department==null?0:Convert.ToInt32(E_Department));
            int pos = (E_Position==null?0:Convert.ToInt32(E_Position));
            model.E_Department = dept;
            model.E_Position = pos;           
            model.E_Username = E_Username;
          
            List<EmployeeModel> lst = dal.BindData(currentpage, pagesize, out totalrowcount,dept,pos,E_Username);
            List<EmployeeModel> checkdelete = new List<EmployeeModel>();
            foreach(var e in lst)
            {
                bool enable = dal.CheckDelete(e.E_ID);
                e.IsDelete = enable;
                checkdelete.Add(e);
            }
            model.Employees = checkdelete;
            if(lst.Count>0)
            {
                List<ListItem> lstpager= Common.generatePager(totalrowcount, pagesize, currentpage);
                model.dlPager = lstpager;
            }
            return View(model);
        }

        private IEnumerable<SelectListItem> GetPosition()
        {
            IEnumerable<SelectListItem> lst = from p in db.Positions
                                              select new SelectListItem
                                              {
                                                  Selected = p.Pos_ID.ToString() == "Active",
                                                  Text = p.Pos_Name,
                                                  Value = p.Pos_ID.ToString()
                                              };
            return lst;
        }

        private IEnumerable<SelectListItem> GetDepartment()
        {

            IEnumerable<SelectListItem> lst = from d in db.Departments
                                              select new SelectListItem
                                              {
                                                  Selected = d.ID.ToString() == "Active",
                                                  Text=d.Name,
                                                  Value=d.ID.ToString() 

                                              };

            return lst;
           
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "E_ID,E_FullName,E_Department,E_Position,E_Username,E_Active")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "E_ID,E_FullName,E_Department,E_Position,E_Username,E_Active")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
