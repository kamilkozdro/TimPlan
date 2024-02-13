using System.ComponentModel.DataAnnotations.Schema;

namespace TimPlan.Models
{
    public class TeamRoleModel : DbModelBase
    {
        [Column(DbIdCol)]
        public override int Id { get; set; }
        [Column(DbNameCol)]
        public string Name { get; set; }
        [Column(DbCanEditAllTasks)]
        public bool CanEditAllTasks { get; set; }
        [Column(DbCanEditCreatedTasks)]
        public bool CanEditCreatedTasks { get; set; }
        [Column(DbCanAssignTasks)]
        public bool CanAssignTasks { get; set; }
        [Column(DbCanViewForeignTasks)]
        public bool CanViewForeignTasks { get; set; }
        [Column(DbCanViewAllTasks)]
        public bool CanViewAllTasks { get; set; }

        public override string DbTableName => "team_roles";


        #region DbNames

        public const string DbNameCol = "name";
        public const string DbCanEditAllTasks = "edit_all_tasks";
        public const string DbCanEditCreatedTasks = "edit_created_tasks";
        public const string DbCanAssignTasks = "assign_tasks";
        public const string DbCanViewForeignTasks = "view_foreign_tasks";
        public const string DbCanViewAllTasks = "view_all_tasks";

        #endregion

        public TeamRoleModel()
        {
            
        }
    }
}
