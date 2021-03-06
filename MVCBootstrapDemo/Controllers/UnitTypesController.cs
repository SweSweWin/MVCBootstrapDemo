﻿using MVCBootstrapDemo.App_Start;
using MVCBootstrapDemo.Models;
using MVCBootstrapDemo.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Controllers
{
    public class UnitTypesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        UnitTypeDAL dal = new UnitTypeDAL();

        // GET: UnitTypes
        public ActionResult UnitTypes(int?page)
        {
            UnitTypeModel model = new UnitTypeModel();
            model.pageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int currentpage = model.pageNumber;
            int pageSize = Common.pagesize;
            int totalRowCount = 0;
            List<UnitTypeModel> lst = dal.BindData(currentpage, pageSize, out totalRowCount);
            List<UnitTypeModel> checkdelete = new List<UnitTypeModel>();
            foreach(var u in lst)
            {
                bool check = dal.CheckDelete(u.ID);
                u.IsDelete = check;
                checkdelete.Add(u);
            }
            model.unittypes = checkdelete;
            if(lst.Count>0)
            {
                List<ListItem> pager = Common.generatePager(totalRowCount, pageSize, currentpage);
                model.dlpager = pager;
            }
            return View(model);
        }

        // GET: UnitTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitTypeModel unitType = dal.FindbyID(id);
            if (unitType == null)
            {
                return HttpNotFound();
            }
            return View(unitType);
        }

        // GET: UnitTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,SortOrder,Type")] Models.UnitTypeModel unitType)
        {
           
            if (ModelState.IsValid)
            {
              
                db.UnitTypes.Add(unitType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitType);
        }

        // GET: UnitTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitTypeModel unitType = dal.FindbyID(id);
            if (unitType == null)
            {
                return HttpNotFound();
            }
            return View(unitType);
        }

        // POST: UnitTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,SortOrder,Type")] UnitTypeModel unitType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitType);
        }

        // GET: UnitTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitTypeModel unitType = dal.FindbyID(id);
            if (unitType == null)
            {
                return HttpNotFound();
            }
            return View(unitType);
        }

        // POST: UnitTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitTypeModel unitType = dal.FindbyID(id);
            db.UnitTypes.Remove(unitType);
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
