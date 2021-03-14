using madwords.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace madwords.Repos
{
    public class MadwordRepository : IMadwordRepository
    {
        readonly private MadwordContext context;

        // constructor
        public MadwordRepository(MadwordContext c)
        {
            context = c;
        }

        public IQueryable<Madword> Madwords
        {
            get
            {
                // Get all the Madword objects in the Madwords DbSet
                // and include the Author, Comment and Rating object in each Madword.
                return context.Madwords.Include(madword => madword.Author)
                    .Include(madword => madword.Comments)
                    .ThenInclude(comment => comment.Commenter)
                    .Include(rating => rating.Ratings)
                    .ThenInclude(rating => rating.Rater);
            }
        }

        public IQueryable<MadwordTemplate> MadwordTemplates
        {
            get
            {
                return context.MadwordTemplates.Include(madword => madword.Author);
            }
        }

        public void AddMadwordTemplate(MadwordTemplate template)
        {
            context.MadwordTemplates.Add(template);
            context.SaveChanges();
        }

        public void AddMadword(Madword madword)
        {
            context.Madwords.Add(madword);
            context.SaveChanges();
        }

        public void DeleteMadword(Madword madword)
        {
            // First delete the comments
            var comments = madword.Comments.ToList();
            foreach (Comment c in comments)
            {
                DeleteComment(c);
            }
            context.Madwords.Attach(madword);
            context.Madwords.Remove(madword);
            context.SaveChanges();
        }

        public void UpdateMadword(Madword madword)
        {
            context.Madwords.Update(madword); // find the story by the MadwordID and update it.
            context.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            context.Comments.Attach(comment);
            context.Comments.Remove(comment);
            context.SaveChanges();
        }

        public void DeleteUsersComments(string id)
        {
            var comments = context.Comments.Where(c => c.Commenter.Id == id).ToList();
            foreach (Comment c in comments)
            {
                context.Comments.Attach(c);
                context.Comments.Remove(c);
                context.SaveChanges();
            }
        }
        public void DeleteUsersRatings(string id)
        {
            var ratings = context.Ratings.Where(c => c.Rater.Id == id).ToList();
            foreach (Rating r in ratings)
            {
                context.Ratings.Attach(r);
                context.Ratings.Remove(r);
                context.SaveChanges();
            }
        }

        public void DeleteUsersMadwords(string id)
        {
            // Delete the madword
            var madwords = context.Madwords.Where(c => c.Author.Id == id).ToList();
            foreach (Madword m in madwords)
            {
                // First delete the comments
                var comments = m.Comments.ToList();
                foreach(Comment c in comments)
                {
                    DeleteComment(c);
                }
                context.Madwords.Attach(m);
                context.Madwords.Remove(m);
                context.SaveChanges();
            }
        }
    }
}
