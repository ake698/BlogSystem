using Autofac;
using Autofac.Integration.Mvc;
using BlogSystem.DAL;
using BlogSystem.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVC.App_Start
{
    public static class AutofacContainer
    {
        public static IContainer Instance;
        public static void Init(Func<ContainerBuilder, ContainerBuilder> func = null)
        {
            //新建容器构建器，用于注册组件和服务
            var builder = new ContainerBuilder();
            //自定义依赖注册
            MyBuild(builder);

            //注册控制器
            builder.RegisterControllers(typeof(AutofacContainer).Assembly);

            //利用构建器创建容器
            Instance = builder.Build();

            //实现
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Instance));
        }

        /// <summary>
        /// 自定义注册
        /// </summary>
        /// <param name="builder"></param>
        public static void MyBuild(ContainerBuilder builder)
        {

            //builder.RegisterType<UserService>().As<IUserService>();
            //builder.RegisterType<BlogCategoryService>().As<IBlogCategoryService>();

            //通过反射找到对应的dal
            Assembly dal = Assembly.Load("BlogSystem.DAL");
            //通过容器注册反射得到的类型，并且与接口层进行关联
            builder.RegisterAssemblyTypes(dal).AsImplementedInterfaces();

            Assembly bll = Assembly.Load("BlogSystem.BLL");
            builder.RegisterAssemblyTypes(bll).AsImplementedInterfaces();

        }
    }
}