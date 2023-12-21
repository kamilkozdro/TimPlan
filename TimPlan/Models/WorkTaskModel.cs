using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public class WorkTaskModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool IsCompleted { get; set; }
        public bool Private { get; set; }
        public int CreatorUserID { get; set; }
        public string? Description { get; set; }


        public WorkTaskModel()
        {
            
        }

    }
}
