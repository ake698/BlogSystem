using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class UserManager : IUserManager
    {

        //IUserService userService = new UserService(); 
        private readonly IUserService userService;
        public UserManager(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task ChangePassword(string email, string oldPwd, string newPwd)
        {
            var result = await userService.GetAllAsync()
                .AnyAsync(m => m.Email == email && m.Password == oldPwd);

            if (result)
            {
                var user = await userService.GetAllAsync().FirstAsync(m => m.Email == email);
                user.Password = newPwd;
                await userService.EditAsync(user);
            }
        }

        public async Task ChangeUserInformation(string email, string siteName, string imagePath)
        {
            var user = await userService.GetAllAsync().FirstAsync(m => m.Email == email);
            user.SiteName = siteName;
            user.ImagePath = imagePath;
            await userService.EditAsync(user);
        }

        public async Task<UserInforMationDto> GetUserByEmail(string email)
        {
            var result = await userService.GetAllAsync()
                .AnyAsync(m => m.Email == email && m.Email == email);

            if (result)
            {
                return await userService.GetAllAsync()
                    .Where(m => m.Email == email)
                    .Select(m => new UserInforMationDto()
                    {
                        Id = m.Id,
                        Email = m.Email,
                        FansCount = m.FansCount,
                        ImagePath = m.ImagePath,
                        SiteName = m.SiteName,
                        FocusCount = m.FocusCount
                    }).FirstAsync();
            }
            else
            {
                throw new ArgumentException("Email is not exist!!");
            }
        }

        public bool Login(string email, string password, out Guid userId)
        {
            var user = userService.GetAllAsync()
                .FirstOrDefaultAsync(m => m.Email == email && m.Password == password);
            user.Wait();
            var data = user.Result;
            if (data == null)
            {
                userId = new Guid();
                return false;
            }
            else
            {
                userId = data.Id;
                return true;
            }
        }

        public async Task Register(string email, string password)
        {
            await userService.CreateAsync(new User() 
            { 
                Email = email,
                Password = password,
                SiteName = "defaul site!",
                ImagePath = "default.png"
            });
        }
    }
}
