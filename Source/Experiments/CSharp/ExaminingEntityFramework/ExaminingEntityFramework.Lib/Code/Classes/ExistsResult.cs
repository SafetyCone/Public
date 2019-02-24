using System;


namespace ExaminingEntityFramework.Lib
{
    public class ExistsResult<T>
        where T : class
    {
        public bool Exists { get; set; }
        public T Entity { get; set; }


        public ExistsResult()
        {
        }

        public ExistsResult(T entity)
            : this(!(entity is null), entity)
        {
        }

        public ExistsResult(bool exists, T entity)
        {
            this.Exists = exists;
            this.Entity = entity;
        }
    }
}
