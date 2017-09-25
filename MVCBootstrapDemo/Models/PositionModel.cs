using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class PositionModel:Position
    {
        public PositionModel()
        {
            IsDelete = true;
        }
        public int No { get; set; }

        public bool IsDelete { get; set; }

        public List<ListItem> dlPager { get; set; }
        public int pageNumber { get; set; }

        public List<PositionModel> positions { get; set; }
    }
}