using madwords.Models;
using madwords.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace madwords.Controllers
{
    public class HomeController : Controller
    {
        readonly IBlogPostRepository repo;
        readonly UserManager<AppUser> userManager;

        public HomeController(IBlogPostRepository r, UserManager<AppUser> u)
        {
            repo = r;
            userManager = u;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Blog()
        {
            //get all blogposts in the database
            var blogposts = (from r in repo.BlogPosts orderby r.BlogPostDate descending select r).ToList();
            return View(blogposts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Open the form for entering a comment
        [Authorize]
        public IActionResult Comment(int blogpostId)
        {
            var commentVM = new BlogPostCommentVM { BlogPostID = blogpostId };
            return View(commentVM);
        }

        [Authorize]
        [HttpPost]
        public RedirectToActionResult Comment(BlogPostCommentVM commentVM)
        {
            // Comment is the domain model
            var comment = new BlogPostComment { CommentText = commentVM.CommentText };
            comment.Commenter = userManager.GetUserAsync(User).Result;
            comment.CommentDate = DateTime.Now;

            // Retrieve the madword that this comment is for
            var blogpost = (from r in repo.BlogPosts
                            where r.BlogPostID == commentVM.BlogPostID
                            select r).First<BlogPost>();

            // Store the madword with the comment in the database
            blogpost.BlogPostComments.Add(comment);
            repo.UpdateBlogPost(blogpost);

            return RedirectToAction("Blog");
        }
    }
}
