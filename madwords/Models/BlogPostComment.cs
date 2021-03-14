using System;
using System.ComponentModel.DataAnnotations;

namespace madwords.Models
{
    public class BlogPostComment
    {
        [Key]
        public int CommentID { get; set; }
        public String CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public AppUser Commenter { get; set; }
        public bool Reported { get; set; }
    }
}
