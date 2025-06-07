using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MCity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public MState State { get; set; }
    }
}
