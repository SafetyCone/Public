using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace ExaminingEntityFramework.Lib
{
    public static class LocalViewExtensions
    {
        public static IEnumerable<TEntity> GetQuery<TEntity>(this LocalView<TEntity> local, Func<TEntity, bool> predicate)
            where TEntity: class
        {
            var output = local.Where(predicate);
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetFirst<TEntity>(this LocalView<TEntity> local, Func<TEntity, bool> predicate)
            where TEntity: class
        {
            var query = local.GetQuery(predicate);

            var output = local.ExistsAndGetFirst(query);
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetFirst<TEntity>(this LocalView<TEntity> local, IEnumerable<TEntity> query)
            where TEntity : class
        {
            var output = query.ExistsAndGetFirst();
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetSingle<TEntity>(this LocalView<TEntity> local, Func<TEntity, bool> predicate)
            where TEntity : class
        {
            var query = local.GetQuery(predicate);

            var output = local.ExistsAndGetSingle(query);
            return output;
        }

        public static ExistsResult<TEntity> ExistsAndGetSingle<TEntity>(this LocalView<TEntity> local, IEnumerable<TEntity> query)
            where TEntity : class
        {
            var output = query.ExistsAndGetSingle();
            return output;
        }
    }
}
