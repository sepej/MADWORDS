using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace madwords.Models
{
    public class MadwordTemplate
    {
        //this MadwordID will become the primary key
        // Add ID to the key. EF knows the convention and makes it primary key
        public int MadwordTemplateID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string MadwordTemplateTitle { get; set; }

        // Has-a relationship
        // when EF creates the table, it will automatically create foreign key because of this
        public AppUser Author { get; set; }
        [Required]
        public string MadwordTemplateText { get; set; }
        public DateTime MadwordTemplateDate { get; set; }
    }
}
