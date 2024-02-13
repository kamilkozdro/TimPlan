using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class TaskTypeModel : DbModelBase
    {
        [Column(DbIdCol)]
        public override int Id { get; set; }
        [Column(DbNameCol)]
        public string Name { get; set; }

        public override string DbTableName => "teams";


        #region DbNames

        public const string DbNameCol = "name";

        #endregion


        public TaskTypeModel()
        {
            
        }
    }
}
