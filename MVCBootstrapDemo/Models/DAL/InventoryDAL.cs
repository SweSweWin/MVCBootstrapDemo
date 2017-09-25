using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models.DAL
{
    public class InventoryDAL
    {
        #region Declar
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        public List<InventoryModel> SelectAllData(int status, string codeNo, int main, int cat, int dept, int type, int currentPage, int pageSize, out List<ListItem> item, out List<ListItem> itemR, out int totalRowCount)
        {
            List<InventoryModel> lst = new List<InventoryModel>();
            totalRowCount = 0;
            item = new List<ListItem>();
            itemR = new List<ListItem>();

            try
            {
                if (!string.IsNullOrEmpty(codeNo))
                {

                    #region with codeno
                    IEnumerable<InventoryModel> qry = (from inv in db.Inventories
                                                       join emp in db.Employees on inv.EmpID equals emp.E_ID
                                                       join post in db.Positions on emp.E_Position equals post.Pos_ID
                                                       join dep in db.Departments on emp.E_Department equals dep.ID
                                                       join mcat in db.MainCategories on inv.Maincategory equals mcat.ID
                                                       join c in db.Categories on inv.Category equals c.ID
                                                       join ut in db.UnitTypes on inv.TypeID equals ut.ID
                                                       join cur in db.Currencies on inv.Currency equals cur.ID
                                                       join rcur in db.Currencies on inv.ReturnCurrency equals rcur.ID into lstRcur
                                                       from Rcurdata in lstRcur.DefaultIfEmpty()
                                                       where inv.CodeNo == codeNo && inv.Status == status
                                                       orderby dep.Name, mcat.Code
                                                       select new InventoryModel
                                                       {
                                                           ID = inv.ID,
                                                           CodeNo = inv.CodeNo,
                                                           GenNo = inv.GenNo,
                                                           Maincategory = inv.Maincategory,
                                                           Category = inv.Category,
                                                           TypeID = inv.TypeID,
                                                           Purchaser = inv.Purchaser,
                                                           PurchaseDate = inv.PurchaseDate,
                                                           UseDate = inv.UseDate,
                                                           EmpID = inv.EmpID,
                                                           Status = inv.Status,
                                                           Amount = inv.Amount,
                                                           Currency = inv.Currency,
                                                           ReturnSellAmount = inv.ReturnSellAmount,
                                                           ReturnCurrency = inv.ReturnCurrency,
                                                           Remark = inv.Remark,
                                                           MainCategory = mcat.Name,
                                                           CatName = c.Name,
                                                           UnitType = ut.Name,
                                                           UserName = emp.E_FullName,
                                                           DeptID = emp.E_Department.Value,
                                                           Department = dep.Name,
                                                           Position = post.Pos_Name,
                                                           CurCode = cur.Name,
                                                           RetrunCurCode = Rcurdata == null ? "" : Rcurdata.Name
                                                       });
                    totalRowCount = qry.Count();
                    qry = qry.AsEnumerable().Select((i, index) => new InventoryModel
                    {
                        ID = i.ID,
                        CodeNo = i.CodeNo,
                        GenNo = i.GenNo,
                        Maincategory = i.Maincategory,
                        Category = i.Category,
                        TypeID = i.TypeID,
                        Purchaser = i.Purchaser,
                        PurchaseDate = i.PurchaseDate,
                        UseDate = i.UseDate,
                        EmpID = i.EmpID,
                        Status = i.Status,
                        Amount = i.Amount,
                        Currency = i.Currency,
                        ReturnSellAmount = i.ReturnSellAmount,
                        ReturnCurrency = i.ReturnCurrency,
                        Remark = i.Remark,
                        MainCategory = i.MainCategory,
                        CatName = i.CatName,
                        UnitType = i.UnitType,
                        UserName = i.UserName,
                        DeptID = i.DeptID,
                        Department = i.Department,
                        Position = i.Position,
                        CurCode = i.CurCode,
                        RetrunCurCode = i.RetrunCurCode,
                        No = ++index
                    });
                    qry = qry.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    lst = qry.ToList();

                    item = (from lett in
                               (from inv in db.Inventories
                                join emp in db.Employees on inv.EmpID equals emp.E_ID
                                join post in db.Positions on emp.E_Position equals post.Pos_ID
                                join dep in db.Departments on emp.E_Department equals dep.ID
                                join cur in db.Currencies on inv.Currency equals cur.ID
                                where inv.CodeNo == codeNo && inv.Status == status
                                select new { inv, cur })
                            group lett by new { lett.cur.ID, lett.cur.Name } into g
                            select new
                            {
                                Text = g.Key.Name,
                                Value = g.Sum(s => s.inv.Amount).Value
                            }).ToList().AsEnumerable().Select(s =>
                                    new ListItem
                                    {
                                        Text = s.Text,
                                        Value = s.Value.ToString()
                                    }).ToList();

                    itemR = (from lett in
                               (from inv in db.Inventories
                                join emp in db.Employees on inv.EmpID equals emp.E_ID
                                join post in db.Positions on emp.E_Position equals post.Pos_ID
                                join dep in db.Departments on emp.E_Department equals dep.ID
                                join cur in db.Currencies on inv.ReturnCurrency equals cur.ID
                                where inv.CodeNo == codeNo && inv.Status == status
                                select new { inv, cur })
                             group lett by new { lett.cur.ID, lett.cur.Name } into g
                             select new
                             {
                                 Text = g.Key.Name,
                                 Value = g.Sum(s => s.inv.ReturnSellAmount).Value
                             }).ToList().AsEnumerable().Select(s =>
                                     new ListItem
                                     {
                                         Text = s.Text,
                                         Value = s.Value.ToString()
                                     }).ToList();
                    #endregion
                }
                else
                {
                    #region without codeno
                    IEnumerable<InventoryModel> qry = (from inv in db.Inventories
                                                       join emp in db.Employees on inv.EmpID equals emp.E_ID
                                                       join post in db.Positions on emp.E_Position equals post.Pos_ID
                                                       join dep in db.Departments on emp.E_Department equals dep.ID
                                                       join mcat in db.MainCategories on inv.Maincategory equals mcat.ID
                                                       join c in db.Categories on inv.Category equals c.ID
                                                       join ut in db.UnitTypes on inv.TypeID equals ut.ID
                                                       join cur in db.Currencies on inv.Currency equals cur.ID
                                                       join rcur in db.Currencies on inv.ReturnCurrency equals rcur.ID into lstRcur
                                                       from Rcurdata in lstRcur.DefaultIfEmpty()
                                                       where inv.Maincategory == (main == 0 ? inv.Maincategory : main)
                                                       && inv.Category == (cat == 0 ? inv.Category : cat)
                                                       && dep.ID == (dept == 0 ? dep.ID : dept)
                                                       && inv.Status == status
                                                       && ut.ID == (type == 0 ? ut.ID : type)
                                                       orderby dep.Name, mcat.Code
                                                       select new InventoryModel
                                                       {
                                                           ID = inv.ID,
                                                           CodeNo = inv.CodeNo,
                                                           GenNo = inv.GenNo,
                                                           Maincategory = inv.Maincategory,
                                                           Category = inv.Category,
                                                           TypeID = inv.TypeID,
                                                           Purchaser = inv.Purchaser,
                                                           PurchaseDate = inv.PurchaseDate,
                                                           UseDate = inv.UseDate,
                                                           EmpID = inv.EmpID,
                                                           Status = inv.Status,
                                                           Amount = inv.Amount,
                                                           Currency = inv.Currency,
                                                           ReturnSellAmount = inv.ReturnSellAmount,
                                                           ReturnCurrency = inv.ReturnCurrency,
                                                           Remark = inv.Remark,
                                                           MainCategory = mcat.Name,
                                                           CatName = c.Name,
                                                           UnitType = ut.Name,
                                                           UserName = emp.E_FullName,
                                                           DeptID = emp.E_Department.Value,
                                                           Department = dep.Name,
                                                           Position = post.Pos_Name,
                                                           CurCode = cur.Name,
                                                           RetrunCurCode = Rcurdata == null ? "" : Rcurdata.Name
                                                       });
                    totalRowCount = qry.Count();
                    qry = qry.AsEnumerable().Select((i, index) => new InventoryModel
                    {
                        ID = i.ID,
                        CodeNo = i.CodeNo,
                        GenNo = i.GenNo,
                        Maincategory = i.Maincategory,
                        Category = i.Category,
                        TypeID = i.TypeID,
                        Purchaser = i.Purchaser,
                        PurchaseDate = i.PurchaseDate,
                        UseDate = i.UseDate,
                        EmpID = i.EmpID,
                        Status = i.Status,
                        Amount = i.Amount,
                        Currency = i.Currency,
                        ReturnSellAmount = i.ReturnSellAmount,
                        ReturnCurrency = i.ReturnCurrency,
                        Remark = i.Remark,
                        MainCategory = i.MainCategory,
                        CatName = i.CatName,
                        UnitType = i.UnitType,
                        UserName = i.UserName,
                        DeptID = i.DeptID,
                        Department = i.Department,
                        Position = i.Position,
                        CurCode = i.CurCode,
                        RetrunCurCode = i.RetrunCurCode,
                        No = ++index
                    });
                    qry = qry.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                    lst = qry.ToList();

                    item = (from lett in
                               (from inv in db.Inventories
                                join emp in db.Employees on inv.EmpID equals emp.E_ID
                                join post in db.Positions on emp.E_Position equals post.Pos_ID
                                join dep in db.Departments on emp.E_Department equals dep.ID
                                join cur in db.Currencies on inv.Currency equals cur.ID
                                join ut in db.UnitTypes on inv.TypeID equals ut.ID
                                where inv.Maincategory == (main == 0 ? inv.Maincategory : main)
                                  && inv.Category == (cat == 0 ? inv.Category : cat)
                                  && dep.ID == (dept == 0 ? dep.ID : dept)
                                  && inv.Status == status
                                  && ut.ID == (type == 0 ? ut.ID : type)
                                select new { inv, cur })
                            group lett by new { lett.cur.ID, lett.cur.Name } into g
                            select new
                            {
                                Text = g.Key.Name,
                                Value = g.Sum(s => s.inv.Amount).Value
                            }).ToList().AsEnumerable().Select(s =>
                                    new ListItem
                                    {
                                        Text = s.Text,
                                        Value = s.Value.ToString()
                                    }).ToList();

                    itemR = (from lett in
                               (from inv in db.Inventories
                                join emp in db.Employees on inv.EmpID equals emp.E_ID
                                join post in db.Positions on emp.E_Position equals post.Pos_ID
                                join dep in db.Departments on emp.E_Department equals dep.ID
                                join cur in db.Currencies on inv.ReturnCurrency equals cur.ID
                                join ut in db.UnitTypes on inv.TypeID equals ut.ID
                                where inv.Maincategory == (main == 0 ? inv.Maincategory : main)
                                  && inv.Category == (cat == 0 ? inv.Category : cat)
                                  && dep.ID == (dept == 0 ? dep.ID : dept)
                                  && inv.Status == status
                                  && ut.ID == (type == 0 ? ut.ID : type)
                                select new { inv, cur })
                             group lett by new { lett.cur.ID, lett.cur.Name } into g
                             select new
                             {
                                 Text = g.Key.Name,
                                 Value = g.Sum(s => s.inv.ReturnSellAmount).Value
                             }).ToList().AsEnumerable().Select(s =>
                                     new ListItem
                                     {
                                         Text = s.Text,
                                         Value = s.Value.ToString()
                                     }).ToList();
                    #endregion

                }

                return lst;
            }
            catch { }
            return lst;
        }

        public MainCategory CodeByID(int main)
        {
            var query = (from maincat in db.MainCategories where maincat.ID == main select maincat).FirstOrDefault();
            return query;
        }

        public List<StockTotal> SelectStockTotal(int status, int main, int catid, int dept, int type)
        {
            List<StockTotal> lst = new List<StockTotal>();
            try
            {
                if (type == 0)
                {
                    #region Qty
                    lst = (from lett in
                               (from t in db.Inventories
                                join m in db.MainCategories on t.Maincategory equals m.ID
                                join em in db.Employees on t.EmpID equals em.E_ID
                                join dep in db.Departments on em.E_Department equals dep.ID
                                where t.Maincategory == (main == 0 ? t.Maincategory : main)
                                && t.Category == (catid == 0 ? t.Category : catid)
                                && dep.ID == (dept == 0 ? dep.ID : dept)
                                && t.Status == status
                                orderby m.SortOrder
                                select new { t, m })
                           group lett by new { mcatid = lett.m.ID, mcatname = lett.m.Name } into g
                           select new StockTotal
                           {
                               Qty = g.Count(),
                               MainID = g.Key.mcatid,
                               MainCategory = g.Key.mcatname
                           }).ToList().OrderBy(s => s.MainID).ToList();
                    #endregion
                }
                else if (type == 1)
                {
                    #region Total
                    lst = (from lett in
                               (from t in db.Inventories
                                join m in db.MainCategories on t.Maincategory equals m.ID
                                join em in db.Employees on t.EmpID equals em.E_ID
                                join dep in db.Departments on em.E_Department equals dep.ID
                                join cur in db.Currencies on t.Currency equals cur.ID
                                where t.Maincategory == (main == 0 ? t.Maincategory : main)
                                && t.Category == (catid == 0 ? t.Category : catid)
                                && dep.ID == (dept == 0 ? dep.ID : dept)
                                && t.Status == status
                                orderby m.SortOrder, cur.SortOrder
                                select new { t, m, cur })
                           group lett by new { mcatid = lett.m.ID, mcatname = lett.m.Name, curid = lett.cur.ID, curname = lett.cur.Name } into g
                           select new StockTotal
                           {
                               Qty = g.Count(),
                               MainID = g.Key.mcatid,
                               CurID = g.Key.curid,
                               MainCategory = g.Key.mcatname,
                               Currency = g.Key.curname,
                               Amount = g.Sum(s => s.t.Amount).Value
                           }).ToList().OrderBy(s => s.MainID).ToList();
                    #endregion
                }
                else
                {
                    #region Return Total
                    lst = (from lett in
                               (from t in db.Inventories
                                join m in db.MainCategories on t.Maincategory equals m.ID
                                join em in db.Employees on t.EmpID equals em.E_ID
                                join dep in db.Departments on em.E_Department equals dep.ID
                                join cur in db.Currencies on t.ReturnCurrency equals cur.ID
                                where t.Maincategory == (main == 0 ? t.Maincategory : main)
                                && t.Category == (catid == 0 ? t.Category : catid)
                                && dep.ID == (dept == 0 ? dep.ID : dept)
                                && t.Status == status
                                orderby m.SortOrder, cur.SortOrder
                                select new { t, m, cur })
                           group lett by new { mcatid = lett.m.ID, mcatname = lett.m.Name, curid = lett.cur.ID, curname = lett.cur.Name } into g
                           select new StockTotal
                           {
                               Qty = g.Count(),
                               MainID = g.Key.mcatid,
                               CurID = g.Key.curid,
                               MainCategory = g.Key.mcatname,
                               Currency = g.Key.curname,
                               Amount = g.Sum(s => s.t.ReturnSellAmount).Value
                           }).ToList().OrderBy(s => s.MainID).ToList();
                    #endregion
                }
            }
            catch { }
            return lst;

        }

        //public InventoryModel SelectModelDataByID(int id)
        //{
            
        //        InventoryModel Model = new InventoryModel();
        //        try
        //        {

        //        IEnumerable<InventoryModel> qry = (from inv in db.Inventories
        //                                           join emp in db.Employees on inv.EmpID equals emp.E_ID
        //                                           join dep in db.Departments on emp.E_Department equals dep.ID
        //                                           select new InventoryModel
        //                                           {

        //                                           });

        //            Model = (from inv in db.Inventories
        //                      where inv.ID == id
        //                      select new InventoryModel
        //                      {
        //                          ID = inv.ID,
        //                          CodeNo = inv.CodeNo,
        //                          GenNo = inv.GenNo,
        //                          Maincategory = inv.Maincategory,
        //                          Category = inv.Category,
        //                          TypeID = inv.TypeID,
        //                          Purchaser = inv.Purchaser,
        //                          PurchaseDate = inv.PurchaseDate,
        //                          UseDate = inv.UseDate,
        //                          EmpID = inv.EmpID,
        //                          Status = inv.Status,
        //                          Amount = inv.Amount,
        //                          Currency = inv.Currency,
        //                          ReturnSellAmount = inv.ReturnSellAmount,
        //                          ReturnCurrency = inv.ReturnCurrency,
        //                          Remark = inv.Remark,
        //                          //MainCategory = mcat.Name,
        //                          //CatName = c.Name,
        //                          //UnitType = ut.Name,
        //                          //UserName = emp.E_FullName,
        //                          //DeptID = emp.E_Department.Value,
        //                          //Department = dep.Name,
        //                          //Position = post.Pos_Name,
        //                          //CurCode = cur.Name,
        //                          //RetrunCurCode = Rcurdata == null ? "" : Rcurdata.Name
                                  

        //                      }).First();
        //        }
        //        catch
        //        {
        //        }
        //        return Model;
           
        //}

        #region GenerateCodeNo
        public string GenerateCodeNo(string maincat)
        {
            string code = "";
            int genno = 0;
            try
            {
                genno = db.Inventories.Where(s => s.CodeNo.StartsWith(maincat)).Max(s => s.GenNo).Value;
                genno++;
                code = maincat + genno.ToString("00000");
            }
            catch { code = maincat + "00001"; }
            return code;
        }
        #endregion


        #region TestASP

        public bool CheckDelete(int id)
        {
            bool check = true;
            try
            {
                var qry = (from h in db.Histories where h.ActiveID == id select h.ActiveID).ToList();
                if (qry.Count > 0)
                    check = false;
            }
            catch { }
            return check;

        }

       
       

    public int Delete(int id)
        {
            int success = 0;
            try
            {
                Inventory inv = (from invg in db.Inventories where invg.ID == id select invg).First<Inventory>();
                if (inv != null)
                {
                    db.Inventories.Remove(inv);
                    success = db.SaveChanges();

                }
            }
            catch { }
            return success;

        }

        public InventoryModel SelectDataByID(int id)
        {
            InventoryModel invModel = new InventoryModel();
            try
            {
                invModel = (from inv in db.Inventories
                            where inv.ID == id
                            join cu in db.Currencies on inv.Currency equals cu.ID
                            join cus in db.Currencies on inv.ReturnCurrency equals cus.ID
                            join c in db.Categories on inv.Category equals c.ID
                            join m in db.MainCategories on c.MainCategory equals m.ID
                            join t in db.UnitTypes on inv.TypeID equals t.ID
                            join e in db.Employees on inv.EmpID equals e.E_ID
                            join d in db.Departments on e.E_Department equals d.ID
                            select new InventoryModel
                            {

                                ID = inv.ID,
                                CodeNo = inv.CodeNo,
                                GenNo = inv.GenNo.Value,
                                Maincategory = inv.Maincategory.Value,
                                MainCategory = m.Name,
                                Category = inv.Category.Value,
                                CatName = c.Name,
                                TypeID = inv.TypeID.Value,
                                UType = t.Type.Value,
                                UnitType = t.Type.Value == 1 ? "Size" : "Model",
                                UName = t.Name,
                                Purchaser = inv.Purchaser,
                                PurchaseDate = inv.PurchaseDate.Value,
                                UseDate = inv.UseDate.Value,
                                DeptID = e.E_Department.Value,
                                EmpName = e.E_FullName,
                                Department = d.Name,
                                EmpID = inv.EmpID.Value,
                                Status = inv.Status.Value,
                                StatusName = inv.Status == 0 ? "" : inv.Status == 1 ? "In Use" : inv.Status == 2 ? "Not In Use" : inv.Status == 3 ? "Damage" : "",//For Status Name
                                Amount = inv.Amount.Value,
                                Currency = inv.Currency.Value,
                                CuName = cu.Name,
                                ReturnSellAmount = inv.ReturnSellAmount.Value,
                                ReturnCurrency = inv.ReturnCurrency.Value,
                                RCuName = cus.Name,
                                Remark = inv.Remark,
                                StartCode = m.Code
                            }).First();
            }
            catch { }
            return invModel;

        }



        public List<InventoryModel> SelectDatForExcel(int status, string code, int main, int cat, int dept, int type, out List<ListItem> item, out List<ListItem> itemR)
        {
            List<InventoryModel> lst = new List<InventoryModel>();
            item = new List<ListItem>();
            itemR = new List<ListItem>();
            try
            {
                if (!string.IsNullOrEmpty(code))
                {

                    #region with code
                    lst = (from inv in db.Inventories
                           join emp in db.Employees on inv.EmpID equals emp.E_ID
                           join post in db.Positions on emp.E_Position equals post.Pos_ID
                           join dep in db.Departments on emp.E_Department equals dep.ID
                           join mcat in db.MainCategories on inv.Maincategory equals mcat.ID
                           join c in db.Categories on inv.Category equals c.ID
                           join ut in db.UnitTypes on inv.TypeID equals ut.ID
                           join cur in db.Currencies on inv.Currency equals cur.ID
                           join rcur in db.Currencies on inv.ReturnCurrency equals rcur.ID into lstRcur
                           from Rcurdata in lstRcur.DefaultIfEmpty()
                           where inv.CodeNo == code && inv.Status == status
                           orderby dep.Name, mcat.Code
                           select new InventoryModel
                           {
                               ID = inv.ID,
                               CodeNo = inv.CodeNo,
                               GenNo = inv.GenNo,
                               Maincategory = inv.Maincategory,
                               Category = inv.Category,
                               TypeID = inv.TypeID,
                               Purchaser = inv.Purchaser,
                               PurchaseDate = inv.PurchaseDate,
                               UseDate = inv.UseDate,
                               EmpID = inv.EmpID,
                               Status = inv.Status,
                               Amount = inv.Amount,
                               Currency = inv.Currency,
                               ReturnSellAmount = inv.ReturnSellAmount,
                               ReturnCurrency = inv.ReturnCurrency,
                               Remark = inv.Remark,
                               MainCategory = mcat.Name,
                               CatName = c.Name,
                               UnitType = ut.Name,
                               UserName = emp.E_FullName,
                               DeptID = emp.E_Department.Value,
                               Department = dep.Name,
                               Position = post.Pos_Name,
                               CurCode = cur.Name,
                               RetrunCurCode = Rcurdata == null ? "" : Rcurdata.Name
                           }).ToList();

                    item = (from lett in
                                 (from inv in db.Inventories
                                  join emp in db.Employees on inv.EmpID equals emp.E_ID
                                  join post in db.Positions on emp.E_Position equals post.Pos_ID
                                  join dep in db.Departments on emp.E_Department equals dep.ID
                                  join cur in db.Currencies on inv.Currency equals cur.ID
                                  where inv.CodeNo == code && inv.Status == status
                                  select new { inv, cur })
                            group lett by new { lett.cur.ID, lett.cur.Name } into g
                            select new
                            {
                                Text = g.Key.Name,
                                Value = g.Sum(s => s.inv.Amount).Value
                            }).ToList().AsEnumerable().Select(s =>
                                  new ListItem
                                  {
                                      Text = s.Text,
                                      Value = s.Value.ToString()
                                  }).ToList();

                    itemR = (from lett in
                                       (from inv in db.Inventories
                                        join emp in db.Employees on inv.EmpID equals emp.E_ID
                                        join post in db.Positions on emp.E_Position equals post.Pos_ID
                                        join dep in db.Departments on emp.E_Department equals dep.ID
                                        join cur in db.Currencies on inv.ReturnCurrency equals cur.ID
                                        where inv.CodeNo == code && inv.Status == status
                                        select new { inv, cur })
                             group lett by new { lett.cur.ID, lett.cur.Name } into g
                             select new
                             {
                                 Text = g.Key.Name,
                                 Value = g.Sum(s => s.inv.ReturnSellAmount).Value
                             }).ToList().AsEnumerable().Select(s =>
                             new ListItem
                             {
                                 Text = s.Text,
                                 Value = s.Value.ToString()
                             }).ToList();
                    #endregion
                }
                else
                {
                    #region without code
                    lst = (from inv in db.Inventories
                           join emp in db.Employees on inv.EmpID equals emp.E_ID
                           join post in db.Positions on emp.E_Position equals post.Pos_ID
                           join dep in db.Departments on emp.E_Department equals dep.ID
                           join mcat in db.MainCategories on inv.Maincategory equals mcat.ID
                           join c in db.Categories on inv.Category equals c.ID
                           join ut in db.UnitTypes on inv.TypeID equals ut.ID
                           join cur in db.Currencies on inv.Currency equals cur.ID
                           join rcur in db.Currencies on inv.ReturnCurrency equals rcur.ID into lstRcur
                           from Rcurdata in lstRcur.DefaultIfEmpty()
                           where inv.Maincategory == (main == 0 ? inv.Maincategory : main)
                           && inv.Category == (cat == 0 ? inv.Category : cat)
                           && dep.ID == (dept == 0 ? dep.ID : dept)
                           && inv.Status == status
                           && ut.ID == (type == 0 ? ut.ID : type)
                           orderby dep.Name, mcat.Code
                           select new InventoryModel
                           {
                               ID = inv.ID,
                               CodeNo = inv.CodeNo,
                               GenNo = inv.GenNo,
                               Maincategory = inv.Maincategory,
                               Category = inv.Category,
                               TypeID = inv.TypeID,
                               Purchaser = inv.Purchaser,
                               PurchaseDate = inv.PurchaseDate,
                               UseDate = inv.UseDate,
                               EmpID = inv.EmpID,
                               Status = inv.Status,
                               Amount = inv.Amount,
                               Currency = inv.Currency,
                               ReturnSellAmount = inv.ReturnSellAmount,
                               ReturnCurrency = inv.ReturnCurrency,
                               Remark = inv.Remark,
                               MainCategory = mcat.Name,
                               CatName = c.Name,
                               UnitType = ut.Name,
                               UserName = emp.E_FullName,
                               DeptID = emp.E_Department.Value,
                               Department = dep.Name,
                               Position = post.Pos_Name,
                               CurCode = cur.Name,
                               RetrunCurCode = Rcurdata == null ? "" : Rcurdata.Name
                           }).ToList();

                    itemR = (from lett in
                                 (from inv in db.Inventories
                                  join emp in db.Employees on inv.EmpID equals emp.E_ID
                                  join post in db.Positions on emp.E_Position equals post.Pos_ID
                                  join dep in db.Departments on emp.E_Department equals dep.ID
                                  join cur in db.Currencies on inv.Currency equals cur.ID
                                  join ut in db.UnitTypes on inv.TypeID equals ut.ID
                                  where inv.Maincategory == (main == 0 ? inv.Maincategory : main)
                                    && inv.Category == (cat == 0 ? inv.Category : cat)
                                    && dep.ID == (dept == 0 ? dep.ID : dept)
                                    && inv.Status == status
                                    && ut.ID == (type == 0 ? ut.ID : type)
                                  select new { inv, cur })
                             group lett by new { lett.cur.ID, lett.cur.Name } into g
                             select new
                             {
                                 Text = g.Key.Name,
                                 Value = g.Sum(s => s.inv.Amount).Value
                             }).ToList().AsEnumerable().Select(s =>
                                   new ListItem
                                   {
                                       Text = s.Text,
                                       Value = s.Value.ToString()
                                   }).ToList();

                    itemR = (from lett in
                                       (from inv in db.Inventories
                                        join emp in db.Employees on inv.EmpID equals emp.E_ID
                                        join post in db.Positions on emp.E_Position equals post.Pos_ID
                                        join dep in db.Departments on emp.E_Department equals dep.ID
                                        join cur in db.Currencies on inv.ReturnCurrency equals cur.ID
                                        join ut in db.UnitTypes on inv.TypeID equals ut.ID
                                        where inv.Maincategory == (main == 0 ? inv.Maincategory : main)
                                          && inv.Category == (cat == 0 ? inv.Category : cat)
                                          && dep.ID == (dept == 0 ? dep.ID : dept)
                                          && inv.Status == status
                                          && ut.ID == (type == 0 ? ut.ID : type)
                                        select new { inv, cur })
                             group lett by new { lett.cur.ID, lett.cur.Name } into g
                             select new
                             {
                                 Text = g.Key.Name,
                                 Value = g.Sum(s => s.inv.ReturnSellAmount).Value
                             }).ToList().AsEnumerable().Select(s =>
                             new ListItem
                             {
                                 Text = s.Text,
                                 Value = s.Value.ToString()
                             }).ToList();
                    #endregion

                }
            }
            catch { }
            return lst;
        }

        #endregion
    }
}