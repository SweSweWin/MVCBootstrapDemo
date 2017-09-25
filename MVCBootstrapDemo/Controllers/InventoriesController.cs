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
using static MVCBootstrapDemo.Models.InventoryModel;
using MVCBootstrapDemo.Models.DAL;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace MVCBootstrapDemo.Controllers
{
    public class InventoriesController : Controller
    {
        private BootstrapMVCEntities db = new BootstrapMVCEntities();
        InventoryDAL dal = new InventoryDAL();

        // GET: Inventories
        [HttpGet]
        public async Task<ActionResult> Stock(int? page,int? Maincategory, int? Category,int? DeptID, int? TypeID,int? Status, string CodeNo)
        {
            InventoryModel model = new InventoryModel();
            model.pageNumber = (page == null ? 1 : Convert.ToInt32(page));
            int currentPage = model.pageNumber;
            int pageSize = Common.pagesize;
            int totalRowCount = 0;

            model.ddlMaincategory = GetddlMainCategory();          
            int cat = (Category == null ? 0 : Convert.ToInt32(Category));
            int dept = (DeptID == null ? 0 : Convert.ToInt32(DeptID));
            int type = (TypeID == null ? 0 : Convert.ToInt32(TypeID));
            model.Maincategory= (Maincategory == null ? 0 : Convert.ToInt32(Maincategory));
            int main = model.Maincategory.Value;
            model.Status= (Status == null ? 1 : Convert.ToInt32(Status));
            int status =model.Status.Value;         
            model.ddlCategory = GetddlCategory(main);
            model.ddlDepartment = GetddlDepartment();
            model.ddlTypeID = GetddlTypeID(main);
            List<RadioType> l = new List<RadioType>
        {
            new RadioType{ID=1 , Name = "In Use"},
            new RadioType{ID=2 , Name = "Not In Use"},
            new RadioType{ID=3 , Name="Damage"}
        };
            model.radiotypes = l;            
            model.Category = cat;
            model.TypeID = type;
            model.DeptID = dept;            
            model.CodeNo = CodeNo;
            List<ListItem> item = new List<ListItem>();
            List<ListItem> itemR = new List<ListItem>();

            List<InventoryModel>lst=dal.SelectAllData(status, CodeNo, main, cat, dept, type, currentPage, pageSize, out item, out itemR,out totalRowCount);
         

            var qry = (from t in lst
                       group t by new { t.CurCode } into g
                       select new { Text = g.Key.CurCode, Value = g.Sum(s => s.Amount).Value }).ToList();

            int count = 0;
            foreach (var res in qry)
            {
                if (count == 0)
                {
                    model.Total = res.Value.ToString("#,#");
                    model.PerCur= res.Text;
                }
                else
                {
                    model.Total = "<br/>" + res.Value.ToString("#,#");
                    model.PerCur = "<br/>" + res.Text;
                }
                count++;
            }

               
            for (int i = 0; i < itemR.Count; i++)
                {
                    decimal amt = 0;
                    if (itemR.Count == 1)
                    {
                        Decimal.TryParse(itemR[i].Value, out amt);
                        model.RAmount = amt.ToString("#,#");
                        model.RCurCode = itemR[i].Text;
                    }
                    else
                    {
                        Decimal.TryParse(itemR[i].Value, out amt);
                        model.RAmount = "<br/>" + amt.ToString("#,#");
                        model.RCurCode = "<br/>" + itemR[i].Text;
                    }
                }
               

            for (int i = 0; i < item.Count; i++)
            {
                decimal amt = 0;
                if (item.Count == 1)
                {
                    Decimal.TryParse(item[i].Value, out amt);
                    model.GrandTotal = amt.ToString("#,#");
                    model.lblCurCode = item[i].Text;
                }
                else
                {
                    Decimal.TryParse(item[i].Value, out amt);
                    model.GrandTotal = "<br/>" + amt.ToString("#,#");
                    model.lblCurCode = "<br/>" + item[i].Text;
                }
            }

            List<InventoryModel> lstr = new List<InventoryModel>();
            foreach(var i in lst)
            {               
                i.Value =i.Amount.Value.ToString("#,#");
                i.ReturnSellAmountValue = i.ReturnSellAmount.Value.ToString("#,#");
                lstr.Add(i);
            }

            model.inventories = lstr;
            if(lst.Count>0)
            {
                List<ListItem> pager = Common.generatePager(totalRowCount, pageSize, currentPage);
                model.dlpager = pager;
            }


            return View(model);
        }        

        private IEnumerable<SelectListItem> GetddlTypeID(int mainid)
        {
            IEnumerable<SelectListItem> lst = (from u in db.UnitTypes
                                               join inv in db.Inventories on u.ID equals inv.TypeID
                                               join main in db.MainCategories on inv.Maincategory equals main.ID
                                               where main.ID == mainid
                                               group u by new { u.ID, u.Name, u.SortOrder } into g
                                               orderby g.Key.SortOrder
                                               select new SelectListItem
                                               {
                                                   Selected = g.Key.ID.ToString() == "Active",
                                                   Text = g.Key.Name,
                                                   Value = g.Key.ID.ToString(),

                                               });
            return lst;
        }

        private IEnumerable<SelectListItem> GetddlDepartment()
        {
            IEnumerable<SelectListItem> lst = from d in db.Departments
                                              select new SelectListItem
                                              {
                                                  Selected = d.ID.ToString() == "Active",
                                                  Text = d.Name,
                                                  Value = d.ID.ToString()
                                              };
            return lst;
        }

        private IEnumerable<SelectListItem> GetddlCategory(int mainid)
        {
            IEnumerable<SelectListItem> lst = from c in db.Categories
                                              where c.MainCategory == mainid
                                              select new SelectListItem
                                              {
                                                  Selected=c.ID.ToString()=="Active",
                                                  Text=c.Name,
                                                  Value=c.ID.ToString()
                                              };
            return lst;
        }

        private IEnumerable<SelectListItem> GetddlMainCategory()
        {
            IEnumerable<SelectListItem> lst = from mc in db.MainCategories
                                              select new SelectListItem
                                              {
                                                  Selected = mc.ID.ToString() == "Active",
                                                  Text = mc.Name,
                                                  Value = mc.ID.ToString()

                                              };

            return lst;
        }
        public ActionResult CategoryList(int? mid)
        {
            int id = (mid == null ? 0 : Convert.ToInt32(mid));
            return RedirectToAction("Stock", new { Maincategory =id });       
        }
        public ActionResult CList(int? mid)
        {
            int id = (mid == null ? 0 : Convert.ToInt32(mid));
            return RedirectToAction("StockTotal", new { Maincategory = id });
        }
        public ActionResult CCList(int? mid)
        {
            int id = (mid == null ? 0 : Convert.ToInt32(mid));
            return RedirectToAction("Create", new { Maincategory = id });
        }
        [HttpPost]
        public ActionResult SomeResult(string SelectedValue)
        {
            var result = SelectedValue;
            return View();
        }

        public ActionResult StockTotal(int? Maincategory, int? Category, int? DeptID, int? Status)
        {
            InventoryModel model = new InventoryModel();           

            model.ddlMaincategory = GetddlMainCategory();
            int cat = (Category == null ? 0 : Convert.ToInt32(Category));
            int dept = (DeptID == null ? 0 : Convert.ToInt32(DeptID));           
            model.Maincategory = (Maincategory == null ? 0 : Convert.ToInt32(Maincategory));
            int main = model.Maincategory.Value;
            model.Status = (Status == null ? 1 : Convert.ToInt32(Status));
            int status = model.Status.Value;
            model.ddlCategory = GetddlCategory(main);
            model.ddlDepartment = GetddlDepartment();
            model.ddlTypeID = GetddlTypeID(main);

            List<RadioType> l = new List<RadioType>
        {
            new RadioType{ID=1 , Name = "In Use"},
            new RadioType{ID=2 , Name = "Not In Use"},
            new RadioType{ID=3 , Name="Damage"}
        };
            model.radiotypes = l;
            model.Category = cat;           
            model.DeptID = dept;
       
            List<StockTotal> lstQty = dal.SelectStockTotal(status, main, cat, dept, 0);            
            var q = (from t in lstQty select t.Qty).ToList();
            model.No = q.Sum();            
            model.lstQty = lstQty;

            #region lstAmt  

            List<StockTotal> lstAmt = dal.SelectStockTotal(status, main, cat, dept, 1);
            List<StockTotal> lstAMR = new List<StockTotal>(); 
            foreach (var item in lstAmt)
            {                
                item.AmountCurrency= item.Amount.ToString("#,#") + " " + item.Currency;               
                lstAMR.Add(item);
            }
            var qry = lstAmt.AsEnumerable().Select((i, index) => new
              StockTotal {
                Currency = i.Currency,
                Amount=i.Amount
            }).ToList();
            var sum = (from t in qry
                       group t by new { t.Currency } into g
                       select new { Text = g.Key.Currency, Value = g.Sum(s => s.Amount) }
                     ).ToList();

            decimal Total = 0;
            for (int i = 0; i < sum.Count; i++)
            {
                decimal amt = 0;
                if (sum.Count == 1)
                {
                    Decimal.TryParse(sum[i].Value.ToString(), out amt);                
                }
                else
                {
                    Decimal.TryParse(sum[i].Value.ToString(), out amt);                
                }
                Total += amt;
                model.Total = Total.ToString("#,#") + " " + sum[i].Text;
            }    
            model.lstAmt = lstAMR;

            #endregion
            #region lstReturn
            List<StockTotal> lstReturn = dal.SelectStockTotal(status, main, cat, dept, 2);
            List<StockTotal> lstReturnR = new List<StockTotal>();
            foreach (var item in lstReturn)
            {
                item.AmountCurrency = item.Amount.ToString("#,#") + " " + item.Currency;
                lstReturnR.Add(item);
            }

            var qrr = lstReturn.AsEnumerable().Select((i, index) => new
             StockTotal
            {
                Currency = i.Currency,
                Amount = i.Amount
            }).ToList();

            var sumr = (from t in qrr
                       group t by new { t.Currency } into g
                       select new { Text = g.Key.Currency, Value = g.Sum(s => s.Amount) }
                     ).ToList();
            decimal RAmount = 0;
            for (int i = 0; i < sumr.Count; i++)
            {
                decimal amt = 0;
                if (sumr.Count == 1)
                {
                    Decimal.TryParse(sumr[i].Value.ToString(), out amt);
                }
                else
                {
                    Decimal.TryParse(sumr[i].Value.ToString(), out amt);
                }
                RAmount += amt;
                model.RAmount = RAmount.ToString("#,#") + " " + sumr[i].Text;
            }
            model.lstReturn = lstReturnR;
            #endregion
            return View(model);
        }

        public ActionResult MoveLocation()
        {
            return View(db.Inventories.ToList());
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create(int? Maincategory, int? Status)
        {
            InventoryModel model = new InventoryModel();
            string maincat = "";
            model.ddlMaincategory = GetddlMainCategory();
            model.Maincategory = (Maincategory == null ? 0 : Convert.ToInt32(Maincategory));
            int main = model.Maincategory.Value;
            var lstresult = dal.CodeByID(main);
            if(lstresult!=null)
            {
                maincat = lstresult.Code;                
            }
            if (maincat != "" && maincat != null)
            {
                model.CodeNo = dal.GenerateCodeNo(maincat);
                model.GenNo = Convert.ToInt32(model.CodeNo.Replace(maincat,""));
            }           
            model.ddlCategory = GetddlCategory(main);            
            model.ddlDepartment = GetddlDepartment();
            model.ddlEmp = GetddlEmp();            
            List<RadioType> l = new List<RadioType>
        {
            new RadioType{ID=4 , Name = "Size"},
            new RadioType{ID=5 , Name = "Model"},          
        };
            model.radiotypes = l;
            model.rdo = 4;
                //Size
                model.ddlTypeID = GetddlType(1);
                //Model
                model.ddlTypeModel = GetddlType(2);
           
            model.ddlCurrency = GetddlCurrency();

            List<RadioType> ll = new List<RadioType>
        {
            new RadioType{ID=1 , Name = "In Use"},
            new RadioType{ID=2 , Name = "Not In Use"},
            new RadioType{ID=3 , Name="Damage"}
        };
            model.llradiotypes = ll;
            model.Status = (Status == null ? 1 : Convert.ToInt32(Status));

            int status = model.Status.Value;


            return View(model);

          
        }

        private IEnumerable<SelectListItem> GetddlCurrency()
        {
            IEnumerable<SelectListItem> lst = from u in db.Currencies                                             
                                              select new SelectListItem
                                              {
                                                  Selected = u.ID.ToString() == "Active",
                                                  Text = u.Name,
                                                  Value = u.ID.ToString()
                                              };
            return lst;
        }

        private IEnumerable<SelectListItem> GetddlType(int status)
        {
            IEnumerable<SelectListItem> lst = from u in db.UnitTypes
                                              where u.Type == status
                                              orderby u.SortOrder
                                              select new SelectListItem
                                              {
                                                  Selected = u.ID.ToString() == "Active",
                                                  Text = u.Name,
                                                  Value = u.ID.ToString()
                                              };
            return lst;

        }

        private IEnumerable<SelectListItem> GetddlEmp()
        {
            IEnumerable<SelectListItem> lst = from e in db.Employees
                                              select new SelectListItem
                                              {
                                                  Selected = e.E_ID.ToString() == "Active",
                                                  Text = e.E_FullName,
                                                  Value = e.E_ID.ToString()
                                              };
            return lst;
        }



        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CodeNo,GenNo,Maincategory,Category,TypeID,Purchaser,PurchaseDate,UseDate,EmpID,Status,Amount,Currency,Remark,ReturnSellAmount,ReturnCurrency")] Inventory inventory)
        {
            Inventory obj = new Inventory();
            obj.ID = inventory.ID;
            obj.CodeNo = inventory.CodeNo;
            obj.GenNo = inventory.GenNo;
            obj.Maincategory = inventory.Maincategory;
            obj.Category = inventory.Category;
            obj.TypeID =inventory.TypeID;
            obj.Purchaser = inventory.Purchaser;
            obj.PurchaseDate = inventory.PurchaseDate;
            obj.UseDate = inventory.UseDate;
            obj.EmpID = inventory.EmpID;
            obj.Status = inventory.Status;
            obj.Amount = (inventory.Amount == null ? 0 : inventory.Amount);
            obj.Currency = (inventory.Currency == null ? 1 : inventory.Currency);
            obj.Remark = inventory.Remark;
            obj.ReturnSellAmount=(inventory.ReturnSellAmount==null?0:inventory.ReturnSellAmount);
            obj.ReturnCurrency = (inventory.ReturnCurrency == null ? 1 : inventory.ReturnCurrency);
            if (ModelState.IsValid)
            {
                db.Inventories.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Stock");
            }

            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           // InventoryModel inventory = new InventoryModel();

            InventoryModel inventory = dal.SelectDataByID(Convert.ToInt32(id));
            //int main = inv.Maincategory.Value;
            if (inventory == null)
            {
                return HttpNotFound();
            }
            //inventory.ID = inv.ID;
            //inventory.Maincategory = inv.Maincategory;


            //InventoryModel inventory=
            inventory.ddlMaincategory = GetddlMainCategory();
            inventory.ddlCategory = GetddlCategory(inventory.Maincategory.Value);          
            inventory.ddlDepartment = GetddlDepartment();
            inventory.ddlEmp = GetddlEmp();
            List<RadioType> l = new List<RadioType>
        {
            new RadioType{ID=4 , Name = "Size"},
            new RadioType{ID=5 , Name = "Model"},
        };
            inventory.radiotypes = l;
            if (inventory.UType == 1)
            {
                inventory.rdo = 4;
               
            }
            else if(inventory.UType==2)
            {
                inventory.rdo = 5;
                
            }
            //Size
            inventory.ddlTypeID = GetddlType(1);
            //Model
            inventory.ddlTypeModel = GetddlType(2);

            inventory.ddlCurrency = GetddlCurrency();

            List<RadioType> ll = new List<RadioType>
        {
            new RadioType{ID=1 , Name = "In Use"},
            new RadioType{ID=2 , Name = "Not In Use"},
            new RadioType{ID=3 , Name="Damage"}
        };
            inventory.llradiotypes = ll;
            if(inventory.Status==1)
            {
                inventory.Status = 1;
            }
            else if(inventory.Status==2)
            {
                inventory.Status = 2;
            }
            else if (inventory.Status == 3)
            { 
                inventory.Status = 3;
            }

            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CodeNo,GenNo,Maincategory,Category,TypeID,Purchaser,PurchaseDate,UseDate,EmpID,Status,Amount,Currency,Remark,ReturnSellAmount,ReturnCurrency")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
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
