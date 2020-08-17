using BlogSystem.BLL;
using BlogSystem.IBLL;
using BlogSystem.MVC.Filters;
using BlogSystem.MVC.Models.ArticleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVC.Controllers
{
    [BlogSystemAuth]
    public class ArticleController : Controller
    {
        private readonly IArticleManager articleManager;

        public ArticleController(IArticleManager articleManager)
        {
            this.articleManager = articleManager;
        }

        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await articleManager.CreateCategory(model.CategoryName, Guid.Parse(Session["userId"].ToString()));
                return RedirectToAction("CategoryList");
            }
            ModelState.AddModelError("11", "你输入有误");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CategoryList()
        {
            var userId = Guid.Parse(Session["userId"].ToString());
            var list = await articleManager.GetAllCategories(userId);
            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> CreateArticle()
        {
            var userId = Guid.Parse(Session["userId"].ToString());
            ViewBag.CategoryIds = await articleManager.GetAllCategories(userId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateArticle(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(Session["userId"].ToString());
                await articleManager.CreateArticle(model.Title, model.Content, model.CategoryIds, userId);
                return RedirectToAction("ArticleList");
            }
            ModelState.AddModelError("error", "添加失败！");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ArticleList(int pageIndex = 0, int pageSize = 1)
        {
            var userId = Guid.Parse(Session["userId"].ToString());
            var result = await articleManager.GetAllArticlesByUserId(userId, pageIndex, pageSize);
            var datacount = await articleManager.GetDateCount(userId);
            ViewBag.PageCount = datacount % pageSize == 0 ? datacount / pageSize : datacount / pageSize + 1;
            ViewBag.PageIndex = pageIndex;
            return View(result);
        }

        public async Task<ActionResult> ArticleDetails(Guid? id)
        {
            if (id == null || !await articleManager.ExistsArticle(id.Value))
            {
                return RedirectToAction(nameof(ArticleList));
            }
            var result = await articleManager.GetOneArticleById(id.Value);
            return View(result);
        }
    }
}