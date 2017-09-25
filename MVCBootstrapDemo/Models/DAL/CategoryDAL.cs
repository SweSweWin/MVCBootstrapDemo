using MVCBootstrapDemo.Models.DL;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MVCBootstrapDemo.Models.DAL
{
    public class CategoryDAL:ICategory
    {

        #region Declaration
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        #region Contstructor
        public CategoryDAL()
        {

        }
        #endregion

        #region SelectAllData
        public List<CategoryModel> SelectAllData(int startIndex, int PageSize, out int TotalCount)
        {
            List<CategoryModel> lst = new List<CategoryModel>();
            TotalCount = 0;
            try
            {
                IEnumerable<CategoryModel> query = (from c in db.Categories
                                                    join mc in db.MainCategories on c.MainCategory equals mc.ID
                                                    orderby (c.SortOrder == null ? 0 : c.SortOrder)
                                                    select new CategoryModel
                                                    {
                                                        ID = c.ID,
                                                        MainCategory = c.MainCategory.Value,
                                                        Name = c.Name,
                                                        MName = mc.Name,
                                                        SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value)
                                                    });
                TotalCount = query.Count();
                query = query.AsEnumerable().Select((c, index) => new CategoryModel()
                {
                    ID = c.ID,
                    MainCategory = c.MainCategory.Value,
                    Name = c.Name,
                    MName = c.Name,
                    SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value),
                    No = ++index
                });
                query = query.Skip(PageSize * (startIndex - 1)).Take(PageSize).ToList();
                return query.ToList();
            }
            catch
            {
            }
            return lst;
        }

        #endregion

        #region ASP Test
        public List<CategoryModel> SelectDataByMainCategoryID(int id)
        {

            List<CategoryModel> lst = new List<CategoryModel>();
            try
            {

                IEnumerable<CategoryModel> query = (from c in db.Categories
                       join mc in db.MainCategories on c.MainCategory equals mc.ID
                       where (c.MainCategory == id)
                       orderby c.SortOrder
                       select new CategoryModel
                       {
                           ID = c.ID,
                           MainCategory = c.MainCategory.Value,
                           Name = c.Name,
                           MName = mc.Name,
                           SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value)

                       });
                query = query.AsEnumerable().Select((c, index) => new CategoryModel()
                {
                    ID = c.ID,
                    MainCategory = c.MainCategory.Value,
                    Name = c.Name,
                    MName = c.Name,
                    SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value),
                    No = ++index
                });
                lst = query.ToList();
                return lst;

            }
            catch { }
            return lst;

        }
        #endregion


        #region SelectAllData For Data Bind Grid
        public List<CategoryModel> SelectAllDataForBindGrid(int startIndex, int PageSize, out int TotalCount)
        {
            List<CategoryModel> lst = new List<CategoryModel>();
            TotalCount = 0;
            try
            {
                IEnumerable<CategoryModel> query = (from c in db.Categories
                                                     join mc in db.MainCategories on c.MainCategory equals mc.ID
                                                     orderby (c.SortOrder == null ? 0 : c.SortOrder)
                                                     select new CategoryModel
                                                     {
                                                         ID = c.ID,
                                                         MainCategory = c.MainCategory.Value,
                                                         Name = c.Name,
                                                         MName = mc.Name,                                                       
                                                         SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value),

                                                     });

                TotalCount = query.Count();

                query = query.AsEnumerable().Select((c, index) => new CategoryModel()
                {
                    ID = c.ID,
                    MainCategory = c.MainCategory.Value,
                    Name = c.Name,
                    MName = c.MName,                 
                    SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value),
                    No = ++index
                });

                query = query.Skip(PageSize * (startIndex - 1)).Take(PageSize).ToList();
                lst = query.ToList();

                return lst;
            }
            catch { }
            return lst;
        }
        #endregion


        #region SelectDataByID
        public CategoryModel SelectDataByID(int id)
        {
            CategoryModel cModel = new CategoryModel();
            try
            {
                cModel = (from c in db.Categories
                                        where c.ID == id
                                        select new CategoryModel
                                        {
                                            ID = c.ID,
                                            MainCategory = c.MainCategory.Value,                                           
                                            Name = c.Name,                                          
                                            SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value)

                                        }).First();
            }
            catch
            {
            }
            return cModel;
        }
        #endregion
        
        #region SelectDetailDataByID
        public CategoryModel SelectDetailDataByID(int id)
        {
            CategoryModel cModel = new CategoryModel();
            try
            {
                cModel = (from c in db.Categories
                          join mc in db.MainCategories on c.MainCategory equals mc.ID
                          where c.ID == id
                          orderby (c.SortOrder == null ? 0 : c.SortOrder)
                          select new CategoryModel
                          {
                              ID = c.ID,
                              MainCategory = c.MainCategory.Value,
                              Name = c.Name,
                              MName = mc.Name,
                              SortOrder = (c.SortOrder == null ? 0 : c.SortOrder.Value)
                          }).First();
              
            }
            catch
            {
            }
            return cModel;
        }
        #endregion


        #region CheckDelete       
        public bool CheckDelete(int id)
        {
            bool check = true;
            try
            {
                var qry = (from inv in db.Inventories where inv.Category == id select inv.Category).ToList();
                if (qry.Count > 0)
                    check = false;
            }
            catch { }
            return check;
        }       
        #endregion
    }
}