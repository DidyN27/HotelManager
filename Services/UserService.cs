using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hotel.Models;
using Hotel.ViewModels;

namespace Hotel.Services
{
    public class UserService
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
                query = query.Where(u => u.FirstName.Contains(searchString) ||
                                         u.LastName.Contains(searchString) ||
                                         u.UserName.Contains(searchString));
            }

            int totalUsers = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new UserIndexViewModel
            {
                Users = users,
                SearchString = searchString,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize)
            };
        }
    }
}