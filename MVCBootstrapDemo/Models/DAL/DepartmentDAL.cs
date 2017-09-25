using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class DepartmentDAL
    {
        #region Declare
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        public List<DepartmentModel> SelectAllDataForBindGrid(int CurrentPage, int pageSize, out int TotalRowCount)
        {
            List<DepartmentModel> lst = new List<DepartmentModel>();
            TotalRowCount = 0;

            try
            {
                IEnumerable<DepartmentModel> query = (from d in db.Departments
                                                      orderby (d.SortOrder == null ? 0 : d.SortOrder)
                                                      select new DepartmentModel
                                                      {
                                                          ID = d.ID,
                                                          Name = d.Name,
                                                          SortOrder = (d.SortOrder == null ? 0 : d.SortOrder.Value)
                                                      }
                                                   );
                TotalRowCount = query.Count();
                query = query.AsEnumerable().Select((d, index) => new DepartmentModel
                {
                    ID = d.ID,
                    Name = d.Name,
                    SortOrder = d.SortOrder.Value,
                    No = ++index
                });
                query = query.Skip(pageSize * (CurrentPage - 1)).Take(pageSize).ToList();
                lst = query.ToList();
                return lst;
            }
            catch
            {

            }
            return lst;
        }

        public bool CheckDelete(int iD)
        {
            bool check = true;
            try
            {
                var qry = (from e in db.Employees where e.E_Department == iD select e.E_Department).ToList();
                if(qry.Count>0)
                {
                    check = false;
                }

            }
            catch
            { }
            return check;
            
        }
        #region ASP Test
        public List<DepartmentModel> BindData()
        {
            List<DepartmentModel> lst = new List<DepartmentModel>();
            try
            {
                IEnumerable<DepartmentModel> query = (from d in db.Departments
                             orderby (d.SortOrder == null ? 0 : d.SortOrder)
                             select new DepartmentModel
                             {
                                 ID = d.ID,
                                 Name = d.Name,
                                 SortOrder = (d.SortOrder == null ? 0 : d.SortOrder.Value)
                             }).ToList();
                query = query.AsEnumerable().Select((d, index) => new DepartmentModel
                {
                    ID = d.ID,
                    Name = d.Name,
                    SortOrder = d.SortOrder.Value,
                    No = ++index
                });
                lst = query.ToList();
                return lst;
            }
            catch { }
            return lst;

        }
        #endregion
    }
}