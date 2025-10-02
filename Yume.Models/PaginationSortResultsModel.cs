using System.ComponentModel.DataAnnotations;

namespace Yume.Models
{
    public class PaginationSortResultsModel
    {
        [Required]
        public bool Ascending { get; set; }
        // todo: add "Allowed strings magic"
        [Required(AllowEmptyStrings = false)]
        public required string Field { get; set; }
    }
}
