using System.ComponentModel.DataAnnotations;

namespace Yume.Models.Client
{
    public class ClientUpdateModel : ClientCreateModel
    {
        [Required]
        public required Guid Id { get; set; }
    }
}
