using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GOL.Entities
{
    public class GameOfLifeGenerations : EntityBase
    {
        public Guid GameId { get; set; } = default;
        public int Generation { get; set; } = default;
        //Save a Position Json
        public string Live { get; set; } = default;


    }
}
