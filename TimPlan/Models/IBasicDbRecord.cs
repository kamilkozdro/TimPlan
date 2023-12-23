using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimPlan.Models
{
    public interface IBasicDbRecord
    {
        public string DbName { get; }
        public uint Id { get; set; }
    }
}
