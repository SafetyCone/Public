using System;


namespace Public.Common.Lib.Visuals
{
    public interface IRgbFloatImageRepository
    {
        void AddImage(ImageID imageID, RgbFloatImage image, bool forceReplace = false);
    }


    public static class IRgbFloatImageRepositoryExtensions
    {
        public static void AddImage(this IRgbFloatImageRepository repository, Tuple<ImageID, RgbFloatImage> imagePair, bool forceReplace = false)
        {
            repository.AddImage(imagePair.Item1, imagePair.Item2, forceReplace);
        }
    }
}
