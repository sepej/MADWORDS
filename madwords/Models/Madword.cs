using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace madwords.Models
{
    public class Madword
    {
        readonly private List<Comment> comments = new List<Comment>();

        readonly private List<Rating> ratings = new List<Rating>();

        //this MadwordID will become the primary key
        // Add ID to the key. EF knows the convention and makes it primary key
        public int MadwordID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string MadwordTitle { get; set; }

        // Has-a relationship
        // when EF creates the table, it will automatically create foreign key because of this
        public AppUser Author { get; set; }
        [Required]
        public string MadwordText { get; set; }
        public DateTime MadwordDate { get; set; }
        public List<Comment> Comments
        {
            get { return comments; }
        }
        public List<Rating> Ratings
        {
            get { return ratings; }
        }
        public bool Reported { get; set; }
    }
}
