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
    public class PositionsController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        PositionDAL dal = new PositionDAL();

        // GET: Positions
        public ActionResult Positions(int?page)
        {
            PositionModel model = new PositionModel();
            model.pageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int currentPage = model.pageNumber;
            int pageSize = Common.pagesize;
            int totalRowCount = 0;
            List<PositionModel> lst = dal.BindData(currentPage, pageSize, out totalRowCount);
            List<PositionModel> chkdeleteList = new List<PositionModel>();
            foreach(var p in lst)
            {
                bool enable = dal.CheckDelete(p.Pos_ID);
                p.IsDelete = enable;
                chkdeleteList.Add(p);
            }
            model.positions = chkdeleteList;
            if(lst.Count>0)
            {
                List<ListItem> pager = Common.generatePager(totalRowCount, pageSize, currentPage);
                model.dlPager = pager;
            }
            return View(model);
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pos_ID,Pos_Name,Pos_SortOrder")] Position position)
        {
            if (ModelState.IsValid)
            {
                db.Positions.Add(position);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(position);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pos_ID,Pos_Name,Pos_SortOrder")] Position position)
        {
            if (ModelState.IsValid)
            {
                db.Entry(position).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(position);
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Position position = db.Positions.Find(id);
            db.Positions.Remove(position);
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
