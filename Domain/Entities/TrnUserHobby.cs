using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TrnUserHobby
    {
        public Guid UserId { get; set; }
        public int HobbyId { get; set; }

        public TrnUserRegistration User { get; set; }
        public MHobby Hobby { get; set; }
    }
}
