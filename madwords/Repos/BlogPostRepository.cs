using madwords.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace madwords.Repos
{
    public class BlogPostRepository : IBlogPostRepository
    {
        readonly private MadwordContext context;

        // constructor
        public BlogPostRepository(MadwordContext c)
        {
            context = c;
        }

        public IQueryable<BlogPost> BlogPosts
        {
            get
            {
                // Get all the BlogPost objects in the BlogPosts DbSet
                // and include the Author object in each BlogPost.
                return context.BlogPosts.Include(post => post.Author)
                    .Include(post => post.BlogPostComments);
            }
        }

        public void AddBlogPost(BlogPost post)
        {
            context.BlogPosts.Add(post);
            context.SaveChanges();
        }

        public BlogPost GetBlogPostByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void UpdateBlogPost(BlogPost post)
        {
            context.BlogPosts.Update(post);
            context.SaveChanges();
        }

        public void DeleteBlogPost(BlogPost post)
        {
            context.BlogPosts.Attach(post);
            context.BlogPosts.Remove(post);
            context.SaveChanges();
        }
        public void DeleteUsersBlogPostComments(string id)
        {
            var comments = context.BlogPostComments.Where(c => c.Commenter.Id == id).ToList();
            foreach (BlogPostComment b in comments)
            {
                context.BlogPostComments.Attach(b);
                context.BlogPostComments.Remove(b);
                context.SaveChanges();
            }
        }

        public void DeleteBlogPostComment(BlogPostComment comment)
        {
            context.BlogPostComments.Attach(comment);
            context.BlogPostComments.Remove(comment);
            context.SaveChanges();
        }

        public void DeleteUsersBlogPosts(string id)
        {
            
            // Delete the posts
            var blogposts = context.BlogPosts.Where(p => p.Author.Id == id).ToList();
            foreach (BlogPost b in blogposts)
            {
                context.BlogPosts.Attach(b);
                context.BlogPosts.Remove(b);
                context.SaveChanges();
            }
        }
    }
}
