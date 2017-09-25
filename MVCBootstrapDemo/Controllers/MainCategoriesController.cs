using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBootstrapDemo.Models;
using MVCBootstrapDemo.App_Start;
using MVCBootstrapDemo.Models.DAL;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Controllers
{
    public class MainCategoriesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        MainCategoryDAL dal = new MainCategoryDAL();

        // GET: MainCategories
        [HttpGet]
        public ActionResult MainCategories(int?Page)
        {
            MainCategoryModel model = new MainCategoryModel();
            model.PageNumber = (Page == null ? 1 : Convert.ToInt32(Page));
            int currentpage = model.PageNumber;
            int pageSize = Common.pagesize;
            int TotalRowCount = 0;
            List<MainCategoryModel> lst = dal.SelectAllDataForBindGrid(currentpage,pageSize,out TotalRowCount);
            List<MainCategoryModel> lstcheckdelete = new List<MainCategoryModel>();
            foreach (var m in lst)
            {
                bool enable = dal.CheckDelete(m.ID);
                m.IsDelete = enable;
                lstcheckdelete.Add(m);
            }
            model.MainCategories = lstcheckdelete;

            if (lst.Count > 0)
            {
                List<ListItem> lstPager = Common.generatePager(TotalRowCount, pageSize, currentpage);
                model.dlPager = lstPager;
            }
            return View(model);
        }

        // GET: MainCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory mainCategory = db.MainCategories.Find(id);
            if (mainCategory == null)
            {
                return HttpNotFound();
            }
            return View(mainCategory);
        }

        // GET: MainCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MainCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,SortOrder,Code")] MainCategory mainCategory)
        {
            if (ModelState.IsValid)
            {
                db.MainCategories.Add(mainCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mainCategory);
        }

        // GET: MainCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory mainCategory = db.MainCategories.Find(id);
            if (mainCategory == null)
            {
                return HttpNotFound();
            }
            return View(mainCategory);
        }

        // POST: MainCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,SortOrder,Code")] MainCategory mainCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mainCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mainCategory);
        }

        // GET: MainCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainCategory mainCategory = db.MainCategories.Find(id);
            if (mainCategory == null)
            {
                return HttpNotFound();
            }
            return View(mainCategory);
        }

        // POST: MainCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MainCategory mainCategory = db.MainCategories.Find(id);
            db.MainCategories.Remove(mainCategory);
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
