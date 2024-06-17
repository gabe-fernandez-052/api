namespace JWT.Models
{
    public record User()
    {
        public string ClientId { get; init; }
        public string[] Roles { get; init; } = [];
    }
}