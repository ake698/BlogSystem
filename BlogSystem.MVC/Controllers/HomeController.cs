using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogSystem.MVC.Models;
using System.Web.Mvc;
using BlogSystem.MVC.Models.UserViewModels;
using BlogSystem.IBLL;
using BlogSystem.BLL;
using System.Threading.Tasks;
using BlogSystem.MVC.Filters;

namespace BlogSystem.MVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserManager userManager;
        public HomeController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [BlogSystemAuth]
        public ActionResult Index()
        {
            return View();
        }
        [BlogSystemAuth]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [BlogSystemAuth]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            userManager.GetUserByEmail("test1@q.com");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //IUserManager userManager = new UserManager();
                await userManager.Register(model.Email, model.Password);
                return Content("register success");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //IUserManager userManager = new UserManager();
                Guid userId;
                if(userManager.Login(model.Email, model.LoginPassword, out userId))
                {
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("loginName")
                        {
                            Value = model.Email,
                            Expires = DateTime.Now.AddDays(7)
                        });
                        Response.Cookies.Add(new HttpCookie("userId")
                        {
                            Value = userId.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });

                    }
                    else
                    {
                        Session["loginName"] = model.Email;
                        Session["userId"] = userId.ToString();
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "您的账号密码有误！");
                }
                
                
            }
            return View();
        }
    }
}