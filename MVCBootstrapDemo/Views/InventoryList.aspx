<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="InventoryList.aspx.cs" Inherits="MVCBootstrapDemo.Views.InventoryList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
       lnkPrint.Attributes["onclick"] = "javascript:CallPrint('divPrint');";
        </script>
    <br />
<div class="page-header">
  <h1>
    <asp:LinkButton runat="server" ID="LinkButton1" PostBackUrl="~/Transactions/Inventory.aspx"><i class="icon-arrow-left-3 fg-darker smaller"></i></asp:LinkButton>
    Stocks Lists
  </h1>
</div>
<hr />
<div class="text-right">
    <asp:LinkButton runat="server" ID="lnkbtnNew" CssClass="button info" PostBackUrl="~/Transactions/Inventory.aspx">New&nbsp;<i class="icon-new"></i></asp:LinkButton>
</div>

<div class="text-left">   
    
</div>

<div class="row">
    <div class="alert alert-success" runat="server" id="message" style="display:none">
      <a href="javascript:void(0)" onclick="Close()" class="close">&times;</a>
      <asp:Label runat="server" ID="lblMessage"></asp:Label>
    </div>
</div>


<div class="grid">
 <asp:HiddenField ID="hdNo" runat="server" Value=""/> 
     <asp:HiddenField ID="hdDep" runat="server" Value=""/> 
     <div class="row">
        <div class="span9"></div>
        <div class="span3">
            <div class="input-control text">
                <label for="MainContent_txtCode">Code :</label>
                <asp:TextBox runat="server" ID="txtCode" Text=""></asp:TextBox>
            </div>
        </div>
        <div class="span2">
            <div style="padding-top:35px">
                <asp:LinkButton runat="server" ID="lnkbtnSearch1" OnClick="lnkbtnSearch1_Click" CssClass="button primary">
                Search&nbsp;<i class="icon-search"></i></asp:LinkButton>
            </div>
        </div>
    </div>
      
     <div class="row">
        <div class="span3">
            <label>Main Category :</label>
            <div class="input-control select">
                <asp:DropDownList ID="ddlMainCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMainCategory_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="span3">
            <label>Category :</label>
            <div class="input-control select">
                <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="span3">
            <label for="MainContent_ddlSizeOrModel">Size/Model:</label>
            <div class="input-control select">
                <asp:DropDownList ID="ddlSizeOrModel" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="span3">
            <label>Departments :</label>
            <div class="input-control select">
                <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="span2">
            <div style="padding-top:35px">
                <asp:LinkButton runat="server" ID="lnkbtnSearch" OnClick="lnkbtnSearch_Click" CssClass="button primary">
                Search&nbsp;<i class="icon-search"></i></asp:LinkButton>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <asp:HiddenField runat="server" ID="rdoType" Value="1"/>
        <div class="input-control radio default-style">
            <label>
                <asp:RadioButton GroupName="group1" runat="server" ID="rdoInUse" Checked="true" AutoPostBack="true" OnCheckedChanged="group1_CheckedChanged" />
                <span class="check"></span>
                In Use
            </label>
        </div>
        <div class="input-control radio default-style">
            <label>
                <asp:RadioButton GroupName="group1" runat="server" ID="rdoNotInUse" AutoPostBack="true" OnCheckedChanged="group1_CheckedChanged"/>
                <span class="check"></span>
                Not In Use
            </label>
        </div>
        <div class="input-control radio default-style">
            <label>
                <asp:RadioButton GroupName="group1" runat="server" ID="rdoDemage" AutoPostBack="true" OnCheckedChanged="group1_CheckedChanged"/>
                <span class="check"></span>
                Damage
            </label>
        </div>
    </div>  

    <div class="row">
        <table width="100%">
            <tr>
                <td align="left">
                   <asp:LinkButton runat="server" ID="lnkPrint" OnClick="lnkPrint_Click" CssClass="button success">
                   <i class="icon-file-excel"></i> Export Excel</asp:LinkButton>
                </td>
                <td align="right">

                    <asp:Repeater runat="server" ID="dlPager" OnItemCommand="dlPager_ItemCommand" OnItemDataBound="dlPager_ItemDataBound">
                        <HeaderTemplate>
                            <div class="pagination">
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li id="lipageno" runat="server">
                                <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                             </ul>
                            </div>
                        </FooterTemplate> 
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    <div class="row">
       
            <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" width="100%" GridLines="None" OnRowDataBound="gvList_RowDataBound"
            CssClass="table hovered border" onrowupdating="gvList_RowUpdating" OnRowDeleting="gvList_RowDeleting"
            OnRowCommand="gvList_RowCommand" OnPreRender="gvList_PreRender"
            EmptyDataText="There is no record." EmptyDataRowStyle-ForeColor="Red" ShowHeaderWhenEmpty="True" DataKeyNames="ID"
            ShowFooter="true">
                     <Columns>
                        <asp:TemplateField HeaderText="No">
                         <ItemTemplate>
                                <center>                              
                              <%--    <%# (Convert.ToInt32(hdNo.Value)-1)*Common.PageSize+Container.DataItemIndex+1%>.--%>
                                   <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>' Visible="false" />
                                </center>
                         </ItemTemplate>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Code No" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Eval("CodeNo")%>
                            </ItemTemplate>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Size/Model" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <span style="font-family:Zawgyi-One;font-size:13px"><%# Eval("UnitType")%></span>
                            </ItemTemplate>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Owner" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Eval("UserName")%> (<%# Eval("Position")%>)
                            </ItemTemplate>
                         </asp:TemplateField>
                          <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Eval("Department")%>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Purchase Date" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("PurchaseDate")).ToString("dd/MM/yyyy")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="row text-right">
                                   <b>Total :</b>
                                </div>
                                <div class="row text-right">
                                    <b>Grand Total :</b>
                                </div>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblAmount" Text='<%# Eval("Amount")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="row text-right">
                                    <b><asp:Label runat="server" ID="lblTotal" ></asp:Label></b>
                                </div>
                                <div class="row text-right">
                                    <b><asp:Label runat="server" ID="lblGrandTotal"></asp:Label></b>
                                </div>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCurCode" Text='<%# Eval("CurCode")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="row text-left">
                                   <b><asp:Label runat="server" ID="lblPerCur"></asp:Label></b>
                                </div>
                                <div class="row text-left">
                                    <b><asp:Label runat="server" ID="lblCurCode"></asp:Label></b>
                                </div>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Return Amount" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                            <ItemTemplate>                 
                                 <%# Eval("ReturnSellAmount")%>
                            </ItemTemplate>
                              <FooterTemplate>
                                <b><asp:Label runat="server" ID="lblRAmount"></asp:Label></b>
                            </FooterTemplate>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Currency" Visible="true" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("RetrunCurCode")%>                            
                            </ItemTemplate>
                            <FooterTemplate>                   
                                <asp:Label runat="server" ID="lblRCurCode" ></asp:Label>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="150px">
                             <ItemTemplate>
                               <center>
                                    <asp:LinkButton ID="lnkbtnView" runat="server" ToolTip="Detail" CommandName="Detail" CommandArgument='<%# Eval("ID")%>'><i class="icon-glasses fg-darker"></i></asp:LinkButton>&nbsp;  
                                    <asp:LinkButton ID="lnkbtnDamage" runat="server" ToolTip="Damage" CommandName="Damage" CommandArgument='<%# Eval("ID")%>'><i class="icon-lightning fg-red"></i></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CommandName="Update"><i class="icon-pencil"></i></asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" CommandName="Delete" OnClientClick="return Confirm('Are you sure you want to delete?')"><i class="icon-remove fg-red"></i></asp:LinkButton>                           
                                </center>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                </asp:GridView>
</div>
     
    <div class="row">
       
    <asp:GridView ID="gvExcel" runat="server" AutoGenerateColumns="False" width="100%"
            EmptyDataText="There is no record." EmptyDataRowStyle-ForeColor="Red" ShowHeaderWhenEmpty="True"
            ShowFooter="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                     <AlternatingRowStyle Font-Names="Zawgyi-One" Font-Size="13px" />
                     <Columns>

                        <asp:TemplateField HeaderText="No">
                         <ItemTemplate>
                                 <center>
                                    <%# Container.DataItemIndex+1 %>.                                
                                </center>
                            </ItemTemplate>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Code No" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Eval("CodeNo")%>
                            </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Size/Model" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <span style="font-family:Zawgyi-One;font-size:13px"><%# Eval("UnitType")%></span>
                            </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Owner" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Eval("UserName")%>(<%# Eval("Position")%>)
                            </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Purchase Date" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("PurchaseDate")).ToString("dd/MM/yyyy")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>Grand Total :</b>
                            </FooterTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("Amount")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b><asp:Label runat="server" ID="Amount"></asp:Label></b>
                            </FooterTemplate>

<FooterStyle HorizontalAlign="Right"></FooterStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("CurCode")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b><asp:Label runat="server" ID="CurCode"></asp:Label></b>
                            </FooterTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                    <%# Eval("Department")%>
                            </ItemTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Return Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                                <%# Eval("ReturnSellAmount")%>                               
                            </ItemTemplate>
                        <FooterTemplate>
                                <b><asp:Label runat="server" ID="RAmount"></asp:Label></b>
                            </FooterTemplate>

<FooterStyle HorizontalAlign="Right"></FooterStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
                         </asp:TemplateField>

                        <asp:TemplateField HeaderText="Currency" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Eval("RetrunCurCode")%>                            
                            </ItemTemplate>
                            <FooterTemplate>
                                <b><asp:Label runat="server" ID="RCurCode"></asp:Label></b>
                            </FooterTemplate>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>

                     </Columns>
       
<EmptyDataRowStyle ForeColor="Red"></EmptyDataRowStyle>
                     <FooterStyle BackColor="White" ForeColor="#000066" />
                     <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Zawgyi-One" Font-Size="13px" ForeColor="White" />
                     <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                     <RowStyle ForeColor="#000066" />
                     <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                     <SortedAscendingCellStyle BackColor="#F1F1F1" />
                     <SortedAscendingHeaderStyle BackColor="#007DBB" />
                     <SortedDescendingCellStyle BackColor="#CAC9C9" />
                     <SortedDescendingHeaderStyle BackColor="#00547E" />
       
                </asp:GridView>                 
    
</div>
        
    <div id="form" style="display:none">
        <table style="width:400px;font-family:Segoe UI,Verdana,Arial,Sans-Serif;border:1px solid #E6EEF7;">
            <tr >
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Code No
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblCode" Text="" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Main Category
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblMainCat" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                   Category
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblCat" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Purchaser
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblPurchaser" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Purchase Date
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblPurchaseDate" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Owner
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblUserName" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Location
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblDepartment" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Amount
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblAmount" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Return Amount
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblRAmount" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    Remark
                </td>
                <td style="padding:5px 10px 5px 10px;border-bottom:1px solid #E6EEF7;">
                    : <asp:Label runat="server" ID="lblRemark" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function showModal() {
            $.Dialog({
                overlay: true,
                shadow: true,
                //flat: true,
                icon: '<i class="icon-info-2"></i>',
                title: 'Stock Detail',
                width: 400,
                padding: 10,
                content: '',
                onShow: function (_dialog) {
                    var content = _dialog.children('.content');
                    content.html($("#form").html());
                }
            });
        }
        
    </script>   
	

</asp:Content>
