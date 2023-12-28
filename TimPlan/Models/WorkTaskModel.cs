using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class WorkTaskModel
    {
        [Column(DbIdCol)]
        public uint Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }
        [Column(DbParentTaskIdCol)]
        public uint ParentTaskID { get; set; }
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
        public uint CreatorUserID { get; set; }

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

        #endregion
        public WorkTaskModel()
        {
            
        }

    }
}
