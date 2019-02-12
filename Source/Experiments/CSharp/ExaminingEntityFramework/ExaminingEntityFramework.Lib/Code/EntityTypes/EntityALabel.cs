using System;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    public class EntityALabel
    {
        public int ID { get; set; }

        public int EntityAID { get; set; }
        public EntityA EntityA { get; set; }
    }
}
