using System;


namespace ExaminingEntityFramework.Lib
{
    public static class EntityAExtensions
    {
        public static AppTypes.EntityA ToAppType(this EntityTypes.EntityA entity)
        {
            var appType = new AppTypes.EntityA()
            {
                GUID = entity.GUID,
                Value1 = entity.Value1,
                Value2 = entity.Value2,
            };

            return appType;
        }

        public static EntityTypes.EntityA ToEntityType(this AppTypes.EntityA appType)
        {
            var entityType = new EntityTypes.EntityA()
            {
                GUID = appType.GUID,
                Value1 = appType.Value1,
                Value2 = appType.Value2,
            };

            return entityType;
        }

        public static void UpdateFrom(this EntityTypes.EntityA entityType, AppTypes.EntityA appType)
        {
            if(entityType.GUID != appType.GUID)
            {
                throw new Exception($@"AppType - EntityType GUID mismatch. AppType GUID: {appType.GUID}, EntityType GUID: {entityType.GUID}");
            }

            entityType.Value1 = appType.Value1;
            entityType.Value2 = appType.Value2;
        }
    }
}
