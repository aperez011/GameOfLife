using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL.Entities.DTOs
{
    public class StartGameModel
    {
        public Guid Id { get; set; } = default;
        public HashSet<Position> ActiveCells { get; set; } = default;
    }

    public class GameOfLifeStateModel
    {
        public GameOfLifeStateModel()
        {
            this.CurrentGeneration = new HashSet<Position>();
            this.NewGeneration = new HashSet<Position>();
        }

        public HashSet<Position> CurrentGeneration { get; set; } = default;
        public HashSet<Position> NewGeneration { get; set; } = default;
    }
}
