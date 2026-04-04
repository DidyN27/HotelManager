using Hotel.Models;

namespace Hotel.ViewModels
{
    public class UserIndexViewModel
    {
        public IEnumerable<UserListItemViewModel> Users { get; set; }
        public PagerViewModel Pager { get; set; }
        public string SearchString { get; set; }
    }
}