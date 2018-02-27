using System;


namespace Eshunna.Lib.Match
{
    public class MatchFileHeader
    {
        public string Version { get; set; }
        /// <summary>
        /// The number of files to which the current matched image has been matched.
        /// </summary>
        public int FileCount { get; set; }
        /// <summary>
        /// Size of the match file definintion in bytes (buffer bytes used).
        /// </summary>
        public int DefinitionSize { get; set; }
        /// <summary>
        /// Total size of the definition in bytes (total buffer bytes).
        /// </summary>
        public int DefinitionBufferSize { get; set; }
        public int FeatureCount { get; set; }


        public MatchFileHeader(string version, int fileCount, int definitionSize, int definitionBuffer, int featureCount)
        {
            this.Version = version;
            this.FileCount = fileCount;
            this.DefinitionSize = definitionSize;
            this.DefinitionBufferSize = definitionBuffer;
            this.FeatureCount = featureCount;
        }

        public MatchFileHeader()
        {
        }
    }
}
