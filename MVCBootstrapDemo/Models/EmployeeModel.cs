using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class EmployeeModel:Employee
    {
        public EmployeeModel()
        {
            IsDelete = true;
            DName = "";
            PName = "";
            ActiveName = "";
        }
        public string DName { get; set; }
        public string PName { get; set; }
        public string ActiveName { get; set; }
        public int No { get; set; }

        //pagination       
        public int PageNumber { get; set; }
        public List<ListItem> dlPager { get; set; }


        //check for delete
        public bool IsDelete { get; set; }

        //bindview
        public List<EmployeeModel> Employees { get; set; }

        //Search
        public IEnumerable<SelectListItem> ddlDepartmant { get; set; }
        public IEnumerable<SelectListItem> ddlPosition { get; set; }
    }
}