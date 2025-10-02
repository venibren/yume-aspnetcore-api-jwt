using System.ComponentModel.DataAnnotations;

namespace Yume.Models.Client
{
    public class ClientCreateModel
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
