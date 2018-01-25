using System;
using System.Collections;
using System.Collections.Generic;

using Public.Common.Lib.Visuals;


namespace MyTheia.Lib
{
    public abstract class FloatImage : IEnumerable<PixelLocation>
    {
        #region Static

        public static int CalculateNumberOfElements(int rows_height_y, int columns_width_x, int numberOfChannels)
        {
            int output = rows_height_y * columns_width_x * numberOfChannels;
            return output;
        }

        /// <summary>
        /// Calculates the offset from the beginning of the internal array for a given row, column, and channel.
        /// This defines the dimensional order of data - row, then column, then channel.
        /// </summary>
        public static int CalculateOffsetRowThenColumnStatic(int row, int column, int channel, int numberOfColumns, int numberOfChannels)
        {
            int output = (row * numberOfColumns + column) * numberOfChannels + channel;
            return output;
        }

        #endregion


        public int Rows { get; protected set; }
        public int Height => this.Rows;
        public int SizeY => this.Rows;
        public int Columns { get; protected set; }
        public int Width => this.Columns;
        public int SizeX => this.Columns;
        public abstract int NumberOfChannels { get; }
        public float[] Data { get; protected set; }
        public int DataSize => this.Data.Length;


        /// <summary>
        /// Allow size-less construction, with later reset to a different size.
        /// </summary>
        public FloatImage()
            : this(0, 0)
        {
        }

        /// <summary>
        /// The preferred constructor.
        /// </summary>
        public FloatImage(int rows_height_y, int columns_width_x)
        {
            this.SetInternals(rows_height_y, columns_width_x);
        }

        private void SetInternals(int rows_height_y, int columns_width_x)
        {
            this.Rows = rows_height_y;
            this.Columns = columns_width_x;

            int numElements = FloatImage.CalculateNumberOfElements(this.Rows, this.Columns, this.NumberOfChannels);
            this.Data = new float[numElements];
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
