using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class PositionDAL
    {
        #region Declar
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        public List<PositionModel> BindData(int currentPage, int pageSize, out int totalRowCount)
        {
            List<PositionModel> lst = new List<PositionModel>();
            totalRowCount = 0;
            try
            {
                IEnumerable<PositionModel> qry = (from p in db.Positions
                                                  select new PositionModel
                                                  {
                                                      Pos_ID=p.Pos_ID,
                                                      Pos_Name=p.Pos_Name,
                                                      Pos_SortOrder=(p.Pos_SortOrder==null?0:p.Pos_SortOrder.Value)
                                                  });
                totalRowCount = qry.Count();
                qry = qry.AsEnumerable().Select((p, index) => new PositionModel
                {
                    Pos_ID = p.Pos_ID,
                    Pos_Name = p.Pos_Name,
                    Pos_SortOrder = p.Pos_SortOrder,
                    No = ++index
                });
                qry = qry.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                lst = qry.ToList();
                return lst;

            }
            catch { }
            return lst;
        }

        public bool CheckDelete(int pos_ID)
        {
            bool check = true;
            try
            {
                var qry = (from e in db.Employees
                           where e.E_Position == pos_ID
                           select e.E_Position).ToList();
                if(qry.Count>0)
                {
                    check = false;
                }

            }
            catch { }
            return check;
        }
    }
}