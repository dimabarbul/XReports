using System;
using System.Collections.Generic;

namespace XReports.Demos.FromDb.Models
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

#pragma warning disable CA2227
        public ICollection<Order> Orders { get; set; }
#pragma warning restore CA2227
    }
}
