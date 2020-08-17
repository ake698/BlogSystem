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
    public class ArticleManager : IArticleManager
    {

        private readonly IBlogCategoryService blogCategoryService;
        private readonly IArticleService articleService;
        private readonly IArticleToCategoryService articleToCategoryService;

        public ArticleManager(IBlogCategoryService blogCategoryService,
            IArticleService articleService,
            IArticleToCategoryService articleToCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
            this.articleService = articleService;
            this.articleToCategoryService = articleToCategoryService;
        }

        //添加文章
        public async Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId)
        {
            var article = new Article()
            {
                Title = title,
                Content = content,
                UserId = userId
            };
            await articleService.CreateAsync(article);
            Guid articleId = article.Id;
            //添加至目录
            foreach (var categoryId in categoryIds)
            {
                await articleToCategoryService.CreateAsync(new ArticleToCategory()
                {
                   ArticleId = articleId,
                   BlogCategoryId = categoryId,
                }, false);
            }
            await articleToCategoryService.Save();

            
        }

        public async Task CreateCategory(string name, Guid userId)
        {
            await blogCategoryService.CreateAsync(new BlogCategory()
            {
                CategoryName = name,
                UserId = userId
            });
        }

        public async Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds)
        {
            throw new NotImplementedException();
        }

        public async Task EditCategory(Guid categoryId, string newCategoryName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsArticle(Guid id)
        {
            return await articleService.GetAllAsync().AnyAsync(m => m.Id == id);
        }

        public async Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ArticleDto>> GetAllArticlesByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize)
        {
            var list = await articleService.GetAllByPageOrderAsync(pageSize, pageIndex, false)
                .Include(m => m.User)
                .Where(m => m.UserId == userId)
                .Select(m => new ArticleDto()
                {
                    Title = m.Title,
                    BadCount = m.BadCount,
                    GoodCount = m.GoodCount,
                    Email = m.User.Email,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    Id = m.Id,
                    UserId = m.User.Id,
                    ImagePath = m.User.ImagePath
                })
                .ToListAsync();

            foreach (var item in list)
            {
                var cates =await articleToCategoryService.GetAllAsync()
                    .Include(m => m.BlogCategory)
                    .Where(m => m.ArticleId == item.Id).ToListAsync();
                item.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
                item.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();
            }
            return list; 
        }

        public async Task<List<BlogCategoryDto>> GetAllCategories(Guid userId)
        {
            return await blogCategoryService.GetAllAsync()
                .Where(m => m.UserId == userId)
                .Select(
                m => new BlogCategoryDto()
                {
                    Id = m.Id,
                    CategoryName = m.CategoryName
                }).ToListAsync();
        }

        public async Task<int> GetDateCount(Guid userId)
        {
            return await articleService.GetAllAsync().CountAsync(m => m.UserId == userId);
        }

        public async Task<ArticleDto> GetOneArticleById(Guid id)
        {
            var data = await articleService.GetAllAsync()
                .Include(m => m.User)
                .Where(m => m.Id == id)
                .Select(m => new ArticleDto()
                {
                    Id = m.Id,
                    Title = m.Title,
                    BadCount = m.BadCount,
                    GoodCount = m.GoodCount,
                    Email = m.User.Email,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    UserId = m.User.Id,
                    ImagePath = m.User.ImagePath
                }).FirstAsync();

            //获取分类信息
            var cates = await articleToCategoryService.GetAllAsync()
                .Include(m => m.BlogCategory)
                .Where(m => m.ArticleId == data.Id).ToListAsync();
            data.CategoryIds = cates.Select(m => m.BlogCategoryId).ToArray();
            data.CategoryNames = cates.Select(m => m.BlogCategory.CategoryName).ToArray();

            return data;
        }

        public async Task RemoveArticle(Guid articltId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveCategory(string name)
        {
            throw new NotImplementedException();
        }
    }
}
