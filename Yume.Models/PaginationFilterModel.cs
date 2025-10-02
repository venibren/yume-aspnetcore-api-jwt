namespace Yume.Models
{
    /// <summary>
    /// Basic FilterModel class
    /// </summary>
    public class PaginationFilterModel
    {
        /// <summary>
        /// Requested order by list
        /// </summary>
        public List<PaginationSortResultsModel> OrderByList { get; set; } = [];
        /// <summary>
        /// Requested page size
        /// </summary>
        public int PageSize { get; set; } = 5;
        /// <summary>
        /// Requested page
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
