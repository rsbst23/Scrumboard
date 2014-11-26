using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class BaseProjectObject : BaseObject
    {
        [Required]
        [DisplayName("Project")]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}