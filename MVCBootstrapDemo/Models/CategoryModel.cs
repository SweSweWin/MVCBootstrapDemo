using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class CategoryModel:Category
    {
        public CategoryModel()
        {         
            MName = "";
            IsDelete = true;
        }
        public string MName { get; set; }
        public int No { get; set; }

        //pagination       
        public int PageNumber { get; set; }
        public List<ListItem> dlPager { get; set; }
        

        //check for delete
        public bool IsDelete { get; set; }
       
        //bind view
        public List<CategoryModel> Categories { get; set; }
        public IEnumerable<SelectListItem> Maincategories { get; set; }   


    }

   
}