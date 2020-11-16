
using Reports.Extensions.Builders.Attributes;

namespace Reports.Demos.FromDb.ReportModels
{
    public class UsersListReport
    {
        [ReportVariable(1, "ID")]
        public int Id { get; set; }

        [ReportVariable(2, "First Name")]
        public string FirstName { get; set; }

        [ReportVariable(3, "Last Name")]
        public string LastName { get; set; }

        [ReportVariable(4, "Email")]
        public string Email { get; set; }

        [ReportVariable(5, "Orders #")]
        public int OrdersCount { get; set; }

        [ReportVariable(6, "Age")]
        public int Age { get; set; }

        [ReportVariable(7, "Registered Days")]
        public int RegisteredInDays { get; set; }
    }
}
