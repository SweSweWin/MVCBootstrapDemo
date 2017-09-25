using MVCBootstrapDemo.App_Start;
using MVCBootstrapDemo.Models;
using MVCBootstrapDemo.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Controllers
{
    public class UserController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        UserDAL dal = new UserDAL();
        // GET: User
        public ActionResult Users(int?page)
        {
            LoginModel model = new LoginModel();
            model.pageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int currentPage = model.pageNumber;
            int pageSize = Common.pagesize;
            int totalRowCount = 0;
            List<LoginModel> lst = dal.BindData(currentPage, pageSize, out totalRowCount);
            // List<LoginModel> checkdelete = new List<LoginModel>();
            model.users = lst;
            if(lst.Count>0)
            {
                List<ListItem> pager = Common.generatePager(totalRowCount,pageSize,currentPage);
                model.dlpager = pager;
            }
           return View(model);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {          
            return View();
        }

        
        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="UserID,UserName,Password")]LoginModel entity)
        {
            
            if (ModelState.IsValid)
            {
                user u = new user();
                u.U_ID = 0;
                u.U_Name = entity.Username;
                string expectedHashString = Common.Encrypt(entity.Password);
                u.U_Password = expectedHashString;
                // u.U_Level = Common.Get_SALT();
                u.U_Level = 0;
                db.users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            var u = db.users.Single(m => m.U_ID == id);
            return View(u);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var u = db.users.Single(m => m.U_ID == id);     
                if (TryUpdateModel(u))
                {
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(u);
            }
            catch
            {
                return View();
            }
            
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
