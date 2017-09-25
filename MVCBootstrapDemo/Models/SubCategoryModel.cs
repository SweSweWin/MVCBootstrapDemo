using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class SubCategoryModel:SubCategory
    {
        public SubCategoryModel()
        {
            MName = "";
            CName = "";          
        }
        public string MName { get; set; }
        public string CName { get; set; }
        public int No { get; set; }

        //pagination       
        public int PageNumber { get; set; }
        public List<ListItem> dlPager { get; set; }
        

        //bind view
        public List<SubCategoryModel> SubCategories { get; set; }
        public IEnumerable<SelectListItem> Maincategories { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

    }
}