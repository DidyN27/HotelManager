using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hotel.Models;
using Hotel.ViewModels;

namespace Hotel.Services
{
    public class UserService : IUserService 
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserIndexViewModel> GetFilteredUsersAsync(string searchString, int pageSize, int pageNumber)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(u => u.FirstName.Contains(searchString) ||
                                         u.LastName.Contains(searchString) ||
                                         u.Email.Contains(searchString) ||
                                         u.EGN.Contains(searchString));
            }

            int totalUsers = await query.CountAsync();
            
            var userList = await query
                .OrderBy(u => u.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListItemViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Email = u.Email,
                    EGN = u.EGN,
                    IsActive = u.IsActive,
                    AppointmentDate = u.AppointmentDate,
                    DismissalDate = u.DismissalDate
                })
                .ToListAsync();

            return new UserIndexViewModel
            {
                Users = userList,
                SearchString = searchString,
                Pager = CreatePagerViewModel(totalUsers, pageSize, pageNumber)
            };
        }

        private PagerViewModel CreatePagerViewModel(int totalUsers, int pageSize, int pageNumber)
        {
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            int currentPage = pageNumber;
            if (currentPage < 1) currentPage = 1;
            if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;

            return new PagerViewModel
            {
                TotalItems = totalUsers,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                StartPage = 1,
                EndPage = totalPages
            };
        }
    }
}