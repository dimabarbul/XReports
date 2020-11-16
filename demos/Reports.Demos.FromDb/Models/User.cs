using System;
using System.Collections.Generic;

namespace Reports.Demos.FromDb.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
