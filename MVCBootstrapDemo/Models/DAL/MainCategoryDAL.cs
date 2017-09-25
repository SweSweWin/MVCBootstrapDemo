using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class MainCategoryDAL
    {
        #region Declaration
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        #region SelectAllData For Data Bind Grid
        
        public List<MainCategoryModel> SelectAllDataForBindGrid(int currentpage, int pageSize, out int totalRowCount)
        {
            List<MainCategoryModel> lst = new List<MainCategoryModel>();
            totalRowCount = 0;
            try
            {
                IEnumerable<MainCategoryModel> query = (from m in db.MainCategories
                                                        orderby (m.SortOrder == null ? 0 : m.SortOrder)
                                                        select new MainCategoryModel
                                                        {
                                                            ID = m.ID,
                                                            Name = m.Name,
                                                            Code = m.Code,
                                                            SortOrder = (m.SortOrder == null ? 0 : m.SortOrder.Value),
                                                        });
                totalRowCount = query.Count();
                query = query.AsEnumerable().Select((m, index) => new MainCategoryModel()
                {
                    ID = m.ID,
                    Name = m.Name,
                    Code = m.Code,
                    SortOrder = (m.SortOrder == null ? 0 : m.SortOrder.Value),
                    No = ++index
                });

                query = query.Skip(pageSize * (currentpage - 1)).Take(pageSize).ToList();
                lst = query.ToList();
                return lst;

            }
            catch
            {

            }
            return lst;

        }
        #endregion

        #region Test ASP
        public List<MainCategoryModel> BindData()
        {
            List<MainCategoryModel> lst = new List<MainCategoryModel>();
            try
            {
                IEnumerable<MainCategoryModel> query=(from mc in db.MainCategories
                             orderby (mc.SortOrder == null ? 0 : mc.SortOrder)
                             select new MainCategoryModel
                             {
                                 ID = mc.ID,
                                 Name = mc.Name,
                                 Code = mc.Code,
                                 SortOrder = (mc.SortOrder == null ? 0 : mc.SortOrder.Value)
                             }).ToList();
                query = query.AsEnumerable().Select((m, index) => new MainCategoryModel()
                {
                    ID = m.ID,
                    Name = m.Name,
                    Code = m.Code,
                    SortOrder = (m.SortOrder == null ? 0 : m.SortOrder.Value),
                    No = ++index
                });
                lst = query.ToList();

                return lst;
            }
            catch { }
            return lst;

        }


        #endregion

        #region CheckDelete
        public bool CheckDelete(int id)
        {
            bool check = true;
            try
            {
                var qry = (from t1 in db.Categories where t1.MainCategory == id select t1.MainCategory).ToList().Union(from t2 in db.Inventories where t2.Maincategory == id select t2.Maincategory).ToList();                
                if (qry.Count > 0)
                    check = false;
            }
            catch { }
            return check;
        }
        #endregion

        
       

    }
}