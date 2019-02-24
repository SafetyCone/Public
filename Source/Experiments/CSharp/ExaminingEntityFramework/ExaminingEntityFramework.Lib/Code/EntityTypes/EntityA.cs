using System;


namespace ExaminingEntityFramework.Lib.EntityTypes
{
    public class EntityA
    {
        #region Static

        public static EntityA NewDefault()
        {
            var output = new EntityA()
            {
                GUID = Guid.NewGuid(),
                Value1 = @"Two",
                Value2 = 2,
            };

            return output;
        }

        #endregion


        public int ID { get; set; }
        public Guid GUID { get; set; }

        public string Value1 { get; set; }
        public int Value2 { get; set; }

        public EntityALabel Label { get; set; }
    }
}
