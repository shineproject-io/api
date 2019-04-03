using ListsAPI.Features.Lists.DataAccess;
using ListsAPI.Features.Profile.DataAccess;
using ListsAPI.Features.TodoItems.DataAccess;
using ListsAPI.Infrastructure.Database;
using System;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile.Services
{
    public interface IUserProfileDeletionManager
    {
        Task DeleteUserProfile(int userProfileId);
    }

    public class UserProfileDeletionManager : IUserProfileDeletionManager
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;
        private readonly IUserProfileWriter _userProfileWriter;
        private readonly IListWriter _listWriter;
        private readonly ITodoItemsWriter _todoItemsWriter;

        public UserProfileDeletionManager(
            IDatabaseConnectionProvider databaseConnectionProvider,
            IUserProfileWriter userProfileWriter,
            IListWriter listWriter,
            ITodoItemsWriter todoItemsWriter)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
            _userProfileWriter = userProfileWriter;
            _listWriter = listWriter;
            _todoItemsWriter = todoItemsWriter;
        }

        public async Task DeleteUserProfile(int userProfileId)
        {
            try
            {
                using (var con = _databaseConnectionProvider.New())
                {
                    con.Open();
                    var transaction = con.BeginTransaction();

                    try
                    {
                        await _userProfileWriter.DeleteUserProfile(userProfileId, transaction);
                        await _listWriter.DeleteListsByUserProfileId(userProfileId, transaction);
                        await _todoItemsWriter.DeleteTodoItemsByUserProfileId(userProfileId, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        con.Close();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}