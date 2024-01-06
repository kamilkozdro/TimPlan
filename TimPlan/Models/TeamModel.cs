using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Interfaces;

namespace TimPlan.Models
{
    public class TeamModel : IDbRecord
    {
        [Column (DbIdCol)]
        public uint Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }

        #region DbNames

        public const string DbTableName = "teams";
        public const string DbIdCol = "id";
        public const string DbNameCol = "name";

        #endregion

        public TeamModel()
        {
            
        }

        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}";
        }
    }
}
