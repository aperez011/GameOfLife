using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL.Entities.DTOs
{
    public class StartGameRequest
    {
        // 0 => X access, 1 => Y access
        public List<Position> InitialPositions { get; set; } = default;

    }

    public class Position
    {
        public int X { get; set; } = default;
        public int Y { get; set; } = default;
    }
}
