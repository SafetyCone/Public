using System.Collections.Generic;

using Public.Common.MATLAB;


namespace Public.Common.Lib.Visuals.MATLAB
{
    public static class PublicCommonLibVisualsCommands
    {
        private static int GetMatlabImageIndex(int nRows, int nCols, int nColorChannels, int row, int column, int colorChannel)
        {
            int output = (nRows * nCols) * colorChannel + nRows * column + row;
            return output;
        }

        private static int[] GetMatlabImageIndices(int nRows, int nCols, int nColorChannels)
        {
            int[] output = new int[nRows * nCols * nColorChannels];
            int iCount = 0;
            for (int iRow = 0; iRow < nRows; iRow++)
            {
                for (int iCol = 0; iCol < nCols; iCol++)
                {
                    for (int iColorChannel = 0; iColorChannel < nColorChannels; iColorChannel++)
                    {
                        int index = PublicCommonLibVisualsCommands.GetMatlabImageIndex(nRows, nCols, nColorChannels, iRow, iCol, iColorChannel);
                        output[iCount] = index;
                        iCount++;
                    }
                }
            }

            return output;
        }

        public static void PutRgbImage(this MatlabApplication matlabApplication, string variableName, RgbByteImage image)
        {
            // Need to reorder the byte array.
            byte[] imageData = image.Data;
            int nElements = imageData.Length;
            int nRows = image.Rows;
            int nCols = image.Columns;
            int nColorChannels = RgbColor.NumberOfRgbColorChannels;
            int[] size = new int[] { nRows, nCols, nColorChannels };

            byte[] matlabByteArray = new byte[nElements];

            int[] matlabIndices = PublicCommonLibVisualsCommands.GetMatlabImageIndices(nRows, nCols, nColorChannels);
            for (int iElement = 0; iElement < nElements; iElement++)
            {
                byte value = imageData[iElement];
                int matlabIndex = matlabIndices[iElement];

                matlabByteArray[matlabIndex] = value;
            }

            // Put the byte array into MATLAB and reshape it into an image.
            using (Variable byteArrayVar = new Variable(matlabApplication, matlabByteArray))
            {
                matlabApplication.Reshape(byteArrayVar.Name, variableName, size);
            }
        }

        public static RgbByteImage GetRgbImage(this MatlabApplication matlabApplication, string variableName)
        {
            int[] size = matlabApplication.Size(variableName);

            byte[] matlabimageData;
            using (Variable byteArrayVar = new Variable(matlabApplication))
            {
                string command = $@"{byteArrayVar.Name} = reshape({variableName}, [1, numel({variableName})]);";
                matlabApplication.Execute(command);

                matlabimageData = matlabApplication.GetRowArray<byte>(byteArrayVar.Name);
            }

            // Reorder the image data.
            int nElements = matlabimageData.Length;
            int nRows = size[0];
            int nCols = size[1];
            int nColorChannels = RgbColor.NumberOfRgbColorChannels;

            int[] matlabIndices = PublicCommonLibVisualsCommands.GetMatlabImageIndices(nRows, nCols, nColorChannels);

            byte[] imageData = new byte[nElements];
            for (int iElement = 0; iElement < nElements; iElement++)
            {
                int matlabIndex = matlabIndices[iElement];
                byte value = matlabimageData[matlabIndex];

                imageData[iElement] = value;
            }

            // Create the output image.
            RgbByteImage output = new RgbByteImage(size[0], size[1], imageData);
            return output;
        }
    }
}
