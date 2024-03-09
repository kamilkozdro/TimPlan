using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimPlan.Models
{
    public class TaskModel : DbModelBase
    {

        public enum TaskState { Suspended, Accepted, Completed }

        [Column(DbIdCol)]
        public override int Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }
        [Column(DbParentTaskIdCol)]
        public int? ParentTaskID { get; set; }
        [Column(DbDateCreatedCol)]
        public DateTime? DateCreated { get; set; }
        [Column(DbDateStartCol)]
        public DateTime? DateStart { get; set; }
        [Column(DbDateEndCol)]
        public DateTime DateEnd { get; set; }
        [Column(DbDescriptionCol)]
        public string? Description { get; set; }
        [Column(DbStateCol)]
        public TaskState State { get; set; } = TaskState.Suspended;
        [Column(DbPrivateCol)]
        public bool Private { get; set; }
        [Column(DbCreatorUserIdCol)]
        public int CreatorUserId { get; set; }
        [Column(DbUserIdCol)]
        public int UserId { get; set; }

        [Column(DbTeamIdCol)]
        public int? TeamId { get; set; }

        public override string DbTableName => "tasks";



        #region DbNames

        public const string DbNameCol = "name";
        public const string DbParentTaskIdCol = "parent_task_id";
        public const string DbDateCreatedCol = "date_created";
        public const string DbDateStartCol = "date_start";
        public const string DbDateEndCol = "date_end";
        public const string DbDescriptionCol = "description";
        public const string DbStateCol = "state";
        public const string DbPrivateCol = "private";
        public const string DbCreatorUserIdCol = "creator_user_id";
        public const string DbUserIdCol = "user_id";
        public const string DbTeamIdCol = "team_id";

        #endregion

        public TaskModel()
        {

        }

    }
}