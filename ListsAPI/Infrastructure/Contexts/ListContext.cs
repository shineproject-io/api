using ListsAPI.Features.Lists.Tables;
using ListsAPI.Features.Profile.Tables;
using ListsAPI.Features.TodoItems.Tables;
using Microsoft.EntityFrameworkCore;

namespace ListsAPI.Infrastructure
{
    public class ListContext : DbContext
    {
        public ListContext(DbContextOptions<ListContext> options) : base(options)
        {
        }

        public ListContext()
        {
        }

        public DbSet<List> Lists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileToken> UserProfileTokens { get; set; }
    }
}