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

        public static void SubMain(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            //MappingExperiments.SubMain(databaseContext);

            //Experiments.TestLogging(serviceProvider);
            //Experiments.DoesEFUpdateAllTouchedOrOnlyChangedFields(databaseContext).Wait();
            Experiments.DoesLabelNeedAddingToContext(databaseContext).Wait();
        }

        /// <summary>
        /// Result: Expected. Addition entities must be added to the context.
        /// Does a label addition entity (an entity that references an entity) need to be added to the database context? Or is just referencing a tracked entity enough to make an entity tracked?
        /// Expected: No, the label is a separate entity and thus must be added to the context independently.
        /// </summary>
        private static async Task DoesLabelNeedAddingToContext(DatabaseContext databaseContext)
        {
            await databaseContext.ClearDatabase();

            var newA = new EntityTypes.EntityA()
            {
                GUID = Guid.NewGuid(),
                Value1 = @"Two",
                Value2 = 2,
            };

            //databaseContext.EntityAs.Add(newA); // Either works.
            databaseContext.Add(newA); // Either works. This is preferred because it is shorter!

            var newALabel = new EntityTypes.EntityALabel()
            {
                EntityA = newA,
            };

            databaseContext.Add(newALabel); // Required if you want the label entity to be added to the database. Note! Results in TWO queries being made to the DB.

            await databaseContext.SaveChangesAsync();
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
