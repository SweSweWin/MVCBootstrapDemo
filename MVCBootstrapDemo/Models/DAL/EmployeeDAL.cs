using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class EmployeeDAL
    {
        #region Declar
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion
        #region BindData     
        public List<EmployeeModel> BindData(int currentpage, int pagesize, out int totalrowcount, int dept, int pos, string e_Username)
        {
            List<EmployeeModel> lst = new List<EmployeeModel>();
            totalrowcount = 0;
            try
            {
                if (string.IsNullOrEmpty(e_Username))
                {
                    IEnumerable<EmployeeModel> qry = (from e in db.Employees
                                                      join d in db.Departments on e.E_Department equals d.ID
                                                      join p in db.Positions on e.E_Position equals p.Pos_ID
                                                      where e.E_Department == (dept == 0 ? e.E_Department : dept)
                                                      && e.E_Position==(pos==0?e.E_Position:pos)
                                                      orderby e.E_FullName
                                                      select new EmployeeModel
                                                      {
                                                          E_ID = e.E_ID,
                                                          E_FullName = e.E_FullName,
                                                          DName = d.Name,
                                                          PName = p.Pos_Name,
                                                          E_Username = e.E_Username,
                                                          E_Active = e.E_Active.Value,
                                                          ActiveName = e.E_Active.Value == true ? "Active" : "InActive"
                                                      });
                    totalrowcount = qry.Count();
                    qry = qry.AsEnumerable().Select((e, index) => new EmployeeModel
                    {
                        E_ID = e.E_ID,
                        E_FullName = e.E_FullName,
                        DName = e.DName,
                        PName = e.PName,
                        E_Username = e.E_Username,
                        E_Active = e.E_Active,
                        ActiveName = e.ActiveName,
                        No = ++index
                    });
                    qry = qry.Skip(pagesize * (currentpage - 1)).Take(pagesize).ToList();
                    lst = qry.ToList();
                }
                else
                {
                    IEnumerable<EmployeeModel> qry = (from e in db.Employees
                                                      join d in db.Departments on e.E_Department equals d.ID
                                                      join p in db.Positions on e.E_Position equals p.Pos_ID
                                                      where e.E_Department == (dept == 0 ? e.E_Department : dept)
                                                       && e.E_Position == (pos == 0 ? e.E_Position : pos)
                                                       && e.E_FullName.StartsWith(e_Username)
                                                      orderby e.E_FullName
                                                      select new EmployeeModel
                                                      {
                                                          E_ID = e.E_ID,
                                                          E_FullName = e.E_FullName,
                                                          DName = d.Name,
                                                          PName = p.Pos_Name,
                                                          E_Username = e.E_Username,
                                                          E_Active = e.E_Active.Value,
                                                          ActiveName = e.E_Active.Value == true ? "Active" : "InActive"
                                                      });
                    totalrowcount = qry.Count();
                    qry = qry.AsEnumerable().Select((e, index) => new EmployeeModel
                    {
                        E_ID = e.E_ID,
                        E_FullName = e.E_FullName,
                        DName = e.DName,
                        PName = e.PName,
                        E_Username = e.E_Username,
                        E_Active = e.E_Active,
                        ActiveName = e.ActiveName,
                        No = ++index
                    });
                    qry = qry.Skip(pagesize * (currentpage - 1)).Take(pagesize).ToList();
                    lst = qry.ToList();
                }
                return lst;
            }
            catch { }
            return lst;
        }
        #endregion
        #region CheckDelete
        public bool CheckDelete(int e_ID)
        {
            bool isdelete = true;
            try
            {
                var qry = (from h in db.Histories where h.To_ID == e_ID select h.To_ID).ToList().Union(from
                        i in db.Inventories where i.EmpID == e_ID select i.EmpID).ToList();
                if(qry.Count>0)
                {
                    isdelete = false;
                }
            }
            catch { }
            return isdelete;
        }
        #endregion
    }
}