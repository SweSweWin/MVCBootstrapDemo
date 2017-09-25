using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class SubCategoryDAL
    {
        #region Declaration
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        public List<SubCategoryModel> SelectAllDataForBindGrid(int currentPage, int pageSize, out int totalRowCount)
        {
            List<SubCategoryModel> lst = new List<SubCategoryModel>();
            totalRowCount = 0;
            try
            {
                IEnumerable<SubCategoryModel> query = (from sc in db.SubCategories
                                                       join c in db.Categories on sc.Category equals c.ID
                                                       join mc in db.MainCategories on c.MainCategory equals mc.ID
                                                       orderby (sc.SortOrder == null ? 0 : sc.SortOrder)
                                                       select new SubCategoryModel{
                                                           ID=sc.ID,
                                                           Name=sc.Name,
                                                           SortOrder=(sc.SortOrder==null?0:sc.SortOrder.Value),
                                                           CName=c.Name,
                                                           MName=mc.Name,
                                                       });
                totalRowCount = query.Count();
                query = query.AsEnumerable().Select((sc, index) => new SubCategoryModel
                {
                    ID = sc.ID,
                    Name = sc.Name,
                    SortOrder = sc.SortOrder.Value,
                    CName = sc.CName,
                    MName = sc.MName,
                    No = ++index

                });
                query = query.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                lst = query.ToList();
                return lst;
            }
            catch
            {

            }
            return lst;
        }

        
    }
}