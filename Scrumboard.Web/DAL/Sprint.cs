using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class Sprint : BaseProjectObject
    {
        [Required]
        [DisplayName("Sprint")]
        public string Title { get; set; }
    }
}