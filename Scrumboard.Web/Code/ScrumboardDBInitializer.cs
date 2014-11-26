using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Scrumboard.Web.DAL;

namespace Scrumboard.Web.Code
{
    public class ScrumboardDBInitializer : DropCreateDatabaseIfModelChanges<ScrumboardDB>
    {
        protected override void Seed(ScrumboardDB context)
        {
            base.Seed(context);

            LoadBacklogItemStatuses(context);

            LoadBusinessValues(context);

            LoadTaskStatuses(context);

            LoadBacklogItemTypes(context);
        }

        protected void LoadBacklogItemStatuses(ScrumboardDB context)
        {
            context.BacklogItemStatus.Add(new DAL.BacklogItemStatus { Title = "1. New" });
            context.BacklogItemStatus.Add(new DAL.BacklogItemStatus { Title = "2. Approved" });
            context.BacklogItemStatus.Add(new DAL.BacklogItemStatus { Title = "3. Committed" });
            context.BacklogItemStatus.Add(new DAL.BacklogItemStatus { Title = "4. In Progress" });
            context.BacklogItemStatus.Add(new DAL.BacklogItemStatus { Title = "5. Done" });
            context.BacklogItemStatus.Add(new DAL.BacklogItemStatus { Title = "X. Removed" });
        }

        protected void LoadBusinessValues(ScrumboardDB context)
        {
            context.BusinessValues.Add(new DAL.BusinessValue { Title = "1. Must Have" });
            context.BusinessValues.Add(new DAL.BusinessValue { Title = "2. Great" });
            context.BusinessValues.Add(new DAL.BusinessValue { Title = "3. Good" });
            context.BusinessValues.Add(new DAL.BusinessValue { Title = "4. Average" });
            context.BusinessValues.Add(new DAL.BusinessValue { Title = "5. Nice to Have" });
        }

        protected void LoadTaskStatuses(ScrumboardDB context)
        {
            context.TaskStatuses.Add(new DAL.TaskStatus { Title = "To Do" });
            context.TaskStatuses.Add(new DAL.TaskStatus { Title = "In Progress" });
            context.TaskStatuses.Add(new DAL.TaskStatus { Title = "Done" });
        }

        protected void LoadBacklogItemTypes(ScrumboardDB context)
        {
            context.BacklogItemTypes.Add(new DAL.BacklogItemType { Title = "Story" });
            context.BacklogItemTypes.Add(new DAL.BacklogItemType { Title = "Bug" });
        }
    }
}