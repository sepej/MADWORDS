using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace madwords.Models
{
    public class SeedData
    {
        public static void Seed(MadwordContext context, UserManager<AppUser> userManager,
                                   RoleManager<IdentityRole> roleManager)
        {
            if (!context.Madwords.Any())  // this is to prevent duplicate data from being added
            {
                // TODO: check the results and do something if the operation failed--if it ever does
                _ = roleManager.CreateAsync(new IdentityRole("Member")).Result;
                _ = roleManager.CreateAsync(new IdentityRole("Admin")).Result;

                // Seeding a default administrator. They will need to change their password after logging in.
                AppUser siteadmin = new AppUser
                {
                    UserName = "SiteAdmin",
                    Name = "Site Admin"
                };
                userManager.CreateAsync(siteadmin, "Secret-123").Wait();
                IdentityRole adminRole = roleManager.FindByNameAsync("Admin").Result;
                userManager.AddToRoleAsync(siteadmin, adminRole.Name);

                // Seed users and reviews for manual site testing

                AppUser josephSepe = new AppUser
                {
                    UserName = "josephsepe",
                    Name = "Joseph Sepe"
                };
                context.Users.Add(josephSepe);
                context.SaveChanges();

                Madword madword = new Madword
                {
                    MadwordTitle = "Test Title",
                    MadwordDate = DateTime.Parse("2/1/2021"),
                    MadwordText = "Test Text",
                    Author = josephSepe,
                };
                context.Madwords.Add(madword);
                context.SaveChanges(); // stores all the reviews in the DB

                BlogPost post = new BlogPost
                {
                    BlogPostTitle = "Test Title",
                    BlogPostDate = DateTime.Parse("2/1/2021"),
                    BlogPostText = "Test Text",
                    Author = josephSepe,
                };
                context.BlogPosts.Add(post);

                context.SaveChanges(); // stores all the reviews in the DB
            }
        }
    }
}
