using System;
using System.Collections;
using System.Collections.Generic;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// An RGB color image storage class.
    /// </summary>
    /// <remarks>
    /// Data is arranged in order of row-column-channel (with the channel dimension varying the fastest), and the channel order is Red, Green, then Blue.
    /// The color order is arranged in Red, Green, then Blue due the RGB convention.
    /// Most frequently, we will iterate through image locations, and set the Red, Green, and Blue values for that image location. Thus to avoid jumping around in memory, we keep the channel values for a given location next to each other.
    /// Because of the matrix convention to list rows before columns, rows are chosen to have a higher precendence over columns. Thus, iteration through image locations is done row-by-row.
    /// 
    /// Features:
    /// * Create an image of a certain XY-size.
    /// * Create an image of no size, then Reset() an image to a different XY-size (which also erases the image's current data).
    /// * Use an already existing data buffer, with a provided XY-size.
    /// 
    /// * Publically specify the number of color channels.
    /// * Publically specify the order of the color channels.
    /// 
    /// TODO: test inlining of offset calculations.
    /// </remarks>
    public class RgbFloatImage : IEnumerable<PixelLocation>
    {
        public const int NumberOfRgbColorChannels = RgbFloatColor.NumberOfRgbColorChannels;
        public const int RedChannelIndex = 0;
        public const int GreenChannelIndex = 1;
        public const int BlueChannelIndex = 2;


        #region Static

        public static int CalculateNumberOfElements(int rows_height_y, int columns_width_x)
        {
            int output = rows_height_y * columns_width_x * RgbFloatImage.NumberOfRgbColorChannels;
            return output;
        }

        /// <summary>
        /// Calculates the offset from the beginning of the internal array for a given row, column, and channel.
        /// This defines the dimensional order of data - row, then column, then channel.
        /// 
        /// A row-column version is supplied since it can be confusing to remember how X and Y relate to row and channel.
        /// </summary>
        public static int CalculateOffsetRowColumnStatic(int row, int column, int channel, int numberOfColumns)
        {
            int output = (row * numberOfColumns + column) * RgbFloatImage.NumberOfRgbColorChannels + channel;
            return output;
        }

        /// <summary>
        /// Get the offset of the start of the pixel (the Red channel value).
        /// </summary>
        public static int CalculateOffsetRowColumnStatic(int row, int column, int numberOfColumns)
        {
            int output = RgbFloatImage.CalculateOffsetRowColumnStatic(row, column, RgbFloatImage.RedChannelIndex, numberOfColumns);
            return output;
        }

        /// <summary>
        /// Calculates the offset from the beginning of internal array for a given X, Y, and channel.
        /// 
        /// An X-Y version is supplied since it can be confusing to remember how X and Y relate to row and channel.
        /// </summary>
        public static int CalculateOffsetXYStatic(int x, int y, int channel, int numberOfColumns)
        {
            int output = RgbFloatImage.CalculateOffsetRowColumnStatic(y, x, channel, numberOfColumns);
            return output;
        }

        /// <summary>
        /// Get the offset of the start of the pixel (the Red channel value).
        /// </summary>
        public static int CalculateOffsetXYStatic(int x, int y, int numberOfColumns)
        {
            int output = RgbFloatImage.CalculateOffsetRowColumnStatic(y, x, RgbFloatImage.RedChannelIndex, numberOfColumns);
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
        public float this[int row_height_y, int column_width_x, int channel]
        {
            get
            {
                int offset = RgbFloatImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns, channel);
                return this.Data[offset];
            }
            set
            {
                int offset = RgbFloatImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns, channel);
                this.Data[offset] = value;
            }
        }
        public RgbFloatColor this[int row_height_y, int column_width_x]
        {
            get
            {
                int offset = RgbFloatImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns);

                float redValue = this.Data[offset + RgbFloatImage.RedChannelIndex];
                float greenValue = this.Data[offset + RgbFloatImage.GreenChannelIndex];
                float blueValue = this.Data[offset + RgbFloatImage.BlueChannelIndex];

                RgbFloatColor output = new RgbFloatColor(redValue, greenValue, blueValue);
                return output;
            }
            set
            {
                int offset = RgbFloatImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns);

                this.Data[offset + RgbFloatImage.RedChannelIndex] = value.R;
                this.Data[offset + RgbFloatImage.GreenChannelIndex] = value.G;
                this.Data[offset + RgbFloatImage.BlueChannelIndex] = value.B;
            }
        }


        /// <summary>
        /// Allow size-less construction, with later reset to a different size.
        /// </summary>
        public RgbFloatImage()
            : this(0, 0)
        {
        }

        /// <summary>
        /// The preferred constructor.
        /// </summary>
        public RgbFloatImage(int rows_height_y, int columns_width_x)
        {
            this.SetInternals(rows_height_y, columns_width_x);
        }

        /// <summary>
        /// Use an already existing data buffer.
        /// </summary>
        public RgbFloatImage(int rows_height_y, int columns_width_x, float[] data)
        {
            // Check the input buffer.
            int expectedNumberOfElements = RgbFloatImage.CalculateNumberOfElements(rows_height_y, columns_width_x);
            int actualNumberOfElements = data.Length;
            if(expectedNumberOfElements != actualNumberOfElements)
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

            int numElements = RgbFloatImage.CalculateNumberOfElements(this.Rows, this.Columns);
            this.Data = new float[numElements];
        }

        public int CalculateOffsetRowColumn(int row, int column, int channel = RgbFloatImage.RedChannelIndex)
        {
            int output = RgbFloatImage.CalculateOffsetRowColumnStatic(row, column, channel, this.Columns);
            return output;
        }

        public int CalculateOffsetXY(int x, int y, int channel = RgbFloatImage.RedChannelIndex)
        {
            int output = RgbFloatImage.CalculateOffsetXYStatic(x, y, channel, this.Columns);
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
