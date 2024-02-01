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
        public IList<Position> ActiveCells { get; set; } = default;
    }

    public class GameOfLifeStateModel
    {
        public GameOfLifeStateModel()
        {
            this.CurrentGeneration = new List<Position>();
            this.NewGeneration = new List<Position>();
        }

        public List<Position> CurrentGeneration { get; set; } = default;
        public List<Position> NewGeneration { get; set; } = default;
    }
}
