namespace Ambev.DeveloperEvaluation.ORM.Dtos.User
{
    public class ListUserQueryResult
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;   
        public string Status { get; set;} = string.Empty;   
        public string Role { get; set; } = string.Empty;
    }
}
