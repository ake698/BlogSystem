using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVC.Models.UserViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("账号")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DisplayName("密码")]
        [StringLength(50,MinimumLength =6)]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        [DisplayName("记住我")]
        public bool RememberMe { get; set; }
    }
}