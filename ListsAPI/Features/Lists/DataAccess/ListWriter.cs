using Dapper;
using ListsAPI.Features.Lists.Enums;
using ListsAPI.Features.Lists.Tables;
using ListsAPI.Infrastructure;
using ListsAPI.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListsAPI.Features.Lists.DataAccess
{
    public interface IListWriter
    {
        Task<int> Add(int userId, string name, string description, ListState state, string backgroundImageFilePath);

        Task DeleteList(int listId, ListState state);

        Task ChangeState(int listId, ListState state);

        Task ChangePicture(int listId, string backgroundImageFilePath, string backgroundImageFileName);

        Task ChangeName(int listId, string name);

        Task ChangeDescription(int listId, string description);

        void ChangeOrder(IEnumerable<List> lists, List<int> orderedListIds);
    }

    public class ListWriter : IListWriter
    {
        private ListContext _context;
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public ListWriter(ListContext context, IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _context = context;
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<int> Add(int userId, string name, string description, ListState state, string backgroundImageFilePath)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var createdDate = DateTime.UtcNow;

                var listId = Convert.ToInt32(await con.ExecuteScalarAsync(@"
                    INSERT INTO
                        Lists
                            (UserId, Name, Description, State, DateCreated, DateUpdated, BackgroundImageFilePath)
                        VALUES
                            (@userId, @name, @description, @state, @createdDate, @createdDate, @backgroundImageFilePath)

                    SELECT SCOPE_IDENTITY()", new
                {
                    userId,
                    name,
                    description,
                    state,
                    createdDate,
                    backgroundImageFilePath
                }));

                return listId;
            }
        }

        public async Task DeleteList(int listId, ListState state)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        State = @state,
                        BackgroundImageFileName = '',
                        BackgroundImageFilePath = '',
                        DateUpdated = updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    state,
                    updateDate
                });
            }
        }

        public async Task ChangeState(int listId, ListState state)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        State = @state,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    state,
                    updateDate
                });
            }
        }

        public async Task ChangePicture(int listId, string backgroundImageFilePath, string backgroundImageFileName)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        BackgroundImageFilePath = @backgroundImageFilePath,
                        BackgroundImageFileName = @backgroundImageFileName,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    backgroundImageFilePath,
                    backgroundImageFileName,
                    updateDate
                });
            }
        }

        public async Task ChangeName(int listId, string name)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        Name = @name,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    name,
                    updateDate
                });
            }
        }

        public async Task ChangeDescription(int listId, string description)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        Description = @description,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    description,
                    updateDate
                });
            }
        }

        public void ChangeOrder(IEnumerable<List> lists, List<int> orderedListIds)
        {
            lists.Select(lst => { lst.Position = null; return lst; }).ToList();

            for (int loop = 0; loop < orderedListIds.Count; loop += 1)
            {
                var list = lists.FirstOrDefault(lst => lst.Id == orderedListIds[loop]);
                list.DateUpdated = DateTime.UtcNow;

                list.Position = loop + 1;
            }

            _context.SaveChanges();
        }
    }
}