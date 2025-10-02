namespace Yume.Models.Client
{
    public class ClientModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Secret { get; set; }
        public required DateTimeOffset CreatedDate { get; set; }
        public required DateTimeOffset UpdatedDate { get; set; }
        public required bool IsActive { get; set; }
        public required Guid OwnerId { get; set; }
    }
}
