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
        public int Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }

        //TODO: Define privileges
        public bool? CanCreateOwnTasks { get; set; }
        public bool? CanEditOwnTasks { get; set; }
        public bool? CanCreateForeignTasks { get; set; }
        public bool? CanEditForeignTasks { get; set; }
        public bool? CanReadForeignTasks { get; set; }

        #region DbNames

        public const string DbTableName = "team_roles";
        public const string DbIdCol = "id";
        public const string DbNameCol = "name";

        #endregion

        public TeamRoleModel()
        {
            
        }

    }
}
