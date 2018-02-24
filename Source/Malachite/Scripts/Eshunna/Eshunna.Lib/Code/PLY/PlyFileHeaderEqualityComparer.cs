using System;
using System.Collections.Generic;
using System.Linq;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public class PlyFileHeaderEqualityComparer : IEqualityComparer<PlyFileHeader>
    {
        public PlyElementDescriptorEqualityComparer ElementComparer { get; }
        public ILog Log { get; }


        public PlyFileHeaderEqualityComparer(PlyElementDescriptorEqualityComparer elementComparer, ILog log)
        {
            this.ElementComparer = elementComparer;
            this.Log = log;
        }

        public PlyFileHeaderEqualityComparer(ILog log)
            : this(new PlyElementDescriptorEqualityComparer(log), log)
        {
        }

        public bool Equals(PlyFileHeader x, PlyFileHeader y)
        {
            bool output = true;

            bool dataFormatEquals = x.FileDataFormat == y.FileDataFormat;
            if(!dataFormatEquals)
            {
                output = false;

                string message = $@"File data formats not equal - x: {x.FileDataFormat.ToPlyFileDataFormatToken()}, y: {y.FileDataFormat.ToPlyFileDataFormatToken()}";
                this.Log.WriteLine(message);
            }

            bool fileVersionEquals = x.FileFormatVersion == y.FileFormatVersion;
            if(!fileVersionEquals)
            {
                output = false;

                string message = $@"File format versions not equal - x: {x.FileFormatVersion.ToString()}, y: {y.FileFormatVersion.ToString()}";
                this.Log.WriteLine(message);
            }

            int nCommentsX = x.Comments.Count;
            int nCommentsY = y.Comments.Count;
            bool nCommentsEqual = nCommentsX == nCommentsY;
            if (!nCommentsEqual)
            {
                output = false;

                string message = $@"Number of file comments not equal - x: {nCommentsX.ToString()}, y: {nCommentsY.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                bool commentsEqual = x.Comments.SequenceEqual(y.Comments);
                if(!commentsEqual)
                {
                    output = false;

                    string message = @"File comments not equal.";
                    this.Log.WriteLine(message);
                }
            }

            int nElementsX = x.Elements.Count;
            int nElementsY = y.Elements.Count;
            bool elementCountEquals = nElementsX == nElementsY;
            if(!elementCountEquals)
            {
                output = false;

                string message = $@"Element count mismatch: x: {nElementsX.ToString()}, y: {nElementsY.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iElement = 0; iElement < nElementsX; iElement++)
                {
                    PlyElementDescriptor elementX = x.Elements[iElement];
                    PlyElementDescriptor elementY = y.Elements[iElement];

                    bool elementsEqual = this.ElementComparer.Equals(elementX, elementY);
                    if(!elementsEqual)
                    {
                        output = false;

                        string message = $@"Element not equal: index: {iElement.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(PlyFileHeader obj)
        {
            int output = obj.GetHashCode();
            return output;
        }
    }
}
