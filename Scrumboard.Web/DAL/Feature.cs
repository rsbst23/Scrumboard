using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class Feature : BaseProjectObject
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Release")]
        public int? ReleaseId { get; set; }

        public virtual Release Release { get; set; }

        [DisplayName("Business Value")]
        public int? BusinessValueId { get; set; }

        public BusinessValue BusinessValue { get; set; }

        public ICollection<BacklogItem> BacklogItems { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Planned Start")]
        public DateTime? PlannedStart { get; set; }

        [DisplayName("Planned End")]
        public DateTime? PlannedEnd { get; set; }
    }
}