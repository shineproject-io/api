using ListsAPI.Features.Lists.DataAccess;
using ListsAPI.Features.Lists.Tables;
using ListsAPI.Infrastructure.Authorisers.Models;
using System.Threading.Tasks;

namespace ListsAPI.Features.Lists.Authorizers
{
    public interface IListAuthoriser
    {
        Task<AuthorisationResponse<List>> IsOwner(int listId, int userProfileId);
    }

    public class ListAuthoriser : IListAuthoriser
    {
        private readonly IListReader _listReader;

        public ListAuthoriser(IListReader listReader)
        {
            _listReader = listReader;
        }

        public async Task<AuthorisationResponse<List>> IsOwner(int listId, int userProfileId)
        {
            var list = await _listReader.GetByListId(listId);

            var authorisationResponse = new AuthorisationResponse<List>
            {
                ResponseObject = list,
                AuthorisationResult = (list != null || list.UserId == userProfileId)
            };

            return authorisationResponse;
        }
    }
}