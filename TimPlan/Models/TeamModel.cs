using System.ComponentModel.DataAnnotations.Schema;
namespace TimPlan.Models
{
    public class TeamModel : DbRecordBase<TeamModel>
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
    }
}
