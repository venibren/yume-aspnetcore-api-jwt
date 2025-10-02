namespace Yume.Models
{
    public class PaginationResultViewModel<TItem, T> : PaginationResultModel<TItem>
        where T : PaginationResultViewModel<TItem, T>, new()
    {
        protected new List<TItem> Items { get; set; } = new List<TItem>();
        public static T Parse(PaginationResultModel<TItem> model)
        {
            //if (model == null) return null;
            return new T
            {
                Items = model.Items,
                CurrentPage = model.CurrentPage,
                PageSize = model.PageSize,
                TotalResults = model.TotalResults
            };
        }
    }
}
