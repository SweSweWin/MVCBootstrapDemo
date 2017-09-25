using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MVCBootstrapDemo.Models
{
    public class LoginModel:user
    {
        public LoginModel()
        {
            IsDelete = true;
        }

        public int No { get; set; }

        public int pageNumber { get; set; }

        public bool IsDelete { get; set; }

        public List<LoginModel> users { get; set; }
        public List<ListItem> dlpager { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ReturnURL { get; set; }

        public bool isRemember { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}