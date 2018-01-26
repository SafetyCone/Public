using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PathExtensions = Public.Common.Lib.IO.Extensions.PathExtensions;
using Public.Common.Lib.Visuals.IO.Serialization;


namespace Public.Common.Lib.Visuals
{
    // Allows 
    public class RgbFloatImageDataFileCache : IDisposable, IRgbFloatImageRepository, IRgbFloatImageSource
    {
        private const string IndexFileName = @"Index.txt";
        private const string ImageDirectoryName = @"Images";
        private const char IndexTokenSeparator = '|';


        #region IDisposable

        private bool zDisposed = false;


        public void Dispose()
        {
            this.CleanUp(true);

            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Clean-up managed resource here.
                }

                // Clean-up unmanaged resources here.
                this.WriteIndex(); // Note, the index file is written with all managed code, but since it is a file it is an unmanaged resourece.
            }

            this.zDisposed = true;
        }

        ~RgbFloatImageDataFileCache()
        {
            this.CleanUp(false);
        }

        #endregion


        private string zDirectoryPath;
        public string DirectoryPath => this.zDirectoryPath;
        private string IndexFilePath { get; set; }
        private string ImagesDirectoryPath { get; set; }
        private Dictionary<string, Tuple<string, string>> DataFileLocationsByImageFileLocation { get; set; } = new Dictionary<string, Tuple<string, string>>();


        public RgbFloatImageDataFileCache(string directoryPath)
        {
            this.SetDirectoryPath(directoryPath);
        }

        private void WriteIndex()
        {
            using (StreamWriter writer = new StreamWriter(this.IndexFilePath))
            {
                char separator = RgbFloatImageDataFileCache.IndexTokenSeparator;
                foreach(var pair in this.DataFileLocationsByImageFileLocation)
                {
                    string line = $@"{pair.Key}{separator}{pair.Value.Item2}{separator}{pair.Value.Item1}";
                    writer.WriteLine(line);
                }
            }
        }

        private void ReadIndex()
        {
            using (StreamReader reader = new StreamReader(this.IndexFilePath))
            {
                char[] separators = new char[] { RgbFloatImageDataFileCache.IndexTokenSeparator };
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string[] tokens = line.Split(separators, StringSplitOptions.None);
                    string rgbLocationToken = tokens[0];
                    string datLocationToken = tokens[1];
                    string titleToken = tokens[2];

                    this.AddIndexEntry(rgbLocationToken, titleToken, datLocationToken);
                }
            }
        }

        private void AddIndexEntry(string rgbFilePath, string title, string dataFilePath)
        {
            this.DataFileLocationsByImageFileLocation.Add(rgbFilePath, Tuple.Create(title, dataFilePath));
        }

        public bool IsAlreadyCached(string rgbFilePath)
        {
            bool output = this.DataFileLocationsByImageFileLocation.ContainsKey(rgbFilePath);
            return output;
        }

        private void SetDirectoryPath(string directoryPath)
        {
            // No need to save the current in-memory index. This method will only be called from constructors thus there will never be a prior in-memory index.

            this.zDirectoryPath = directoryPath;

            // Create index file path, and if it exists, read it in.
            this.IndexFilePath = Path.Combine(this.DirectoryPath, RgbFloatImageDataFileCache.IndexFileName);
            if(File.Exists(this.IndexFilePath))
            {
                this.ReadIndex();
            }

            // Create the images directory path, and if it does not exist, create it.
            this.ImagesDirectoryPath = Path.Combine(this.DirectoryPath, RgbFloatImageDataFileCache.ImageDirectoryName);
            if(!Directory.Exists(this.ImagesDirectoryPath))
            {
                Directory.CreateDirectory(this.ImagesDirectoryPath);
            }
        }

        public void AddImage(ImageID imageID, RgbFloatImage image, bool forceReplace = false)
        {
            // Is the image already in the index?
            if (this.IsAlreadyCached(imageID.Location))
            {
                if (forceReplace)
                {
                    this.RemoveImage(imageID);
                }
                else
                {
                    return;
                }
            }

            // Get the image data file path.
            string guidStr = Guid.NewGuid().ToString().ToUpperInvariant();
            string fileName = $@"{guidStr}{PathExtensions.WindowsFileExtensionSeparatorChar}{ImageFormatFileExtensions.BinaryDataExtension}";
            string dataFilePath = Path.Combine(this.ImagesDirectoryPath, fileName);

            // Save the image.
            RgbFloatImageBinarySerializer.Serialize(dataFilePath, image);

            // Add the index entry.
            this.AddIndexEntry(imageID.Location, imageID.Title, dataFilePath);
        }

        public void RemoveImage(ImageID imageID)
        {
            string dataFilePath = this.DataFileLocationsByImageFileLocation[imageID.Location].Item2;
            File.Delete(dataFilePath);

            this.DataFileLocationsByImageFileLocation.Remove(imageID.Location);
        }

        public IEnumerator<Tuple<ImageID, RgbFloatImage>> GetEnumerator()
        {
            foreach(string initialImageFilePath in this.DataFileLocationsByImageFileLocation.Keys)
            {
                Tuple<string, string> titleAndRgbFloatImageDataFilePath = this.DataFileLocationsByImageFileLocation[initialImageFilePath];

                ImageID imageID = new ImageID(titleAndRgbFloatImageDataFilePath.Item1, initialImageFilePath);
                RgbFloatImage rgbFloatImage = RgbFloatImageBinarySerializer.Deserialize(titleAndRgbFloatImageDataFilePath.Item2);

                yield return Tuple.Create(imageID, rgbFloatImage);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
