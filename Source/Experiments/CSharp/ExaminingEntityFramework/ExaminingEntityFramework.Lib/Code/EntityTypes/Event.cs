using System;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    public class Event : IIDed, IGUIDed
    {
        public int ID { get; set; }
        public Guid GUID { get; set; }

        public int EventTypeID { get; set; }
        public EventType EventType { get; set; }
    }
}
