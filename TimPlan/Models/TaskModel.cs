using System;
using System.ComponentModel.DataAnnotations.Schema;
using TimPlan.Interfaces;

namespace TimPlan.Models
{
    public class TaskModel :IDbRecord
    {
        [Column(DbIdCol)]
        public uint Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }
        [Column(DbParentTaskIdCol)]
        public uint? ParentTaskID { get; set; }
        [Column(DbDateCreatedCol)]
        public DateTime? DateCreated { get; set; }
        [Column(DbDateStartCol)]
        public DateTime? DateStart { get; set; }
        [Column(DbDateEndCol)]
        public DateTime? DateEnd { get; set; }
        [Column(DbDescriptionCol)]
        public string? Description { get; set; }
        [Column(DbIsCompletedCol)]
        public bool IsCompleted { get; set; }
        [Column(DbPrivateCol)]
        public bool Private { get; set; }
        [Column(DbCreatorUserIdCol)]
        public uint CreatorUserId { get; set; }
        [Column(DbUserIdCol)]
        public uint UserId { get; set; }

        [Column(DbTeamIdCol)]
        public uint? TeamId { get; set; }



        #region DbNames

        public const string DbTableName = "tasks";
        public const string DbIdCol = "id";
        public const string DbNameCol = "name";
        public const string DbParentTaskIdCol = "parent_task_id";
        public const string DbDateCreatedCol = "date_created";
        public const string DbDateStartCol = "date_start";
        public const string DbDateEndCol = "date_end";
        public const string DbDescriptionCol = "description";
        public const string DbIsCompletedCol = "is_completed";
        public const string DbPrivateCol = "private";
        public const string DbCreatorUserIdCol = "creator_user_id";
        public const string DbUserIdCol = "user_id";
        public const string DbTeamIdCol = "team_id";

        #endregion

        public TaskModel()
        {
            
        }



        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}, ParentTaskId:{ParentTaskID}," +
                $" DateCreated:{DateCreated}, DateStart:{DateStart}," +
                $" DateEnd:{DateEnd}, Description:{Description}," +
                $" IsCompleted:{IsCompleted}, Private:{Private}," +
                $" CreatorUserId:{CreatorUserId}, UserId:{UserId}," +
                $" TeamId:{TeamId}";
        }

    }
}
