using System.ComponentModel.DataAnnotations.Schema;
namespace TimPlan.Models
{
    public class TeamModel : DbModelBase<TeamModel>
    {
        [Column (DbIdCol)]
        public override int Id { get; set; }
        [Column(DbNameCol)]
        public string? Name { get; set; }

        public override string DbTableName => "teams";



        #region DbNames

        public const string DbNameCol = "name";

        #endregion

        public TeamModel()
        {
            
        }
    }
}
