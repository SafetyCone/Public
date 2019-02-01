using System;
using System.Threading.Tasks;


namespace ExaminingEntityFramework.Lib
{
    public static class DatabaseContextExtensions
    {
        public static async Task ClearDatabase(this DatabaseContext context)
        {
            await context.EntityAs.DeleteAllFromDatabaseAsync();
        }
    }
}
