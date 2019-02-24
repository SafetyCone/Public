using System;
using System.Threading.Tasks;


namespace ExaminingEntityFramework.Lib
{
    public static class DatabaseContextExtensions
    {
        public static void ClearDatabase(this DatabaseContext context)
        {
            context.EntityBToEntityCMappings.DeleteAllFromDatabase();

            context.EntityAs.DeleteAllFromDatabase();
            context.EntityBs.DeleteAllFromDatabase();
            context.EntityCs.DeleteAllFromDatabase();

            context.Posts.DeleteAllFromDatabase();
            context.Blogs.DeleteAllFromDatabase();
        }

        public static async Task ClearDatabaseAsync(this DatabaseContext context)
        {
            await context.EntityBToEntityCMappings.DeleteAllFromDatabaseAsync();
            
            await context.EntityAs.DeleteAllFromDatabaseAsync();
            await context.EntityBs.DeleteAllFromDatabaseAsync();
            await context.EntityCs.DeleteAllFromDatabaseAsync();

            await context.Posts.DeleteAllFromDatabaseAsync();
            await context.Blogs.DeleteAllFromDatabaseAsync();
        }
    }
}
