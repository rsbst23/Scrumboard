using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrumboard.Web.DAL
{
    public class BacklogItemType : BaseObject
    {
        [Required]
        [DisplayName("Type")]
        public string Title { get; set; }
    }
}