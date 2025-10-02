namespace Yume.Models.Session
{
    public class SessionPaginatedModel : PaginationResultViewModel<SessionViewModel, SessionPaginatedModel>
    {
        public IEnumerable<SessionViewModel> Sessions
        {
            get => Items;
            set => Items = value.ToList();
        }
    }
}
