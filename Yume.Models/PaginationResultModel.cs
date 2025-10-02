namespace Yume.Models
{
    /// <summary>
    /// pagination result summary model
    /// </summary>
    public class PaginationResultModel<TItem>
    {
        /// <summary>
        /// List of items
        /// </summary>
        public List<TItem> Items { get; set; } = [];

        /// <summary>
        /// Currently focused page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int PageCount => PageSize > 0 ? (int)Math.Ceiling(TotalResults / (double)PageSize) : 1;

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items
        /// </summary>
        public int TotalResults { get; set; }
    }
}
