using ListsAPI.Features.Profile.Enums;
using ListsAPI.Features.Profile.Tables;
using ListsAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile.DataAccess
{
    public interface IUserProfileTokenReader
    {
        Task<UserProfileToken> Get(string token, UserProfileTokenType tokenType);
    }

    public class UserProfileTokenReader : IUserProfileTokenReader
    {
        private readonly ListContext _context;

        public UserProfileTokenReader(ListContext context)
        {
            _context = context;
        }

        public async Task<UserProfileToken> Get(string token, UserProfileTokenType tokenType)
        {
            var storedToken = await _context.UserProfileTokens.SingleOrDefaultAsync(tok => tok.Token == token && tok.TokenType == tokenType);

            return storedToken;
        }
    }
}