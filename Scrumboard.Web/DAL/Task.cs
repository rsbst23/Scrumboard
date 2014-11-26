using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class Task : BaseObject
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int? Priority { get; set; }

        [DisplayName("Status")]
        public int TaskStatusId { get; set; }

        public virtual TaskStatus TaskStatus { get; set; }

        [DisplayName("Assigned To")]
        public int? TeamMemberId { get; set; }

        public virtual TeamMember TeamMember { get; set; }

        [DisplayName("BacklogItem")]
        public int BacklogItemId { get; set; }

        public virtual BacklogItem BacklogItem { get; set; }
    }
}