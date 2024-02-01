using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL.Entities.DTOs
{
    public class GeneralResponseModel
    {
        public Guid Id { get; set; } = default;
    }

    public class GameResponseModel
    {
        public Guid Id { get; set; } = default;
        public long Generations { get; set; } = default;
        public string Status { get; set; } = default;
        public long RunningTime { get; set; } = default;
    }

    public class GenerationResponseModel
    {
        public Guid GameId { get; set; }
        public int GenerationNumber { get; set; } = default;
        public IList<Position> LiveCells { get; set; } = default;
    }

    public class GameFinalStateResponseModel : GameResponseModel
    {
        public IList<GenerationResponseModel> GenerationHistory { get; set; }
    }
}
