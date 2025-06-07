using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetUserRegistrationListDTO : DTODataTablesRequest
    {
        public int? StateId { get; set; }
        public int? CityId { get; set; }
    }
}
