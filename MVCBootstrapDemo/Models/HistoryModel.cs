using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBootstrapDemo.Models
{
    public class HistoryModel:History
    {
        public HistoryModel()
        {
            DName = "";
            InvCodeNo = "";
            EE_FullName = "";
            InvUseDate = DateTime.Now;
            InvRemark = "";

        }

        public string DName { get; set; }
        public string InvCodeNo { get; set; }
        public string EE_FullName { get; set; }
        public DateTime InvUseDate { get; set; }
        public String InvRemark { get; set; }


    }
}