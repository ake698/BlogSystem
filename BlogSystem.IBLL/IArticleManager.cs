using BlogSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IArticleManager
    {
        Task CreateArticle(string title, string content, Guid[] categoryIds, Guid userId);
        Task CreateCategory(string name, Guid userId);

        Task<List<BlogCategoryDto>> GetAllCategories(Guid userId);
        Task<List<ArticleDto>> GetAllArticlesByUserId(Guid userId, int pageIndex, int pageSize);
        Task<List<ArticleDto>> GetAllArticlesByEmail(string email);
        Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId);

        Task<int> GetDateCount(Guid userId);

        Task<ArticleDto> GetOneArticleById(Guid id);
        Task<bool> ExistsArticle(Guid id);


        Task RemoveCategory(string name);
        Task EditCategory(Guid categoryId, string newCategoryName);
        Task RemoveArticle(Guid articltId);
        Task EditArticle(Guid articleId, string title, string content, Guid[] categoryIds);

    }
}
