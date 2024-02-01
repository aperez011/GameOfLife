using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL.Entities
{
    public partial class EntityBase
    {
        public int Id { get; set; } = default;

        public Guid GID { get; set; } = default;
    }
}
