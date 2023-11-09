namespace API.DTOs.Tokens
{
    public class ClaimsDto
    {
        public Guid EmployeeGuid { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string StatusAccount { get; set; }
        public string? AverageRating { get; set; }
        public string? Foto { get; set; }
        public List<string> Role { get; set; }
    }
}
