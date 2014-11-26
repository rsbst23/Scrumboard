using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using Scrumboard.Web.Models;

namespace Scrumboard.Web.DAL
{
    public class ScrumboardDB : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feature>().HasOptional(x => x.Project).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Feature>().HasOptional(x => x.Release).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Feature>().HasOptional(x => x.BusinessValue).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Release>().HasOptional(x => x.Project).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Sprint>().HasRequired(x => x.Project).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>().HasRequired(x => x.TaskStatus).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<BacklogItem>().HasRequired(x => x.BacklogItemType).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<BacklogItem>().Ignore(x => x.ToDo);

            modelBuilder.Entity<BacklogItem>().Ignore(x => x.InProgress);

            modelBuilder.Entity<BacklogItem>().Ignore(x => x.Done);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BacklogItemStatus> BacklogItemStatus { get; set; }

        public DbSet<BusinessValue> BusinessValues { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Release> Releases { get; set; }

        public DbSet<Sprint> Sprints { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<BacklogItem> BacklogItems { get; set; }

        public DbSet<BacklogItemType> BacklogItemTypes { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}