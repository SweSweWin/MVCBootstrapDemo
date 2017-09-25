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

namespace MVCBootstrapDemo.Controllers
{
    public class DepartmentsController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        DepartmentDAL dal = new DepartmentDAL();

        // GET: Departments
        [HttpGet]
        public ActionResult Departments(int?page)
        {
            DepartmentModel model = new DepartmentModel();
            model.PageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int CurrentPage = model.PageNumber;
            int pageSize = Common.pagesize;
            int TotalRowCount = 0;
            List<DepartmentModel> lst = dal.SelectAllDataForBindGrid(CurrentPage, pageSize, out TotalRowCount);
            List<DepartmentModel> lstdeletecheck = new List<DepartmentModel>();
            foreach(var item in lst)
            {
                bool Isdelete = dal.CheckDelete(item.ID);
                model.IsDelete = Isdelete;
                lstdeletecheck.Add(item);
            }
            model.Departments = lstdeletecheck;
            if(lst.Count>0)
            {
                List<ListItem> pager = Common.generatePager(TotalRowCount,pageSize,CurrentPage);
                model.dlPager = pager;
            }
            return View(model);            
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,SortOrder")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,SortOrder")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
