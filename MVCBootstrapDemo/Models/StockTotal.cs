using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models
{
    public class StockTotal
    {
        public StockTotal()
        {
            //
            // TODO: Add constructor logic here
            //
          
        }

        public int MainID { get; set; }
        public int CurID { get; set; }
        public int Qty { get; set; }
        public string MainCategory { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string AmountCurrency { get; set; }
        
    }
}