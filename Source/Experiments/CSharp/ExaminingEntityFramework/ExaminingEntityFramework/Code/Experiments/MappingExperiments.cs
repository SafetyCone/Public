using System;
using System.Linq;
using System.Threading.Tasks;

using LinqKit;

using ExaminingEntityFramework.Lib;

using EntityTypes = ExaminingEntityFramework.Lib.EntityTypes;


namespace ExaminingEntityFramework
{
    public static class MappingExperiments
    {
        public static void SubMain(DatabaseContext databaseContext)
        {
            //MappingExperiments.QueryMapping(databaseContext);
            //MappingExperiments.QueryMappingsOrConcat(databaseContext);
            //MappingExperiments.QueryMappingsOrUnion(databaseContext);
            MappingExperiments.QueryMappingsUsingLinqKit(databaseContext);
        }

        /// <summary>
        /// Result: Expected. One query, works.
        /// Using LinqKit: https://github.com/scottksmith95/LINQKit, predicates should be buildable into an expression tree usable by EF.
        /// Expected: A single query with a combined where clause.
        /// </summary>
        /// <param name="databaseContext"></param>
        private static void QueryMappingsUsingLinqKit(DatabaseContext databaseContext)
        {
            var newB1 = new EntityTypes.EntityB();
            var newC1 = new EntityTypes.EntityC();

            var predicate = PredicateBuilder.New<EntityTypes.EntityBToEntityCMapping>(x => x.EntityB == newB1 && x.EntityC == newC1);

            var newB2 = new EntityTypes.EntityB();
            var newC2 = new EntityTypes.EntityC();

            predicate.Or(x => x.EntityB == newB2 && x.EntityC == newC2);

            var query = databaseContext.EntityBToEntityCMappings.Where(predicate);

            var mappings = query.ToList();
        }

        /// <summary>
        /// Result: Expected.
        /// What is the query issued by two where clauses, concatenated together to get an (AND) OR (AND)?
        /// Expected: Two queries, since the argument to Concat() is an IEnumerable, which means the query was executed to become the IEnumerable.
        /// </summary>
        private static void QueryMappingsOrUnion(DatabaseContext databaseContext)
        {
            var newB1 = new EntityTypes.EntityB();
            var newC1 = new EntityTypes.EntityC();

            var query1 = databaseContext.EntityBToEntityCMappings.Where(x => x.EntityB == newB1 && x.EntityC == newC1);

            var newB2 = new EntityTypes.EntityB();
            var newC2 = new EntityTypes.EntityC();

            var query2 = databaseContext.EntityBToEntityCMappings.Where(x => x.EntityB == newB2 && x.EntityC == newC2);

            var query1Or2 = query1.Union(query2);

            var mappings = query1Or2.ToList();
        }

        /// <summary>
        /// Result: Unexpected! Issues TWO queries! I guess this makes sense since the argument to Concat() is an IEnumerable, which means the query was executed to become the IEnumerable.
        /// What is the query issued by two where clauses, concatenated together to get an (AND) OR (AND)?
        /// Expected: A single WHERE (AND) OR (AND) clause.
        /// </summary>
        private static void QueryMappingsOrConcat(DatabaseContext databaseContext)
        {
            var newB1 = new EntityTypes.EntityB();
            var newC1 = new EntityTypes.EntityC();

            var query1 = databaseContext.EntityBToEntityCMappings.Where(x => x.EntityB == newB1 && x.EntityC == newC1);

            var newB2 = new EntityTypes.EntityB();
            var newC2 = new EntityTypes.EntityC();

            var query2 = databaseContext.EntityBToEntityCMappings.Where(x => x.EntityB == newB2 && x.EntityC == newC2);

            var query1Or2 = query1.Concat(query2);

            var mappings = query1Or2.ToList();
        }

        /// <summary>
        /// Result: Expected. Uses IDs of entities in a WHERE-AND clause.
        /// What is the query issued by a WHERE(entity AND entity)?
        /// Expected: A single WHERE-AND. 
        /// </summary>
        private static void QueryMapping(DatabaseContext databaseContext)
        {
            var newB = new EntityTypes.EntityB();
            var newC = new EntityTypes.EntityC();

            var query = databaseContext.EntityBToEntityCMappings.Where(x => x.EntityB == newB && x.EntityC == newC);

            var mapping = query.FirstOrDefault();
        }

        //private static void SeedDatabase(DatabaseContext databaseContext)
        //{
        //    int num
        //}
    }
}
