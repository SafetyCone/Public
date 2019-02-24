using System;
using System.Collections.Generic;
using System.Linq;


namespace ExaminingEntityFramework.Lib
{
    public static class IEnumerableExtensions
    {
        public static ExistsResult<T> ExistsAndGetFirst<T>(this IEnumerable<T> query)
            where T : class
        {
            var entityOrDefault = query.FirstOrDefault();

            var output = new ExistsResult<T>(entityOrDefault);
            return output;
        }

        public static ExistsResult<T> ExistsAndGetSingle<T>(this IEnumerable<T> query)
            where T : class
        {
            var entityOrDefault = query.SingleOrDefault();

            var output = new ExistsResult<T>(entityOrDefault);
            return output;
        }
    }
}
