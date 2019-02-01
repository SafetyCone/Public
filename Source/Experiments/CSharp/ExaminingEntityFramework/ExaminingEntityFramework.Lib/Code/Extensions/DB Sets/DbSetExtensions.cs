using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;


namespace ExaminingEntityFramework.Lib
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Get the <see cref="DbContext"/> of a <see cref="DbSet{TEntity}"/>.
        /// (A utility method for other DbSet extension methods.)
        /// </summary>
        /// <remarks>
        /// Source: https://dev.to/j_sakamoto/how-to-get-a-dbcontext-from-a-dbset-in-entityframework-core-c6m
        /// </remarks>
        public static DbContext GetDbContext<T>(this DbSet<T> set)
            where T : class
        {
            var infrastructure = set as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext))
                                       as ICurrentDbContext;
            return currentDbContext.Context;
        }

        /// <summary>
        /// Get the name of the database table name for a <see cref="DbSet{TEntity}"/>.
        /// </summary>
        /// <<remarks>
        /// Source: https://dev.to/j_sakamoto/how-to-get-the-actual-table-name-from-dbset-in-entityframework-core-20-56k0
        /// </remarks>
        public static string GetTableName<T>(this DbSet<T> set)
            where T : class
        {
            var context = set.GetDbContext();

            var model = context.Model;
            var entityTypes = model.GetEntityTypes();
            var entityType = entityTypes.First(t => t.ClrType == typeof(T));
            var tableNameAnnotation = entityType.GetAnnotation(@"Relational:TableName");
            var tableName = tableNameAnnotation.Value.ToString();
            return tableName;
        }

        /// <summary>
        /// Delete all entities from the database.
        /// </summary>
        public static async Task<int> DeleteAllFromDatabaseAsync<T>(this DbSet<T> set)
            where T : class
        {
            var context = set.GetDbContext();

            var tableName = set.GetTableName();
            var command = $@"DELETE FROM [{tableName}]";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            var rowCount = await context.Database.ExecuteSqlCommandAsync(command);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            return rowCount;
        }
    }
}
