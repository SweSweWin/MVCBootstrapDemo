using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class UserDAL
    {
        #region Declar
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion
        public List<LoginModel> BindData(int currentPage, int pageSize, out int totalRowCount)
        {
            List<LoginModel> lst = new List<LoginModel>();
            totalRowCount = 0;
            try
            {
                IEnumerable<LoginModel> qry = (from u in db.users
                                               select new LoginModel
                                               {
                                                   U_ID = u.U_ID,
                                                   U_Level=u.U_Level.Value,
                                                   U_Name=u.U_Name
                                               });
                totalRowCount = qry.Count();
                qry = qry.AsEnumerable().Select((u, index) => new LoginModel
                {
                    U_ID = u.U_ID,
                    U_Level = u.U_Level.Value,
                    U_Name = u.U_Name,
                    No=++index
                });
                qry = qry.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                lst = qry.ToList();

                return lst;
            }
            catch
            { }
            return lst;
        }
       
    }
}