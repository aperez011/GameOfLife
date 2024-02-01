using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL.Entities
{
    public class GameOfLifeHeader : EntityBase
    {

        public DateTime StartTime { get; set; } = default;
        public DateTime? EndTime { get; set; } = default;
        //NOTA: User the enum GOLStatus
        public string Status { get; set; } = default;
    }
}
