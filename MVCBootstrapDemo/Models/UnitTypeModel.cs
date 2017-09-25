using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class UnitTypeModel : UnitType
    {
        public UnitTypeModel()
        {
            IsDelete = true;
            UTypeName = "";

        }
        public int No { get; set; }
        public string UTypeName { get; set; }

        public bool IsDelete { get; set; }

        //pagination
        public List<ListItem> dlpager { get; set; }
        public int pageNumber { get; set; }

        //data bind
        public List<UnitTypeModel> unittypes { get; set; }
    }
}