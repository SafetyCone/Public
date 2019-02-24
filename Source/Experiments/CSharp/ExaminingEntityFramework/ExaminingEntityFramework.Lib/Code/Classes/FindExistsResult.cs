using System;


namespace ExaminingEntityFramework.Lib
{
    public class FindExistsResult<T>
        where T: class
    {
        public LocalOrRemote LocalOrRemote { get; set; }
        public bool Exists { get; set; }
        public T Entity { get; set; }


        public FindExistsResult()
        {
        }

        public FindExistsResult(LocalOrRemote localOrRemote, T entity)
            : this(localOrRemote, !(entity is null), entity)
        {
        }

        public FindExistsResult(LocalOrRemote localOrRemote, bool exists, T entity)
        {
            this.LocalOrRemote = localOrRemote;
            this.Exists = exists;
            this.Entity = entity;
        }
    }


    public static class ExistsResultExtensions
    {
        public static FindExistsResult<T> ToFindExistsResult<T>(this ExistsResult<T> existsResult, LocalOrRemote localOrRemote)
            where T: class
        {
            var output = new FindExistsResult<T>(localOrRemote, existsResult.Exists, existsResult.Entity);
            return output;
        }
    }
}
