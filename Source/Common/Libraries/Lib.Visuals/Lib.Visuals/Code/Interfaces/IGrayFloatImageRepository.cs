using System;


namespace Public.Common.Lib.Visuals
{
    public interface IGrayFloatImageRepository
    {
        void AddImage(ImageID imageID, GrayFloatImage image, bool forceReplace = false);
    }


    public static class IGrayFloatImageRepositoryExtensions
    {
        public static void AddImage(this IGrayFloatImageRepository repository, Tuple<ImageID, GrayFloatImage> imagePair, bool forceReplace = false)
        {
            repository.AddImage(imagePair.Item1, imagePair.Item2, forceReplace);
        }
    }
}
