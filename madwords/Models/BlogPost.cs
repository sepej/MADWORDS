using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace madwords.Models
{
    public class BlogPost
    {
        readonly private List<BlogPostComment> comments = new List<BlogPostComment>();
        //this BlogPostID will become the primary key
        // Add ID to the key. EF knows the convention and makes it primary key
        public int BlogPostID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string BlogPostTitle { get; set; }

        // Has-a relationship
        // when EF creates the table, it will automatically create foreign key because of this
        public AppUser Author { get; set; }
        [Required]
        public string BlogPostText { get; set; }
        public DateTime BlogPostDate { get; set; }
        public List<BlogPostComment> BlogPostComments
        {
            get { return comments; }
        }
    }
}
