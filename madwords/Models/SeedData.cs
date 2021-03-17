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
                    Name = "SiteAdmin"
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

                MadwordTemplate template = new MadwordTemplate
                {
                    MadwordTemplateTitle = "Smash Mouth",
                    MadwordTemplateText = "Somebody once told me the [Noun] is gonna [Verb] me, I ain't the [Adjective] [Noun] in the [Place] , She was looking kind of [Adjective] with her [Noun] and her [Noun] in the shape of a [Letter] on her [Noun]",
                    MadwordTemplateDate = DateTime.Now,
                };
                context.MadwordTemplates.Add(template);

                template = new MadwordTemplate
                {
                    MadwordTemplateTitle = "Rudolph",
                    MadwordTemplateText = "[Name] the [Adjective] -nosed [Noun] had a very [Adjective] nose, And if you ever [Verb] it, You would even say it glows, All of the other [Noun] used to [Verb] and call him [Noun] , They never let [Adjective] [Name] [Verb] in any [Noun] games.",
                    MadwordTemplateDate = DateTime.Now,
                };
                context.MadwordTemplates.Add(template);

                template = new MadwordTemplate
                {
                    MadwordTemplateTitle = "Wild Animal",
                    MadwordTemplateText = "The majestic [Animal] has roamed the forests of [Country] for thousands of years. Today, it wanders in search of [Noun] . It must find [Noun] to survive the harsh [Season]",
                    MadwordTemplateDate = DateTime.Now,
                };
                context.MadwordTemplates.Add(template);

                template = new MadwordTemplate
                {
                    MadwordTemplateTitle = "Sk8terboi",
                    MadwordTemplateText = "He was a [Noun] , She was a [Noun] . Can I make it any more obvious? He was a [Noun] , She did [Sport] . What more can I say? He wanted [Noun] , She'd never tell. Secretly she [Verb_(past_tense)] [Noun] as well. But all of her [Noun] , Stuck up their [Noun] , They had a problem with his [Adjective] [Noun_(plural)]",
                    MadwordTemplateDate = DateTime.Now,
                };
                context.MadwordTemplates.Add(template);

                template = new MadwordTemplate
                {
                    MadwordTemplateTitle = "Blink182",
                    MadwordTemplateText = "All the [Adjective] [Noun_(plural)] , true care truth brings. I'll [Verb] one [Noun] . Your [Noun] , [Adjective] [Noun] . Always I know, you'll be at my [Noun] . [Verb_(ending_w/_-ing)] , [Verb_(ending_w/_-ing)] , [Verb_(ending_w/_-ing)]",
                    MadwordTemplateDate = DateTime.Now,
                };
                context.MadwordTemplates.Add(template);

                template = new MadwordTemplate
                {
                    MadwordTemplateTitle = "Third Eye Blind - Jumper",
                    MadwordTemplateText = "I wish you would [Verb] back from that [Noun] my friend. You could [Verb] with all the [Noun_(plural)] that you've been [Verb_(ending_w/_-ing)] in. And if you do not want to [Verb] me again, I would understand, I would understand.",
                    MadwordTemplateDate = DateTime.Now,
                };
                context.MadwordTemplates.Add(template);

                context.SaveChanges(); // stores all in the DB
            }
        }
    }
}
