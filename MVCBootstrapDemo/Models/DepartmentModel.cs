using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class DepartmentModel:Department
    {
        public DepartmentModel()
        {
            IsDelete = true;
        }

        public int No { get; set; }

        //pagination       
        public int PageNumber { get; set; }
        public List<ListItem> dlPager { get; set; }


        //check for delete
        public bool IsDelete { get; set; }

        //bindview
        public List<DepartmentModel> Departments { get; set; }
        
    }
}