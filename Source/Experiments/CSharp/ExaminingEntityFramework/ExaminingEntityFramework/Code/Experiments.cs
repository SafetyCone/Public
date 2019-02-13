using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ExaminingEntityFramework.Lib;

using AppTypes = ExaminingEntityFramework.Lib.AppTypes;
using EntityTypes = ExaminingEntityFramework.Lib.EntityTypes;


namespace ExaminingEntityFramework
{
    public class Experiments
    {
        #region Static

        public static void SubMain()
        {
            var serviceProvider = Program.BuildServiceProvider();

            var databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            MappingExperiments.SubMain(databaseContext);

            //Experiments.TestLogging(serviceProvider);
            //Experiments.DoesEFUpdateAllTouchedOrOnlyChangedFields(databaseContext).Wait();
        }

        /// <summary>
        /// Result: FALSE! Only the modified field was updated in the databse.
        /// EF keeps track of changes that occurred to entities (as in, changes to the values of fields of entity objects).
        /// Upon SaveChanges(), EF updates all changed fields in the database.
        /// The question is, does it update ONLY fields that have been changed, or all fields that have been touched (changed).
        /// Expected: All touched fields.
        /// 
        /// This is great, it means that only fields with changed values will be updated in the database.
        /// </summary>
        private static async Task DoesEFUpdateAllTouchedOrOnlyChangedFields(DatabaseContext databaseContext)
        {
            await databaseContext.ClearDatabase();

            var appType1 = new AppTypes.EntityA()
            {
                GUID = Guid.NewGuid(),
                Value1 = @"String",
                Value2 = 5,
            };

            var entityType1 = appType1.ToEntityType();

            databaseContext.Add(entityType1);
            await databaseContext.SaveChangesAsync();

            appType1.Value1 = @"New String";

            entityType1.UpdateFrom(appType1);

            await databaseContext.SaveChangesAsync(); // Information-level logging shows that only Value1 was included in the query!
        }

        private static void TestLogging(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Experiments>>();

            logger.LogDebug(@"Hello world!");
            logger.LogInformation(@"Hello world!");
            logger.LogWarning(@"Hello world!");

            System.Threading.Thread.Sleep(1000); // Required due to console logging being asynchronous on another thread.
        }

        #endregion
    }
}
