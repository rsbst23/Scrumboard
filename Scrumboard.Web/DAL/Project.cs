using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class Project : BaseObject
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Feature> Features { get; set; }
    }
}