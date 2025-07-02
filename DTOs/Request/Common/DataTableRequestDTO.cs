using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Request.Common
{
    public class DataTableRequestDTO
    {
        [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers allowed.")]
        public int Draw { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers allowed.")]
        public int Start { get; set; } = 0;

        [RegularExpression("^[0-9]+$", ErrorMessage = "Numbers allowed.")]
        public int Length { get; set; } = 10;

        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string? searchValue { get; set; }
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string SortColumn { get; set; } = "CreatedOn";

        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string SortDirection { get; set; } = "desc";
    } 
}
