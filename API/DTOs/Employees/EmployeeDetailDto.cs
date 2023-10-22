namespace API.DTOs.Employees
{
    public class EmployeeDetailDto
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StatusEmployee { get; set; }
        public string NameCompany { get; set; }
        public string Address { get; set; }
        public string HardSkill { get; set; }
        public string SoftSkill { get; set; }
        public string Experience { get; set; }
        public string Position { get; set; }
        public string CompanyExperience { get; set; }
        public string EmployeeOwner { get; set; }
        public Guid OwnerGuid { get; set; }
        public double? AverageRating { get; set; }

    }
}
