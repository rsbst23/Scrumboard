using Scrumboard.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class TeamMember : BaseObject
    {
        [Required]
        public int UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        [Required]
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }

        public string UserName
        {
            get
            {
                return UserProfile.UserName;
            }
        }
    }
}