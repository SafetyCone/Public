using System;
using System.Collections;
using System.Collections.Generic;


namespace Public.Common.Lib.Visuals
{
    public class GrayFloatImage : IEnumerable<PixelLocation>
    {
        public const int NumberOfGrayColorChannels = 1;
        public const int GrayChannelIndex = 0;


        #region Static

        public static int CalculateNumberOfElements(int rows_height_y, int columns_width_x)
        {
            int output = rows_height_y * columns_width_x * GrayFloatImage.NumberOfGrayColorChannels;
            return output;
        }

        /// <summary>
        /// Calculates the offset from the beginning of the internal array for a given row, column, and channel.
        /// This defines the dimensional order of data - row, then column, then channel.
        /// 
        /// A row-column version is supplied since it can be confusing to remember how X and Y relate to row and channel.
        /// </summary>
        public static int CalculateOffsetRowColumnStatic(int row, int column, int numberOfColumns)
        {
            int output = row * numberOfColumns + column;
            return output;
        }

        /// <summary>
        /// Calculates the offset from the beginning of internal array for a given X, Y, and channel.
        /// 
        /// An X-Y version is supplied since it can be confusing to remember how X and Y relate to row and channel.
        /// </summary>
        public static int CalculateOffsetXYStatic(int x, int y, int numberOfColumns)
        {
            int output = GrayFloatImage.CalculateOffsetRowColumnStatic(y, x, numberOfColumns);
            return output;
        }

        #endregion


        public int Rows { get; protected set; }
        public int Height => this.Rows;
        public int SizeY => this.Rows;
        public int Columns { get; protected set; }
        public int Width => this.Columns;
        public int SizeX => this.Columns;
        /// <summary>
        /// Allows public read/write access to the image data. The data array itself cannot be replaced.
        /// </summary>
        public float[] Data { get; protected set; }
        public int DataSize => this.Data.Length;
        public float this[int row_height_y, int column_width_x]
        {
            get
            {
                int offset = GrayFloatImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns);
                return this.Data[offset];
            }
            set
            {
                int offset = GrayFloatImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns);
                this.Data[offset] = value;
            }
        }


        /// <summary>
        /// Allow size-less construction, with later reset to a different size.
        /// </summary>
        public GrayFloatImage()
            : this(0, 0)
        {
        }

        /// <summary>
        /// The preferred constructor.
        /// </summary>
        public GrayFloatImage(int rows_height_y, int columns_width_x)
        {
            this.SetInternals(rows_height_y, columns_width_x);
        }

        /// <summary>
        /// Use an already existing data buffer.
        /// </summary>
        public GrayFloatImage(int rows_height_y, int columns_width_x, float[] data)
        {
            // Check the input buffer.
            int expectedNumberOfElements = GrayFloatImage.CalculateNumberOfElements(rows_height_y, columns_width_x);
            int actualNumberOfElements = data.Length;
            if (expectedNumberOfElements != actualNumberOfElements)
            {
                throw new ArgumentException($"Unable to create image from data buffer. Size: {rows_height_y}x{columns_width_x}. Expected number of data buffer elements: {expectedNumberOfElements}, actual: {actualNumberOfElements}.");
            }

            this.Rows = rows_height_y;
            this.Columns = columns_width_x;
            this.Data = data;
        }

        private void SetInternals(int rows_height_y, int columns_width_x)
        {
            this.Rows = rows_height_y;
            this.Columns = columns_width_x;

            int numElements = GrayFloatImage.CalculateNumberOfElements(this.Rows, this.Columns);
            this.Data = new float[numElements];
        }

        public int CalculateOffsetRowColumn(int row, int column)
        {
            int output = GrayFloatImage.CalculateOffsetRowColumnStatic(row, column, this.Columns);
            return output;
        }

        public int CalculateOffsetXY(int x, int y)
        {
            int output = GrayFloatImage.CalculateOffsetXYStatic(x, y, this.Columns);
            return output;
        }

        /// <summary>
        /// Reset the image to a different size, erasing the current image data.
        /// </summary>
        public void Reset(int rows_height_y, int columns_width_x)
        {
            this.SetInternals(rows_height_y, columns_width_x);
        }

        public IEnumerator<PixelLocation> GetEnumerator()
        {
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                for (int iCol = 0; iCol < this.Columns; iCol++)
                {
                    yield return new PixelLocation(iRow, iCol);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
