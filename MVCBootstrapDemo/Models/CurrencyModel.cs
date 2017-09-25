using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class CurrencyModel:Currency
    {
        public CurrencyModel()
        {
            IsDelete = true;
        }
        public int No { get; set; }

        public int PageNumber { get; set; }
        public List<ListItem> dlPager { get; set; }

        public bool IsDelete { get; set; }

        public List<CurrencyModel> currencies { get; set; }

    }
    
}