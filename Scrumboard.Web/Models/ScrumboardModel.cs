using Scrumboard.Web.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.Models
{
    public class ScrumboardModel
    {
        public IEnumerable<BacklogItem> BacklogItems { get; set; }

        public IEnumerable<TeamMember> TeamMembers { get; set; }

        public ScrumboardModel()
        {
            BacklogItems = new List<BacklogItem>();
            TeamMembers = new List<TeamMember>();
        }
    }
}