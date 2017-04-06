using System;

using Public.Common.Lib;
using Augustus.Extensions;


namespace Augustus
{
    public class BuildItemFactory
    {
        #region Static

        public static IBuildItem GetBuildItem(string buildItemSpecification)
        {
            BuildItemSpecification specification = BuildItemSpecification.Parse(buildItemSpecification);

            IBuildItem output = BuildItemFactory.GetBuildItem(specification);
            return output;
        }

        public static IBuildItem GetBuildItem(string buildItemSpecification, char tokenSeparator)
        {
            BuildItemSpecification specification = BuildItemSpecification.Parse(buildItemSpecification, tokenSeparator);

            IBuildItem output = BuildItemFactory.GetBuildItem(specification);
            return output;
        }

        public static IBuildItem GetBuildItem(BuildItemSpecification buildItemSpecification)
        {
            IBuildItem output;
            switch(buildItemSpecification.Platform)
            {
                case Platform.Cygwin:
                    output = BuildItemFactory.GetCygwinBuildItem(buildItemSpecification);
                    break;

                case Platform.Windows:
                    output = BuildItemFactory.GetWindowsBuildItem(buildItemSpecification);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(buildItemSpecification.Platform);
            }

            return output;
        }

        private static IBuildItem GetCygwinBuildItem(BuildItemSpecification buildItemSpecification)
        {
            var output = new CygwinBuildItem(buildItemSpecification.BuildFilePath, buildItemSpecification.Platform);
            return output;
        }

        private static IBuildItem GetWindowsBuildItem(BuildItemSpecification buildItemSpecification)
        {
            var output = new WindowsBuildItem(buildItemSpecification.BuildFilePath, buildItemSpecification.Platform, buildItemSpecification.Architecture);
            return output;
        }

        #endregion
    }
}
