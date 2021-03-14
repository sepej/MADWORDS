using madwords.Models;
using madwords.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madwords.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        readonly private UserManager<AppUser> userManager;
        readonly private RoleManager<IdentityRole> roleManager;
        readonly private IMadwordRepository madwordRepo;
        private readonly IBlogPostRepository blogpostRepo;

        //readonly private IBlogPostRepository blogpostRepo;

        public AdminController(UserManager<AppUser> userMngr,
                               RoleManager<IdentityRole> roleMngr,
                               IMadwordRepository repo,
                               IBlogPostRepository blogpostrepo)
        {
            userManager = userMngr;
            roleManager = roleMngr;
            madwordRepo = repo;
            blogpostRepo = blogpostrepo;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = new List<AppUser>();
            foreach (AppUser user in userManager.Users)
            {
                user.RoleNames = await userManager.GetRolesAsync(user);
                users.Add(user);
            }

            AdminVM model = new AdminVM
            {
                Users = users, // List of AppUsers
                Roles = roleManager.Roles
            };

            return View(model);
        } // the other action methods } 

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityResult result = null;
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Delete everything from the user
                madwordRepo.DeleteUsersComments(user.Id);
                madwordRepo.DeleteUsersRatings(user.Id);
                madwordRepo.DeleteUsersMadwords(user.Id);
                blogpostRepo.DeleteUsersBlogPostComments(user.Id);
                blogpostRepo.DeleteUsersBlogPosts(user.Id);

                result = await userManager.DeleteAsync(user);
            }
            if (!result.Succeeded)
            {
                // if failed 
                string errorMessage = "";
                foreach (IdentityError error in result.Errors)
                {
                    errorMessage += errorMessage != "" ? " | " : "";   // put a separator between messages
                    errorMessage += error.Description;
                }
                TempData["message"] = errorMessage;
            }
            else
            {
                TempData["message"] = "User " + user.Name + " deleted.";  // No errors, clear the message
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToAdmin(string id)
        {
            IdentityRole adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                TempData["message"] = "Admin role does not exist. "
                    + "Click 'Create Admin Role' button to create it.";
            }
            else
            {
                AppUser user = await userManager.FindByIdAsync(id);
                await userManager.AddToRoleAsync(user, adminRole.Name);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromAdmin(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            var result = await userManager.RemoveFromRoleAsync(user, "Admin");
            if (result.Succeeded) { }
            return RedirectToAction("Index");
        }


        /****************  Role management *******************/

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminRole()
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RegisterVM model)
        {

            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username, Name = model.Username };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBlogPost(BlogPost model)
        {
            if (ModelState.IsValid)
            {
                model.Author = userManager.GetUserAsync(User).Result;
                // TODO: get the user's real name in registration
                model.Author.Name = model.Author.UserName;
                model.BlogPostDate = DateTime.Now;
                // Store the model in the database
                blogpostRepo.AddBlogPost(model);
                return Redirect("/Home/Blog");
            }
            return View();
        }

        public IActionResult ReportedMadwords()
        {
            var madword = (from r in madwordRepo.Madwords
                           where r.Reported == true
                           select r).ToList();
            return View(madword);
        }
    }
}