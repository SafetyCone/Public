using System;
//using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using ExaminingEntityFramework.Lib;

using EntityTypes = ExaminingEntityFramework.Lib.EntityTypes;


namespace ExaminingEntityFramework
{
    public static class Demonstrations
    {
        public static void SubMain(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            Demonstrations.CorrectWayToUseContains(databaseContext);
        }

        /// <summary>
        /// This seems to have a transient error on the Contains() method, not sure how I made it go away, maybe by using System.Collections.Generic?
        /// This is probably caused by needing to select a PROPERTY of x, not just 'x'.
        /// </summary>
        /// <param name="databaseContext"></param>
        private static void CorrectWayToUseContains(DatabaseContext databaseContext)
        {
            var strings = new[] { @"A", @"B", @"C", @"D" };
            //var strings = new List<string>() { @"A", @"B", @"C", @"D" };

            //var query = databaseContext.EntityAs.Where(x => strings.Contains(x.Value1));

            Expression<Func<EntityTypes.EntityA, bool>> exp = x => strings.Contains(x.Value1);

            var query = databaseContext.EntityAs.Where(exp);
        }
    }
}
