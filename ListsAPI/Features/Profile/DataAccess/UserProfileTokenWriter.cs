using ListsAPI.Features.Profile.Tables;
using ListsAPI.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile.DataAccess
{
    public interface IUserProfileTokenWriter
    {
        Task<string> Add(UserProfileToken token);

        Task Use(UserProfileToken token);
    }

    public class UserProfileTokenWriter : IUserProfileTokenWriter
    {
        private readonly ListContext _context;

        public UserProfileTokenWriter(ListContext context)
        {
            _context = context;
        }

        public async Task<string> Add(UserProfileToken token)
        {
            _context.UserProfileTokens.Add(token);

            await _context.SaveChangesAsync();

            return token.Token;
        }

        public async Task Use(UserProfileToken token)
        {
            token.DateUsed = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}