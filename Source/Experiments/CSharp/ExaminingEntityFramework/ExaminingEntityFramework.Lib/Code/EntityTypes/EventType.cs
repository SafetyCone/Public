using System;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    public class EventType : IIDed, INamed
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
