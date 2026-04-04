using Hotel.ViewModels;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public interface IUserService
    {
        Task<UserIndexViewModel> GetFilteredUsersAsync(string searchString, int pageSize, int pageNumber);
    }
}