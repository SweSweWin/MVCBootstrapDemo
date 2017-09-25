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
    public class SubCategoriesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        SubCategoryDAL dal = new SubCategoryDAL();

        // GET: SubCategories
        [HttpGet]
        public ActionResult SubCategories(int?Page)
        {
            SubCategoryModel model = new SubCategoryModel();
            model.PageNumber = (Page == null ? 1 : Convert.ToInt32(Page));
            int currentPage = model.PageNumber;
            int pageSize = Common.pagesize;
            int TotalRowCount = 0;
            List<SubCategoryModel> lst = dal.SelectAllDataForBindGrid(currentPage, pageSize, out TotalRowCount);
           
            model.SubCategories = lst;

            if (lst.Count > 0)
            {
                List<ListItem> lstPager = Common.generatePager(TotalRowCount, pageSize, currentPage);
                model.dlPager = lstPager;
            }
            return View(model);
        }

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        private IEnumerable<SelectListItem> GetAllMaincategory()
        {
            var list = new MainCategory();
            IEnumerable<SelectListItem> slItem = from s in db.MainCategories
                                                 select new SelectListItem
                                                 {
                                                     Selected = s.ID.ToString() == "Active",
                                                     Text = s.Name,
                                                     Value = s.ID.ToString()
                                                 };
            return slItem;
        }

        private IEnumerable<SelectListItem> GetAllCategoryByMID(int id)
        {
            var list = new MainCategory();
            IEnumerable<SelectListItem> slItem = from s in db.Categories
                                                 where s.MainCategory==id
                                                 select new SelectListItem
                                                 {
                                                     Selected = s.ID.ToString() == "Active",
                                                     Text = s.Name,
                                                     Value = s.ID.ToString()
                                                 };
            return slItem;
        }


        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Category,Name,SortOrder")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Category,Name,SortOrder")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.SubCategories.Find(id);
            db.SubCategories.Remove(subCategory);
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
