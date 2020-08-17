using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVC.Models.ArticleViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(30,MinimumLength =2)]
        [DisplayName("类别名")]
        public string CategoryName { get; set; }
    }
}