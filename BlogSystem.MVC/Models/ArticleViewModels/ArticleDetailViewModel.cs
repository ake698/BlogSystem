using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSystem.MVC.Models.ArticleViewModels
{
    public class ArticleDetailViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime dateTime { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string[] CategoryNames { get; set; }
        public Guid[] CategoryIds { get; set; }
        public int GoodCount { get; set; }
        public int BadCount { get; set; }
    }
}