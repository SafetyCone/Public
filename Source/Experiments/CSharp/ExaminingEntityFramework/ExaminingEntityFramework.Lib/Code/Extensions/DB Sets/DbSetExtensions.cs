using System;
using System.Linq;
using System.Linq.Expressions;
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
        public static int DeleteAllFromDatabase<T>(this DbSet<T> set)
            where T : class
        {
            var context = set.GetDbContext();

            var tableName = set.GetTableName();
            var command = $@"DELETE FROM [{tableName}]";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            var rowCount = context.Database.ExecuteSqlCommand(command);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            return rowCount;
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

        public static IQueryable<TEntity> GetQuery<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var output = set.Where(predicate);
            return output;
        }

        #region Acquire

        /// <summary>
        /// Acquires (gets or constructs) a single simple entity.
        /// </summary>
        public static TEntity Acquire<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> identityExpression, Func<TEntity> constructor)
            where TEntity : class
        {
            var existsResult = set.ExistsAndGetSingle(identityExpression);
            if (existsResult.Exists)
            {
                return existsResult.Entity;
            }
            else
            {
                var newEntity = constructor();

                set.Add(newEntity);

                return newEntity;
            }
        }

        /// <summary>
        /// Acquires (gets or constructs) a single simple entity asynchronously.
        /// </summary>
        public static async Task<TEntity> AcquireAsync<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> identityExpression, Func<TEntity> constructor)
            where TEntity : class
        {
            var existsResult = await set.ExistsAndGetSingleAsync(identityExpression);
            if (existsResult.Exists)
            {
                return existsResult.Entity;
            }
            else
            {
                var newEntity = constructor();

                set.Add(newEntity);

                return newEntity;
            }
        }

        public static TEntity AcquireFind<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> identityExpression, Func<TEntity> constructor)
            where TEntity : class
        {
            var existsResult = set.ExistsAndGetSingleFind(identityExpression);
            if (existsResult.Exists)
            {
                return existsResult.Entity;
            }
            else
            {
                var newEntity = constructor();

                set.Add(newEntity);

                return newEntity;
            }
        }

        #endregion

        #region Exists And Get

        public static ExistsResult<TEntity> ExistsAndGetFirst<TEntity>(this DbSet<TEntity> set, IQueryable<TEntity> query)
            where TEntity : class
        {
            var output = query.ExistsAndGetFirst();
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetFirst<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var query = set.GetQuery(predicate);

            var output = set.ExistsAndGetFirst(query);
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetSingle<TEntity>(this DbSet<TEntity> set, IQueryable<TEntity> query)
            where TEntity : class
        {
            var output = query.ExistsAndGetSingle();
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetSingle<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var query = set.GetQuery(predicate);

            var output = set.ExistsAndGetSingle(query);
            return output;
        }

        public static Task<ExistsResult<TEntity>> ExistsAndGetSingleAsync<TEntity>(this DbSet<TEntity> set, IQueryable<TEntity> query)
            where TEntity : class
        {
            var output = query.ExistsAndGetSingleAsync();
            return output;
        }

        public static Task<ExistsResult<TEntity>> ExistsAndGetSingleAsync<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var query = set.GetQuery(predicate);

            var output = set.ExistsAndGetSingleAsync(query);
            return output;
        }

        public static FindExistsResult<TEntity> ExistsAndGetSingleFind<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
            where TEntity: class
        {
            var predicateFunction = predicate.Compile();

            var localResult = set.Local.ExistsAndGetSingle(predicateFunction);
            if(localResult.Exists)
            {
                var output = localResult.ToFindExistsResult(LocalOrRemote.Local);
                return output;
            }
            else
            {
                var remoteResult = set.ExistsAndGetSingle(predicate);

                var output = localResult.ToFindExistsResult(LocalOrRemote.Remote);
                return output;
            }
        }

        public static async Task<FindExistsResult<TEntity>> ExistsAndGetSingleFindAsync<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var predicateFunction = predicate.Compile();

            var localResult = set.Local.ExistsAndGetSingle(predicateFunction);
            if (localResult.Exists)
            {
                var output = localResult.ToFindExistsResult(LocalOrRemote.Local);
                return output;
            }
            else
            {
                var remoteResult = await set.ExistsAndGetSingleAsync(predicate);

                var output = localResult.ToFindExistsResult(LocalOrRemote.Remote);
                return output;
            }
        }

        #endregion
    }
}
