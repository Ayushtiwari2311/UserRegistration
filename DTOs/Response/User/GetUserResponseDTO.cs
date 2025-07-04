﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Response.User
{
    public class GetUserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? ContactNo { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public List<string> Hobbies { get; set; }
        public string? PhotoPath { get; set; }
    }

}
