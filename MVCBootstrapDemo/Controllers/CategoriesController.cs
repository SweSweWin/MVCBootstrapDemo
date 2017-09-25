using MVCBootstrapDemo.App_Start;
using MVCBootstrapDemo.Models;
using MVCBootstrapDemo.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Controllers
{
    public class CategoriesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        CategoryDAL dal = new CategoryDAL();
        // GET: Categories
        [HttpGet]
        public ActionResult Categories(int? page)       
        {
            CategoryModel model = new CategoryModel();
            model.PageNumber = (page == null ? 1 : Convert.ToInt32(page));          
            int currentPage =model.PageNumber;
            int pageSize = Common.pagesize;
            int TotalRowCount = 0;
            List<CategoryModel> lst = dal.SelectAllDataForBindGrid(currentPage, pageSize, out TotalRowCount);
            List<CategoryModel> lstcheckdelete = new List<CategoryModel>();           
            foreach(var c in lst)
            {
                bool enable = dal.CheckDelete(c.ID);
                c.IsDelete = enable;
                lstcheckdelete.Add(c);
            }            
            model.Categories = lstcheckdelete;
            
            if (lst.Count>0)
            {               
                List<ListItem> lstPager = Common.generatePager(TotalRowCount, pageSize, currentPage);
                model.dlPager=lstPager;                
            } 
            
            return View(model);           
        }
     
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel category = dal.SelectDetailDataByID(Convert.ToInt32(id));
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {             
            var c = new CategoryModel
            {
                Maincategories = GetAllMaincategory()
            };
            return View(c);
        }

        private IEnumerable<SelectListItem> GetAllMaincategory()
        {
            //var list = new MainCategory();
            IEnumerable<SelectListItem> slItem = from s in db.MainCategories
                                                 select new SelectListItem
                                                 {
                                                     Selected = s.ID.ToString() == "Active",
                                                     Text = s.Name,
                                                     Value = s.ID.ToString()
                                                 };
            return slItem;
        }
        
        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MainCategory,Name,SortOrder")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryModel category = dal.SelectDataByID(Convert.ToInt32(id));  
          
            if (category == null)
            {
                return HttpNotFound();
            }
         category.Maincategories = GetAllMaincategory();
          return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MainCategory,Name,SortOrder")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
