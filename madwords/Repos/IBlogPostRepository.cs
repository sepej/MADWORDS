using madwords.Models;
using System.Linq;

namespace madwords.Repos
{
    public interface IBlogPostRepository
    {
        IQueryable<BlogPost> BlogPosts { get; }
        void AddBlogPost(BlogPost post);
        void UpdateBlogPost(BlogPost post);
        void DeleteBlogPost(BlogPost post);
        void DeleteUsersBlogPosts(string id);
        void DeleteUsersBlogPostComments(string id);
        BlogPost GetBlogPostByTitle(string title);
    }
}
