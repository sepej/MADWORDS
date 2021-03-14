using madwords.Models;
using madwords.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace madwords.Controllers
{
    public class MadwordController : Controller
    {
        readonly IMadwordRepository repo;
        readonly UserManager<AppUser> userManager;

        public MadwordController(IMadwordRepository r, UserManager<AppUser> u)
        {
            repo = r;
            userManager = u;
        }

        public IActionResult Index()
        {
            //get all madwords in the database
            //List<Madword> madwords = repo.Madwords.ToList<Madword>(); // Use ToList to convert the IQueryable to a list
            var madwords = (from r in repo.Madwords orderby r.MadwordDate descending select r).ToList();
            return View(madwords);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Madword model)
        {
            if (ModelState.IsValid)
            {
                model.Author = userManager.GetUserAsync(User).Result;
                // TODO: get the user's real name in registration
                model.Author.Name = model.Author.UserName;
                model.MadwordDate = DateTime.Now;
                // Store the model in the database
                repo.AddMadword(model);
                return Redirect("Madword");
            }
            return View();
        }

        public IActionResult Madword(int id)
        {
            var madwordExists = (from r in repo.Madwords where r.MadwordID.Equals(id) select r).Count();
            if (madwordExists == 0)
            {
                return Redirect("/Madword");
            }
            else
            {
                return base.View((from r in repo.Madwords
                                  where r.MadwordID.Equals(id)
                                  select r).FirstOrDefault());
            }
        }

        public IActionResult Madwords()
        {
            //get all madwords in the database
            List<Madword> madwords = repo.Madwords.ToList<Madword>(); // Use ToList to convert the IQueryable to a list

            return View(madwords);
        }

        [HttpPost]
        public IActionResult Index(string madwordSearch)
        {
            var madwordsList = (from r in repo.Madwords where r.MadwordText.Contains(madwordSearch) || r.MadwordTitle.Contains(madwordSearch) || r.Author.Name.Contains(madwordSearch) orderby r.MadwordDate descending select r).ToList();
            return View(madwordsList);
        }

        public IActionResult TopRated()
        {
            var madwordsList = (from r in repo.Madwords orderby r.Ratings.Count descending select r).ToList();
            return View(madwordsList);
        }

        [HttpPost]
        public IActionResult TopRated(string madwordSearch)
        {
            var madwordsList = (from r in repo.Madwords where r.MadwordText.Contains(madwordSearch) || r.MadwordTitle.Contains(madwordSearch) || r.Author.Name.Contains(madwordSearch) orderby r.Ratings.Count descending select r).ToList();
            return View(madwordsList);
        }

        // Open the form for entering a comment
        [Authorize]
        public IActionResult Comment(int madwordId)
        {
            var commentVM = new CommentVM { MadwordID = madwordId };
            return View(commentVM);
        }

        [Authorize]
        [HttpPost]
        public RedirectToActionResult Comment(CommentVM commentVM)
        {
            // Comment is the domain model
            var comment = new Comment { CommentText = commentVM.CommentText };
            comment.Commenter = userManager.GetUserAsync(User).Result;
            comment.CommentDate = DateTime.Now;

            // Retrieve the madword that this comment is for
            var madword = (from r in repo.Madwords
                           where r.MadwordID == commentVM.MadwordID
                           select r).First<Madword>();

            // Store the madword with the comment in the database
            madword.Comments.Add(comment);
            repo.UpdateMadword(madword);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int madwordId)
        {
            var madword = (from r in repo.Madwords
                           where r.MadwordID == madwordId
                           select r).First<Madword>();
            repo.DeleteMadword(madword);
            return RedirectToAction("Index");
        }

        public IActionResult ReportMadword(int madwordId)
        {
            var madword = (from r in repo.Madwords
                           where r.MadwordID == madwordId
                           select r).First<Madword>();
            madword.Reported = true;
            repo.UpdateMadword(madword);
            return Redirect("/Madword");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RemoveReport(int madwordId)
        {
            var madword = (from r in repo.Madwords
                           where r.MadwordID == madwordId
                           select r).First<Madword>();
            madword.Reported = false;
            repo.UpdateMadword(madword);
            return Redirect("/Madword");
        }

        [Authorize]
        public IActionResult Vote(int madwordId)
        {

            // Comment is the domain model
            var rating = new Rating { RatingScore = 1 };
            rating.Rater = userManager.GetUserAsync(User).Result;

            // Retrieve the madword that this rating is for
            var madword = (from r in repo.Madwords
                           where r.MadwordID == madwordId
                           select r).First<Madword>();

            var raterExists = madword.Ratings.Where(x => x.Rater == rating.Rater).ToList();

            // Store the madword with the rating in the database
            if (raterExists.Count <= 0)
            {
                madword.Ratings.Add(rating);
                repo.UpdateMadword(madword);
            }
            else
            {
                TempData["message"] = "You already voted for this MADWORD";
            }
            return Redirect("/Madword");
        }
    }
}
