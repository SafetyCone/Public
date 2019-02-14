using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using LinqKit;

using ExaminingEntityFramework.Lib;

using EntityTypes = ExaminingEntityFramework.Lib.EntityTypes;


namespace ExaminingEntityFramework
{
    public static class Explorations
    {
        public static void SubMain(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            Explorations.CreateEntityACollectionQuery(databaseContext);
        }

        private static void CreateEntityACollectionQuery(DatabaseContext databaseContext)
        {
            var guids = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            Expression<Func<EntityTypes.EntityA, Guid>> guidIdentitySelectorExpression = entity => entity.GUID;

            var predicate = PredicateBuilder.New<EntityTypes.EntityA>();
            foreach (var guid in guids)
            {
                var guidIdentityEquals = Explorations.BuildExpressionForEqualsGuidIdentity(guidIdentitySelectorExpression, guid);

                predicate = predicate.Or(guidIdentityEquals);
            }

            var query = databaseContext.EntityAs.Where(predicate);

            var entityAs = query.ToList();
        }

        private static Expression<Func<EntityTypes.EntityA, bool>> BuildExpressionForEqualsGuidIdentity(Expression<Func<EntityTypes.EntityA, Guid>> guidIdentitySelectorExpression, Guid guid)
        {
            var guidIdentityConstant = Expression.Constant(guid);

            var entityParameter = Expression.Parameter(typeof(EntityTypes.EntityA), @"entity");

            var guidIdentityEqualBody = Expression.Equal(Expression.Invoke(guidIdentitySelectorExpression, entityParameter), guidIdentityConstant);

            var guidIdentityEqual = Expression.Lambda<Func<EntityTypes.EntityA, bool>>(guidIdentityEqualBody, entityParameter);
            return guidIdentityEqual;
        }
    }
}
