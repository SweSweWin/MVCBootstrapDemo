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
    public class CurrenciesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        CurrencyDAL dal = new CurrencyDAL();

        // GET: Currencies
        public ActionResult Currencies(int?page)
        {
            CurrencyModel model = new CurrencyModel();
            model.PageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int currentPage = model.PageNumber;
            int pageSize = Common.pagesize;
            int TotalRowCount = 0;
            List<CurrencyModel> lst = dal.GetBindData(currentPage, pageSize,out TotalRowCount);
            List<CurrencyModel> deletecheck = new List<CurrencyModel>();
          
                foreach(var item in lst)
                {
                    bool enable = dal.CheckDelete(item.ID);
                    item.IsDelete = enable;
                    deletecheck.Add(item);
                }
            model.currencies = deletecheck;
            if(lst.Count>0)
            {
                List<ListItem> pager = Common.generatePager(TotalRowCount, pageSize, currentPage);
                model.dlPager = pager;
            }
                
            
            return View(model);
        }

        // GET: Currencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // GET: Currencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,SortOrder")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                db.Currencies.Add(currency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        // GET: Currencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,SortOrder")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Currency currency = db.Currencies.Find(id);
            db.Currencies.Remove(currency);
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
