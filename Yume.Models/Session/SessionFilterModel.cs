using Yume.Enums;

namespace Yume.Models.Session
{
    public class SessionFilterModel : PaginationFilterModel
    {
        public List<Guid>? Ids { get; set; }
        public List<Guid>? OwnerIds { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? ExpiresFrom { get; set; }
        public DateTime? ExpiresTo { get; set; }
        public SessionTypeEnum? Type { get; set; }
    }
}
