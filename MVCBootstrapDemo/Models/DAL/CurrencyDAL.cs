using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models.DAL
{
    public class CurrencyDAL
    {
        #region Declar
        BootstrapMVCEntities db = new BootstrapMVCEntities();
        #endregion

        public List<CurrencyModel> GetBindData(int currentPage, int pageSize, out int totalRowCount)
        {
            totalRowCount = 0;
            List<CurrencyModel> lst = new List<CurrencyModel>();
            try
            {
                IEnumerable<CurrencyModel> qry = (from c in db.Currencies
                                                  select new CurrencyModel
                                                  {
                                                  ID=c.ID,
                                                  Name=c.Name,
                                                  SortOrder=(c.SortOrder==null?0:c.SortOrder.Value)
                                                  });
                totalRowCount = qry.Count();
                qry = qry.AsEnumerable().Select((c, index) =>new CurrencyModel
                  {
                    ID=c.ID,
                    Name=c.Name,
                    SortOrder=c.SortOrder,
                    No=++index
                  });
                qry = qry.Skip(pageSize * (currentPage-1)).Take(pageSize).ToList();
                lst = qry.ToList();

            }
            catch { }
            return lst;
        }

        public bool CheckDelete(int iD)
        {
            bool delete = true;
            try
            {
                var qry = (from inv in db.Inventories
                           where inv.Currency == iD | inv.ReturnCurrency == iD
                           select inv.Currency | inv.ReturnCurrency).ToList();
                if(qry.Count>0)
                {
                    delete = false;
                }               
            }
            catch { }
            return delete;

        }
    }
}