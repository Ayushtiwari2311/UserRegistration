using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TrnUserRegistration
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string ContactNo { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string PhotoPath { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public MGender Gender { get; set; }
        public MState State { get; set; }
        public MCity City { get; set; }

        public ICollection<TrnUserHobby> UserHobbies { get; set; }
    }
}
