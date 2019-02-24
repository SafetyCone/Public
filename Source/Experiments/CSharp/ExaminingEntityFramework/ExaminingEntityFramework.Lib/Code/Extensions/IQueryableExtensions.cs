using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


namespace ExaminingEntityFramework.Lib
{
    public static class IQueryableExtensions
    {
        public static ExistsResult<T> ExistsAndGetFirst<T>(this IQueryable<T> query)
            where T : class
        {
            var entityOrDefault = query.FirstOrDefault();

            var output = new ExistsResult<T>(entityOrDefault);
            return output;
        }

        public static async Task<ExistsResult<T>> ExistsAndGetFirstAsync<T>(this IQueryable<T> query)
            where T : class
        {
            var entityOrDefault = await query.FirstOrDefaultAsync();

            var output = new ExistsResult<T>(entityOrDefault);
            return output;
        }

        public static ExistsResult<T> ExistsAndGetSingle<T>(this IQueryable<T> query)
            where T : class
        {
            var entityOrDefault = query.SingleOrDefault();

            var output = new ExistsResult<T>(entityOrDefault);
            return output;
        }

        public static async Task<ExistsResult<T>> ExistsAndGetSingleAsync<T>(this IQueryable<T> query)
            where T : class
        {
            var entityOrDefault = await query.SingleOrDefaultAsync();

            var output = new ExistsResult<T>(entityOrDefault);
            return output;
        }
    }
}
