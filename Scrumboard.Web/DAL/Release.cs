using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class Release : BaseProjectObject
    {
        [Required]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }
    }
}