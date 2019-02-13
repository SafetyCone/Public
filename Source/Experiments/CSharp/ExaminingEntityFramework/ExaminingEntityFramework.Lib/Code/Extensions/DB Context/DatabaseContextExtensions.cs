using System;
using System.Threading.Tasks;


namespace ExaminingEntityFramework.Lib
{
    public static class DatabaseContextExtensions
    {
        public static async Task ClearDatabase(this DatabaseContext context)
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
