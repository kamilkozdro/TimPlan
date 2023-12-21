using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class TeamRoleModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        //TODO: Define privileges
        public bool? CanCreateOwnTasks { get; set; }
        public bool? CanEditOwnTasks { get; set; }
        public bool? CanCreateForeignTasks { get; set; }
        public bool? CanEditForeignTasks { get; set; }
        public bool? CanReadForeignTasks { get; set; }


        public TeamRoleModel()
        {
            
        }

    }
}
