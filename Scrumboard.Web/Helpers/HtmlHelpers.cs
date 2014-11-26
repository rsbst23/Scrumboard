using Scrumboard.Web.DAL;
using Scrumboard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Scrumboard.Web.Helpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Generates the proper HTML to represent a Product Backlog Item/Story, meant for consumption
        /// within a Scrumboard or Grooming Board.
        /// </summary>
        /// <param name="helper">The HtmlHelper to extend. This parameter is not actually used during the method,
        /// and so may be passed null without any adverse effects.</param>
        /// <param name="story">The Story to render.</param>
        /// <returns>The HTML code representing the provided Story.</returns>
        public static MvcHtmlString Story(this HtmlHelper helper, BacklogItem story)
        {
            var storyTag = new TagBuilder("dl");
            storyTag.AddCssClass("pbi");
            storyTag.Attributes.Add("id", story.Id.ToString());
            storyTag.Attributes.Add("onclick", "onSbiClicked(event, this);");

            if (story.Release != null)
            {
                storyTag.Attributes.Add("data-release", story.Release.Title);
            }

            var dtTag = new TagBuilder("dt");
            dtTag.SetInnerText("Story: ");

            var tfsLink = new TagBuilder("a");
            //string url = string.Format("{0}FTI%20Backlog/_workitems#_a=edit&id={1}", WebConfigurationManager.AppSettings["TfsServerLocation"], story.Id);
            string url = "hello";
            tfsLink.Attributes.Add("href", url);
            tfsLink.Attributes.Add("target", "_blank");
            tfsLink.SetInnerText(story.Id.ToString());
            dtTag.InnerHtml += tfsLink.ToString();

            storyTag.InnerHtml += dtTag.ToString();

            var titleTag = new TagBuilder("dd");
            titleTag.AddCssClass("title");

            if (story.Release != null)
            {
                titleTag.SetInnerText(string.Format("({0}) {1}", story.Release, story.Title));
            }
            else
            {
                titleTag.SetInnerText(story.Title);
            }



            storyTag.InnerHtml += titleTag.ToString();

            var storyPointsTag = new TagBuilder("dd");
            storyPointsTag.AddCssClass("workRemaining");
            storyPointsTag.AddCssClass("abbr");
            storyPointsTag.Attributes.Add("title", string.Format("Story Points: {0}", story.Points));
            storyPointsTag.SetInnerText(story.Points.ToString());
            storyTag.InnerHtml += storyPointsTag.ToString();

            //var estimatedEffortTag = new TagBuilder("dd");
            //estimatedEffortTag.AddCssClass("estimatedEffort");
            //estimatedEffortTag.AddCssClass("abbr");
            //estimatedEffortTag.Attributes.Add("title", string.Format("Estimated Effort: {0} hours", story.EstimatedEffort));
            //estimatedEffortTag.SetInnerText(story.EstimatedEffort.ToString());
            //storyTag.InnerHtml += estimatedEffortTag.ToString();

            var relativeValueTag = new TagBuilder("dd");
            relativeValueTag.AddCssClass("relativeValue");
            relativeValueTag.AddCssClass("abbr");
            relativeValueTag.Attributes.Add("title", string.Format("Relative Value: {0}", story.Priority));
            relativeValueTag.SetInnerText(story.Priority.ToString());
            storyTag.InnerHtml += relativeValueTag.ToString();

            var iterationPathTag = new TagBuilder("dd");
            iterationPathTag.AddCssClass("iterationPath");
            iterationPathTag.AddCssClass("abbr");
            iterationPathTag.Attributes.Add("title", string.Format("Sprint: {0}", story.Sprint.Title));
            iterationPathTag.SetInnerText(string.Format("Sprint: {0}", story.Sprint.Title));
            storyTag.InnerHtml += iterationPathTag.ToString();

            var statusTag = new TagBuilder("dd");
            statusTag.AddCssClass("status");
            statusTag.SetInnerText(story.BacklogItemStatus.Title);
            storyTag.InnerHtml += statusTag.ToString();

            return MvcHtmlString.Create(storyTag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Generates the proper HTML to represent a Task, meant for consumption within a Scrumboard.
        /// </summary>
        /// <param name="helper">The HtmlHelper to extend. This parameter is not actually used during the method,
        /// and so may be passed null without any adverse effects.</param>
        /// <param name="task">The Task to render.</param>
        /// <param name="pbiId">The TFS work item id of the Story/Bug this task is a child of.</param>
        /// <returns>The HTML code representing the provided Task.</returns>
        public static MvcHtmlString Task(this HtmlHelper helper, Task task, int pbiId)
        {
            var taskTag = new TagBuilder("dl");
            taskTag.AddCssClass("sbi");

            //if (task.Category != TaskCategory.None)
            //{
            //    taskTag.AddCssClass(string.Concat(task.Category.ToString().ToLower(), "-task"));
            //}

            taskTag.Attributes.Add("onclick", "onSbiClicked(event, this);");

            taskTag.Attributes.Add("id", task.Id.ToString());
            taskTag.Attributes.Add("data-pbiid", pbiId.ToString());
            taskTag.Attributes.Add("data-relativevalue", task.Priority.ToString());

            var dtTag = new TagBuilder("dt");
            dtTag.SetInnerText("Task: ");

            var handleTag = new TagBuilder("dd");
            handleTag.AddCssClass("handle");
            taskTag.InnerHtml += handleTag.ToString();

            var tfsLink = new TagBuilder("a");
            //string url = string.Format("{0}/Task/Edit/{1}", HttpContext.Current.Request.Url.AbsoluteUri, task.Id);
            string url = string.Format("/Task/Edit/{0}", task.Id);
            tfsLink.Attributes.Add("href", url);
            tfsLink.Attributes.Add("target", "_blank");
            tfsLink.SetInnerText(task.Id.ToString());
            dtTag.InnerHtml += tfsLink.ToString();

            taskTag.InnerHtml += dtTag.ToString();

            var titleTag = new TagBuilder("dd");
            titleTag.AddCssClass("title");
            titleTag.SetInnerText(task.Title);
            taskTag.InnerHtml += titleTag.ToString();

            var ownerTag = new TagBuilder("dd");
            ownerTag.AddCssClass("owner");
            ownerTag.AddCssClass("abbr");
            Dictionary<int, UserProfile> userProfiles = (Dictionary<int, UserProfile>)HttpContext.Current.Session["UserProfiles"];
            string ownerName = null;
            if (task.TeamMember != null)
            {
                ownerName = userProfiles[task.TeamMember.UserProfileId].UserName;
                ownerTag.Attributes.Add("title", ownerName);
            }

            string initials = string.Empty;

            if (task.TeamMember != null)
            {
                initials = ownerName;
                // string.Format("{0}{1}", task.AssignedTo[0], task.AssignedTo[task.AssignedTo.IndexOf(' ') + 1]);
            }

            ownerTag.SetInnerText(initials);
            taskTag.InnerHtml += ownerTag.ToString();

            //var workRemainingTag = new TagBuilder("dd");
            //workRemainingTag.AddCssClass("workRemaining");
            //workRemainingTag.AddCssClass("abbr");
            //workRemainingTag.Attributes.Add("title", string.Format("Work Remaining: {0} hours", task.WorkRemaining));
            //workRemainingTag.SetInnerText(task.WorkRemaining.ToString());
            //taskTag.InnerHtml += workRemainingTag.ToString();

            var relativeValueTag = new TagBuilder("dd");
            relativeValueTag.AddCssClass("relativeValue");
            relativeValueTag.AddCssClass("abbr");
            relativeValueTag.Attributes.Add("title", string.Format("Relative Value: {0}", task.Priority));
            relativeValueTag.SetInnerText(task.Priority.ToString());
            taskTag.InnerHtml += relativeValueTag.ToString();

            return MvcHtmlString.Create(taskTag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Generates the proper HTML to represent a Bug, meant for consumption within a Scrumboard or Grooming Board.
        /// </summary>
        /// <param name="helper">The HtmlHelper to extend. This parameter is not actually used during the method,
        /// and so may be passed null without any adverse effects.</param>
        /// <param name="bug">The Bug to render.</param>
        /// <returns>The HTML code representing the provided Bug.</returns>
        public static MvcHtmlString Bug(this HtmlHelper helper, BacklogItem bug)
        {
            var bugTag = new TagBuilder("dl");
            bugTag.AddCssClass("pbi");
            bugTag.AddCssClass("bug");

            bugTag.Attributes.Add("id", bug.Id.ToString());
            bugTag.Attributes.Add("onclick", "onSbiClicked(event, this);");

            if (bug.Release != null)
            {
                bugTag.Attributes.Add("data-release", bug.Release.Title);
            }

            var dtTag = new TagBuilder("dt");
            dtTag.SetInnerText("Bug: ");

            var tfsLink = new TagBuilder("a");
            //string url = string.Format("{0}FTI%20Backlog/_workitems#_a=edit&id={1}", WebConfigurationManager.AppSettings["TfsServerLocation"], bug.Id);
            string url = "hello";
            tfsLink.Attributes.Add("href", url);
            tfsLink.Attributes.Add("target", "_blank");
            tfsLink.SetInnerText(bug.Id.ToString());
            dtTag.InnerHtml += tfsLink.ToString();

            bugTag.InnerHtml += dtTag.ToString();

            var titleTag = new TagBuilder("dd");
            titleTag.AddCssClass("title");

            if (bug.Release != null)
            {
                titleTag.SetInnerText(string.Format("({0}) {1}", bug.Release, bug.Title));
            }
            else
            {
                titleTag.SetInnerText(bug.Title);
            }



            bugTag.InnerHtml += titleTag.ToString();

            var bugPointsTag = new TagBuilder("dd");
            bugPointsTag.AddCssClass("workRemaining");
            bugPointsTag.AddCssClass("abbr");
            bugPointsTag.Attributes.Add("title", string.Format("Bug Points: {0}", bug.Points));
            bugPointsTag.SetInnerText(bug.Points.ToString());
            bugTag.InnerHtml += bugPointsTag.ToString();

            //var estimatedEffortTag = new TagBuilder("dd");
            //estimatedEffortTag.AddCssClass("estimatedEffort");
            //estimatedEffortTag.AddCssClass("abbr");
            //estimatedEffortTag.Attributes.Add("title", string.Format("Estimated Effort: {0} hours", bug.EstimatedEffort));
            //estimatedEffortTag.SetInnerText(bug.EstimatedEffort.ToString());
            //bugTag.InnerHtml += estimatedEffortTag.ToString();

            var relativeValueTag = new TagBuilder("dd");
            relativeValueTag.AddCssClass("relativeValue");
            relativeValueTag.AddCssClass("abbr");
            relativeValueTag.Attributes.Add("title", string.Format("Relative Value: {0}", bug.Priority));
            relativeValueTag.SetInnerText(bug.Priority.ToString());
            bugTag.InnerHtml += relativeValueTag.ToString();

            var iterationPathTag = new TagBuilder("dd");
            iterationPathTag.AddCssClass("iterationPath");
            iterationPathTag.AddCssClass("abbr");
            iterationPathTag.Attributes.Add("title", string.Format("Sprint: {0}", bug.Sprint.Title));
            iterationPathTag.SetInnerText(string.Format("Sprint: {0}", bug.Sprint.Title));
            bugTag.InnerHtml += iterationPathTag.ToString();

            var statusTag = new TagBuilder("dd");
            statusTag.AddCssClass("status");
            statusTag.SetInnerText(bug.BacklogItemStatus.Title);
            bugTag.InnerHtml += statusTag.ToString();

            return MvcHtmlString.Create(bugTag.ToString(TagRenderMode.Normal));
        }
    }
}