using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class BacklogItem : BaseProjectObject
    {
        public BacklogItem()
        {
            Tasks = new List<Task>();
        }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Team")]
        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }

        [DisplayName("Business Value")]
        public int? BusinessValueId { get; set; }

        public virtual BusinessValue BusinessValue { get; set; }

        public int? Points { get; set; }

        public int? Priority { get; set; }

        [DisplayName("Release")]
        public int? ReleaseId { get; set; }

        public virtual Release Release { get; set; }

        [DisplayName("Sprint")]
        public int? SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }

        [DisplayName("Feature")]
        public int? FeatureId { get; set; }

        public virtual Feature Feature { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public int? BacklogItemStatusId { get; set; }

        public virtual BacklogItemStatus BacklogItemStatus { get; set; }

        [Required]
        [DisplayName("Type")]
        public int? BacklogItemTypeId { get; set; }

        public virtual BacklogItemType BacklogItemType { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public ICollection<Task> ToDo
        {
            get
            {
                return Tasks.Where(x => x.TaskStatusId == 1).ToList();
            }
        }

        public ICollection<Task> InProgress
        {
            get
            {
                return Tasks.Where(x => x.TaskStatusId == 2).ToList();
            }
        }

        public ICollection<Task> Done
        {
            get
            {
                return Tasks.Where(x => x.TaskStatusId == 3).ToList();
            }
        }
    }
}