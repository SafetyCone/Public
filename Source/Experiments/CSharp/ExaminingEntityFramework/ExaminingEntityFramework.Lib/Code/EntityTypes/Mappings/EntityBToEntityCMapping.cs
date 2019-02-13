using System;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    public class EntityBToEntityCMapping
    {
        public int ID { get; set; }

        public int EntityBID { get; set; }
        public EntityB EntityB { get; set; }

        public int EntityCID { get; set; }
        public EntityC EntityC { get; set; }
    }
}
