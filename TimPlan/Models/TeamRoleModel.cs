using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Interfaces;

namespace TimPlan.Models
{
    public class TeamRoleModel : IDbRecord
    {
        [Column(DbIdCol)]
        public uint Id { get; set; }
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


        #region DbNames

        public const string DbTableName = "team_roles";
        public const string DbIdCol = "id";
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

        public override string ToString()
        {
            return $"Id={Id},Name={Name},CanEditAllTasks={CanEditAllTasks},CanEditCreatedTasks={CanEditCreatedTasks}," +
                $"CanAssignTasks={CanAssignTasks},CanViewForeignTasks={CanViewForeignTasks},CanViewAllTasks={CanViewAllTasks}";
        }
    }
}
