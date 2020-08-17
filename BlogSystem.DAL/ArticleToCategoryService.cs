

using BlogSystem.IDAL;
using BlogSystem.Model;

namespace BlogSystem.DAL
{
    public class ArticleToCategoryService : BaseService<ArticleToCategory> , IArticleToCategoryService
    {
        public ArticleToCategoryService() : base(new BlogContext())
        {
        }
    }
}
