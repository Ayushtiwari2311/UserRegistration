using DataTransferObjects.Request.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Request.User
{
    public class GetUserRequestListDTO : DataTableRequestDTO
    {
        public int? StateId { get; set; }
        public int? CityId { get; set; }
    }
}
