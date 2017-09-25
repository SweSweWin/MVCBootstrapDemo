using MVCBootstrapDemo.App_Start;
using MVCBootstrapDemo.Models;
using MVCBootstrapDemo.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Views
{
    public partial class InventoryList : System.Web.UI.Page
    {
        int pageSize = Common.pagesize;
        InventoryDAL dal_inventory = new InventoryDAL();

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMainCategory();
                BindCategory();
                BindDeparments();
                BindSizeOrModel();

                ViewState["PageNo"] = 1;
                BindGrid(1);
                //BindExcel();
            }

            if (gvList.Rows.Count > 0)
            {
                gvList.UseAccessibleHeader = true;
                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #region BindMainCategory
        private void BindMainCategory()
        {
            MainCategoryDAL dal = new MainCategoryDAL();
            List<MainCategoryModel> lst = dal.BindData();

            ddlMainCategory.DataSource = lst;
            ddlMainCategory.DataTextField = "Name";
            ddlMainCategory.DataValueField = "ID";
            ddlMainCategory.DataBind();
            ddlMainCategory.Items.Insert(0, new ListItem("Select One", ""));
        }
        #endregion

        #region BindCategory
        private void BindCategory()
        {
            int id = 0;
            Int32.TryParse(ddlMainCategory.SelectedValue, out id);

            CategoryDAL dal = new CategoryDAL();
            List<CategoryModel> lst = dal.SelectDataByMainCategoryID(id);

            ddlCategory.DataSource = lst;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "ID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Select One", ""));
        }
        #endregion

        #region BindDeparments
        private void BindDeparments()
        {
            DepartmentDAL dal = new DepartmentDAL();
            List<DepartmentModel> lst = dal.BindData();

            ddlDepartment.DataSource = lst;
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "ID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("Select One", ""));
        }
        #endregion

        #region BindSizeOrModel
        private void BindSizeOrModel()
        {
            int id = 0;
            Int32.TryParse(ddlMainCategory.SelectedValue, out id);
            UnitTypeDAL dal = new UnitTypeDAL();
            List<UnitTypeModel> lst = dal.BindDataByMainCat(id);

            ddlSizeOrModel.DataSource = lst;
            ddlSizeOrModel.DataTextField = "Name";
            ddlSizeOrModel.DataValueField = "ID";
            ddlSizeOrModel.DataBind();
            ddlSizeOrModel.Items.Insert(0, new ListItem("Select One", ""));
        }
        #endregion

        #region BindGrid
        private void BindGrid(int currentPage)
        {

            message.Attributes.Add("style", "display:none");
            lblMessage.Text = "";
            int main = 0;
            Int32.TryParse(ddlMainCategory.SelectedValue, out main);
            hdDep.Value = main.ToString();
            int cat = 0;
            Int32.TryParse(ddlCategory.SelectedValue, out cat);
            int dept = 0;
            Int32.TryParse(ddlDepartment.SelectedValue, out dept);
            int type = 0;
            Int32.TryParse(ddlSizeOrModel.SelectedValue, out type);
            int status = 0;
            Int32.TryParse(rdoType.Value, out status);
            int totalRowCount = 0;


            InventoryDAL dal = new InventoryDAL();
            List<InventoryModel> lst = new List<InventoryModel>();
            List<ListItem> item = new List<ListItem>();
            List<ListItem> itemR = new List<ListItem>();

            lst = dal.SelectAllData(status, txtCode.Text, main, cat, dept, type, currentPage, pageSize, out item, out itemR,out totalRowCount);
           

            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                if (ddlDepartment.Items.FindByValue(lst[0].DeptID.ToString()) != null)
                {
                    ddlDepartment.SelectedValue = lst[0].DeptID.ToString();
                }
            }

            if (ViewState["PageNo"].ToString() == "1")
            {
                hdNo.Value = "1";
            }
            else
            {
                hdNo.Value = ViewState["PageNo"].ToString();
            }


            gvList.DataSource = lst;
            gvList.DataBind();


            if (gvList.Rows.Count > 0)
            {
                gvList.UseAccessibleHeader = true;
                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;

                Label lblTotal = (Label)gvList.FooterRow.FindControl("lblTotal");
                Label lblPerCur = (Label)gvList.FooterRow.FindControl("lblPerCur");

                var qry = (from t in lst
                           group t by new { t.CurCode } into g
                           select new { Text = g.Key.CurCode, Value = g.Sum(s => s.Amount).Value }).ToList();
                int count = 0;
                foreach (var res in qry)
                {
                    if (count == 0)
                    {
                        lblTotal.Text = res.Value.ToString("#,#");
                        lblPerCur.Text = res.Text;
                    }
                    else
                    {
                        lblTotal.Text = "<br/>" + res.Value.ToString("#,#");
                        lblPerCur.Text = "<br/>" + res.Text;
                    }
                    count++;
                }

                Label lblGrandTotal = (Label)gvList.FooterRow.FindControl("lblGrandTotal");
                Label lblCurCode = (Label)gvList.FooterRow.FindControl("lblCurCode");

                for (int i = 0; i < item.Count; i++)
                {
                    decimal amt = 0;
                    if (item.Count == 1)
                    {
                        Decimal.TryParse(item[i].Value, out amt);
                        lblGrandTotal.Text = amt.ToString("#,#");
                        lblCurCode.Text = item[i].Text;
                    }
                    else
                    {
                        Decimal.TryParse(item[i].Value, out amt);
                        lblGrandTotal.Text = "<br/>" + amt.ToString("#,#");
                        lblCurCode.Text = "<br/>" + item[i].Text;
                    }
                }


                if (rdoDemage.Checked)
                {
                    Label lblRAmount = (Label)gvList.FooterRow.FindControl("lblRAmount");
                    Label lblRCurCode = (Label)gvList.FooterRow.FindControl("lblRCurCode");
                    lblRAmount.Text = "0";
                    for (int i = 0; i < itemR.Count; i++)
                    {
                        decimal amt = 0;
                        if (itemR.Count == 1)
                        {
                            Decimal.TryParse(itemR[i].Value, out amt);
                            lblRAmount.Text = amt.ToString("#,#");
                            lblRCurCode.Text = itemR[i].Text;
                        }
                        else
                        {
                            Decimal.TryParse(itemR[i].Value, out amt);
                            lblRAmount.Text = "<br/>" + amt.ToString("#,#");
                            lblRCurCode.Text = "<br/>" + itemR[i].Text;
                        }
                    }
                    gvList.Columns[8].Visible = true;
                    gvList.Columns[9].Visible = true;
                }
                else
                {
                    gvList.Columns[8].Visible = false;
                    gvList.Columns[9].Visible = false;
                }
            }
           
            generatePager(totalRowCount, pageSize, currentPage);
        }
        #endregion

        #region CheckedChanged
        protected void group1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoInUse.Checked)
            {
                rdoType.Value = "1";
            }
            else if (rdoNotInUse.Checked)
            {
                rdoType.Value = "2";
            }
            else
            {
                rdoType.Value = "3";
            }
            #region repair1
            ViewState["PageNo"] = 1;
            #endregion
            BindGrid(1);
        }
        #endregion

        #region MainCategory SelectedIndexChanged
        protected void ddlMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCategory();
            BindSizeOrModel();
        }
        #endregion

        #region Search1 Click
        protected void lnkbtnSearch1_Click(object sender, EventArgs e)
        {
            ddlMainCategory.SelectedIndex = -1;
            ddlCategory.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlSizeOrModel.SelectedIndex = -1;

            BindGrid(1);
        }
        #endregion

        #region Search Click
        protected void lnkbtnSearch_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            BindGrid(1);
        }
        #endregion

        #region RowUpdating
        protected void gvList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvList.DataKeys[e.RowIndex].Value;
            Response.Redirect("~/Transactions/Inventory.aspx?id=" + id);
        }
        #endregion

        #region RowDeleting
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = 0;
            Int32.TryParse(gvList.DataKeys[e.RowIndex].Value.ToString(), out id);
            InventoryDAL dal = new InventoryDAL();
            int success = dal.Delete(id);
            if (success > 0)
            {
                message.Attributes.Add("class", "alert alert-success");
                message.Attributes.Add("style", "display:block");
                lblMessage.Text = "<i class='icon-checkmark rd-success'></i> Delete Successful.";
            }
            #region repair3
            ViewState["PageNo"] = 1;
            #endregion
            BindGrid(1);
        }
        #endregion

        #region RowCommand
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                int id = 0;
                Int32.TryParse(e.CommandArgument.ToString(), out id);
                InventoryDAL dal = new InventoryDAL();
                InventoryModel obj = dal.SelectDataByID(id);
                lblCode.Text = obj.CodeNo;
                lblMainCat.Text = obj.MainCategory;
                lblCat.Text = obj.CatName;
                lblPurchaser.Text = obj.Purchaser;
                lblPurchaseDate.Text = obj.PurchaseDate.Value.ToString("dd/MM/yyyy");
                lblUserName.Text = obj.EmpName;
                lblDepartment.Text = obj.Department;
                lblAmount.Text = obj.Amount.Value.ToString("#,##0.00") + " " + obj.CuName;
                lblRAmount.Text = obj.ReturnSellAmount.Value.ToString("#,##0.00") + " " + obj.RCuName;
                lblRemark.Text = obj.Remark;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "detail", "showModal();", true);
            }
            else if (e.CommandName == "Damage")
            {
                int id = 0;
                Int32.TryParse(e.CommandArgument.ToString(), out id);
                Response.Redirect("~/Transactions/Inventory.aspx?rdo=d&id=" + id);
            }
        }
        #endregion

        #region RowDataBound
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = (Label)e.Row.FindControl("lblID");
                LinkButton lnkbtnDelete = (LinkButton)e.Row.FindControl("lnkbtnDelete");
                int id = 0;
                Int32.TryParse(lblID.Text, out id);

                bool enable = dal_inventory.CheckDelete(id);
                lnkbtnDelete.Enabled = enable;
                if (!enable)
                {
                    lnkbtnDelete.OnClientClick = "";
                }
            }
        }
        #endregion

        #region PreRender
        protected void gvList_PreRender(object sender, EventArgs e)
        {
            if (gvList.Rows.Count > 0)
            {
                gvList.UseAccessibleHeader = true;
                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        #endregion

        #region Pagination
        #region dlPager_ItemCommand
        protected void dlPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                ViewState["PageNo"] = e.CommandArgument;
                BindGrid(Convert.ToInt32(e.CommandArgument));
            }
        }
        #endregion

        #region dlPager_ItemDataBound
        protected void dlPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl li = (e.Item.FindControl("lipageno") as HtmlGenericControl);
                LinkButton lnkbtnpageno = (LinkButton)e.Item.FindControl("lnkPageNo");
                if (lnkbtnpageno.Text == ViewState["PageNo"].ToString())
                {
                    li.Attributes.Add("class", "active");

                }
            }
        }
        #endregion

        #region generatePager
        private void generatePager(int TotalRowCount, int PageSize, int CurrentPage)
        {
            List<ListItem> lstPager = Common.generatePager(TotalRowCount, pageSize, CurrentPage);
            dlPager.DataSource = lstPager;
            dlPager.DataBind();
        }
        #endregion

        #endregion

        #region BindExcel
        private void BindExcel()
        {
            message.Attributes.Add("style", "display:none");
            lblMessage.Text = "";
            int main = 0;
            Int32.TryParse(ddlMainCategory.SelectedValue, out main);
            hdDep.Value = main.ToString();
            int cat = 0;
            Int32.TryParse(ddlCategory.SelectedValue, out cat);
            int dept = 0;
            Int32.TryParse(ddlDepartment.SelectedValue, out dept);
            int type = 0;
            Int32.TryParse(ddlSizeOrModel.SelectedValue, out type);
            int status = 0;
            Int32.TryParse(rdoType.Value, out status);


            InventoryDAL dal = new InventoryDAL();
            List<InventoryModel> excel = new List<InventoryModel>();
            List<ListItem> item = new List<ListItem>();
            List<ListItem> itemR = new List<ListItem>();

            excel = dal.SelectDatForExcel(status, txtCode.Text, main, cat, dept, type, out item, out itemR);
        


            gvExcel.DataSource = excel;
            gvExcel.DataBind();


            if (gvExcel.Rows.Count > 0)
            {
                Label lblAmount = (Label)gvExcel.FooterRow.FindControl("Amount");
                Label lblCurCode = (Label)gvExcel.FooterRow.FindControl("CurCode");

                for (int i = 0; i < item.Count; i++)
                {
                    decimal amt = 0;
                    if (item.Count == 1)
                    {
                        Decimal.TryParse(item[i].Value, out amt);
                        lblAmount.Text = amt.ToString("#,#");
                        lblCurCode.Text = item[i].Text;
                    }
                    else
                    {
                        Decimal.TryParse(item[i].Value, out amt);
                        lblAmount.Text = "<br/>" + amt.ToString("#,#");
                        lblCurCode.Text = "<br/>" + item[i].Text;
                    }
                }


                if (rdoDemage.Checked)
                {
                    Label lblRAmount = (Label)gvExcel.FooterRow.FindControl("RAmount");
                    Label lblRCurCode = (Label)gvExcel.FooterRow.FindControl("RCurCode");
                    lblRAmount.Text = "0";
                    for (int i = 0; i < itemR.Count; i++)
                    {
                        decimal amt = 0;
                        if (itemR.Count == 1)
                        {
                            Decimal.TryParse(itemR[i].Value, out amt);
                            lblRAmount.Text = amt.ToString("#,#");
                            lblRCurCode.Text = itemR[i].Text;
                        }
                        else
                        {
                            Decimal.TryParse(itemR[i].Value, out amt);
                            lblRAmount.Text = "<br/>" + amt.ToString("#,#");
                            lblRCurCode.Text = "<br/>" + itemR[i].Text;
                        }
                    }

                    //Visible true gridview columns
                    gvExcel.Columns[8].Visible = true;
                    gvExcel.Columns[9].Visible = true;
                }
                else
                {
                    //Visible false gridview columns
                    gvExcel.Columns[8].Visible = false;
                    gvExcel.Columns[9].Visible = false;
                }
            }

           
          
        }
        #endregion

        #region lnkPrint
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.Ticks + "StockList.xls");
            Response.Charset = "";

            Response.ContentType = "application/vnd.xls";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            BindExcel();
            gvExcel.RenderControl(htmlWriter);
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.Write(stringWriter.ToString());

            Response.End();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

    }
}