using XReports.SchemaBuilders.Attributes;

namespace XReports.Demos.FromDb.ReportModels
{
    public class UsersListReport
    {
        [ReportColumn(1, "ID")]
        public int Id { get; set; }

        [ReportColumn(2, "First Name")]
        public string FirstName { get; set; }

        [ReportColumn(3, "Last Name")]
        public string LastName { get; set; }

        [ReportColumn(4, "Email")]
        public string Email { get; set; }

        [ReportColumn(5, "Orders #")]
        public int OrdersCount { get; set; }

        [ReportColumn(6, "Age")]
        public int Age { get; set; }

        [ReportColumn(7, "Registered Days")]
        public int RegisteredInDays { get; set; }
    }
}
