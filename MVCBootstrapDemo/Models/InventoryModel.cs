using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class InventoryModel : Inventory
    {
        public InventoryModel()
        {
            DeptID = 0;
            Total = "0";
            PerCur = "";
            GrandTotal = "0";
            lblCurCode = "";
            RAmount = "0";
            i = 0;
        }
        public string Value { get; set; }
        public string ReturnSellAmountValue { get; set; }
        public int No { get; set; }
        public int pageNumber { get; set; }
        public int TypeModelID { get; set; }

        public List<ListItem> dlpager { get; set; }

        public List<InventoryModel> inventories { get; set; }

        public int i { get; set; }
        public List<StockTotal> lstQty { get; set; }
        public List<StockTotal> lstAmt { get; set; }
        public List<StockTotal> lstReturn { get; set; }     

        public IEnumerable<SelectListItem> ddlMaincategory { get; set; }
        public IEnumerable<SelectListItem> ddlCategory { get; set; }
        public IEnumerable<SelectListItem> ddlDepartment { get; set; }
        public IEnumerable<SelectListItem> ddlTypeID { get; set; }
        public IEnumerable<SelectListItem> ddlEmp { get; set; }
        public IEnumerable<SelectListItem> ddlTypeModel { get; set; }
        public IEnumerable<SelectListItem> ddlCurrency { get; set; }

        public int rdo { get; set; }
        public List<RadioType> radiotypes { get; set; }
        public List<RadioType> llradiotypes { get; set; }


        //stock total
        public int MainID { get; set; }
        public int CurID { get; set; }
        public int Qty { get; set; }
        public string MainCategory { get; set; }
        
        public string UserName { get; set; }
        public int DeptID { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }      
        public string CatName { get; set; }
        public string UnitType { get; set; }
        public string CurCode { get; set; }//
        public string RetrunCurCode { get; set; }

        public int UType { get; set; }
        public string UName { get; set; }
        public string StatusName { get; set; }
        public string CuName { get; set; }
        public string RCuName { get; set; }
        public string StartCode { get; set; }
        public string EmpName { get; set; }

        public string Total { get; set; }
        public string PerCur { get; set; }
        public string GrandTotal { get; set; }
        public string lblCurCode { get; set; }//
        public string RAmount { get; set; }
        public string RCurCode { get; set; }
        
    }
    
}