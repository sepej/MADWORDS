using madwords.Models;
using System.Linq;

namespace madwords.Repos
{
    public interface IMadwordRepository
    {
        IQueryable<Madword> Madwords { get; }  // Read (or retrieve) stories
        IQueryable<MadwordTemplate> MadwordTemplates { get; }  // Read (or retrieve) stories
        void AddMadword(Madword madword);  // Create a review
        void UpdateMadword(Madword madword);
        void DeleteMadword(Madword madword);
        void DeleteComment(Comment comment);
        void DeleteUsersComments(string id);
        void DeleteUsersRatings(string id);
        void DeleteUsersMadwords(string id);
        void AddMadwordTemplate(MadwordTemplate template);

    }
}
