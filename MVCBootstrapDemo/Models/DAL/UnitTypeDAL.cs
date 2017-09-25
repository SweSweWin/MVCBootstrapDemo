using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class UnitTypeDAL
    {

        #region Declare
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        public List<UnitTypeModel> BindData(int currentpage, int pageSize, out int totalRowCount)
        {
            List<UnitTypeModel> lst = new List<UnitTypeModel>();
            totalRowCount = 0;
            try
            {
                IEnumerable<UnitTypeModel> qry = (from u in db.UnitTypes
                                                  orderby (u.SortOrder==null?0:u.SortOrder)
                                                  select new UnitTypeModel
                                                  {
                                                      ID=u.ID,
                                                      Type=u.Type.Value,
                                                      UTypeName = u.Type.Value == 1 ? "Size" : "Model",
                                                      Name = u.Name,
                                                      SortOrder=(u.SortOrder==null?0:u.SortOrder.Value)
                                                  });
                totalRowCount = qry.Count();
                qry = qry.AsEnumerable().Select((u, index) => new UnitTypeModel
                {
                    ID=u.ID,
                    Type=u.Type,
                    UTypeName=u.UTypeName,
                    Name=u.Name,
                    SortOrder=u.SortOrder,
                    No=++index
                });
                qry = qry.Skip(pageSize * (currentpage - 1)).Take(pageSize).ToList();
                lst = qry.ToList();
                return lst;
            }
            catch
            { }
            return lst;
        }

        public bool CheckDelete(int iD)
        {
            bool check = true;
            var qry = (from i in db.Inventories where i.TypeID == iD select i).ToList();
            if(qry.Count>0)
            {
                check = false;
            }
            return check;
        }

        public UnitTypeModel FindbyID(int? id)
        {
            UnitTypeModel model = new UnitTypeModel();
            try
            {
                model = (from u in db.UnitTypes
                         where u.ID == id
                         select new UnitTypeModel
                         {
                            ID=u.ID,
                            Name=u.Name,
                            Type=u.Type.Value,
                            SortOrder=u.SortOrder.Value,
                            UTypeName=u.Type.Value==1?"Size" : "Model"


                         }).First();
            }
            catch
            { }
            return model;
        }

        #region Test ASP
        public List<UnitTypeModel> BindDataByMainCat(int id)
        {
            List<UnitTypeModel> lst = new List<UnitTypeModel>();
            try
            {
                IEnumerable<UnitTypeModel> qry = (from u in db.UnitTypes
                             join inv in db.Inventories on u.ID equals inv.TypeID
                             join main in db.MainCategories on inv.Maincategory equals main.ID
                             where main.ID == id
                             group u by new { u.ID, u.Name, u.SortOrder } into g
                             orderby g.Key.SortOrder
                             select new UnitTypeModel
                             {
                                 ID = g.Key.ID,
                                 Name = g.Key.Name,
                                 SortOrder = g.Key.SortOrder.Value
                             }).ToList();
                qry = qry.AsEnumerable().Select((u, index) => new UnitTypeModel
                {
                    ID = u.ID,
                    Type = u.Type,
                    UTypeName = u.UTypeName,
                    Name = u.Name,
                    SortOrder = u.SortOrder,
                    No = ++index
                });

                lst = qry.ToList();
                return lst;
            }
            catch { }
            return lst;

        }
        #endregion
    }
}