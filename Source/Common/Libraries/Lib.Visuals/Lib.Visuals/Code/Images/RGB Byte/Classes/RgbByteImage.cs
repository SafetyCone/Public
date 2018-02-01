using System;

using Public.Common.Lib.Extensions;


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
    public class RgbByteImage
    {
        #region Static

        public static int CalculateNumberOfElements(int rows_height_y, int columns_width_x)
        {
            int output = rows_height_y * columns_width_x * RgbColor.NumberOfRgbColorChannels;
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
            int output = (row * numberOfColumns + column) * RgbColor.NumberOfRgbColorChannels + channel;
            return output;
        }

        /// <summary>
        /// Get the offset of the start of the pixel (the Red channel value).
        /// </summary>
        public static int CalculateOffsetRowColumnStatic(int row, int column, int numberOfColumns)
        {
            int output = RgbByteImage.CalculateOffsetRowColumnStatic(row, column, RgbColor.RedChannelIndex, numberOfColumns);
            return output;
        }

        /// <summary>
        /// Calculates the offset from the beginning of internal array for a given X, Y, and channel.
        /// 
        /// An X-Y version is supplied since it can be confusing to remember how X and Y relate to row and channel.
        /// </summary>
        public static int CalculateOffsetXYStatic(int x, int y, int channel, int numberOfColumns)
        {
            int output = RgbByteImage.CalculateOffsetRowColumnStatic(y, x, channel, numberOfColumns);
            return output;
        }

        /// <summary>
        /// Get the offset of the start of the pixel (the Red channel value).
        /// </summary>
        public static int CalculateOffsetXYStatic(int x, int y, int numberOfColumns)
        {
            int output = RgbByteImage.CalculateOffsetRowColumnStatic(y, x, RgbColor.RedChannelIndex, numberOfColumns);
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
        public byte[] Data { get; protected set; }
        public int DataSize => this.Data.Length;
        public byte this[int row_height_y, int column_width_x, int channel]
        {
            get
            {
                int offset = RgbByteImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns, channel);
                return this.Data[offset];
            }
            set
            {
                int offset = RgbByteImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns, channel);
                this.Data[offset] = value;
            }
        }
        public RgbByteColor this[int row_height_y, int column_width_x]
        {
            get
            {
                int offset = RgbByteImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns);

                byte redValue = this.Data[offset + RgbColor.RedChannelIndex];
                byte greenValue = this.Data[offset + RgbColor.GreenChannelIndex];
                byte blueValue = this.Data[offset + RgbColor.BlueChannelIndex];

                RgbByteColor output = new RgbByteColor(redValue, greenValue, blueValue);
                return output;
            }
            set
            {
                int offset = RgbByteImage.CalculateOffsetRowColumnStatic(row_height_y, column_width_x, this.Columns);

                this.Data[offset + RgbColor.RedChannelIndex] = value.R;
                this.Data[offset + RgbColor.GreenChannelIndex] = value.G;
                this.Data[offset + RgbColor.BlueChannelIndex] = value.B;
            }
        }


        /// <summary>
        /// Allow size-less construction, with later reset to a different size.
        /// </summary>
        public RgbByteImage()
            : this(0, 0)
        {
        }

        /// <summary>
        /// The preferred constructor.
        /// </summary>
        public RgbByteImage(int rows_height_y, int columns_width_x)
        {
            this.SetInternals(rows_height_y, columns_width_x);
        }

        /// <summary>
        /// Use an already existing data buffer.
        /// </summary>
        public RgbByteImage(int rows_height_y, int columns_width_x, byte[] data)
        {
            // Check the input buffer.
            int expectedNumberOfElements = RgbByteImage.CalculateNumberOfElements(rows_height_y, columns_width_x);
            int actualNumberOfElements = data.Length;
            if (expectedNumberOfElements != actualNumberOfElements)
            {
                throw new ArgumentException($@"Unable to create image from data buffer. Size: {rows_height_y}x{columns_width_x}. Expected number of data buffer elements: {expectedNumberOfElements}, actual: {actualNumberOfElements}.");
            }

            this.Rows = rows_height_y;
            this.Columns = columns_width_x;
            this.Data = data;
        }

        public RgbByteImage(RgbByteImage other)
        {
            this.Rows = other.Rows;
            this.Columns = other.Columns;
            this.Data = other.Data.Copy();
        }

        private void SetInternals(int rows_height_y, int columns_width_x)
        {
            this.Rows = rows_height_y;
            this.Columns = columns_width_x;

            int numElements = RgbByteImage.CalculateNumberOfElements(this.Rows, this.Columns);
            this.Data = new byte[numElements];
        }

        public int CalculateOffsetRowColumn(int row, int column, int channel = RgbColor.RedChannelIndex)
        {
            int output = RgbByteImage.CalculateOffsetRowColumnStatic(row, column, channel, this.Columns);
            return output;
        }

        public int CalculateOffsetXY(int x, int y, int channel = RgbColor.RedChannelIndex)
        {
            int output = RgbByteImage.CalculateOffsetXYStatic(x, y, channel, this.Columns);
            return output;
        }

        /// <summary>
        /// Reset the image to a different size, erasing the current image data.
        /// </summary>
        public void Reset(int rows_height_y, int columns_width_x)
        {
            this.SetInternals(rows_height_y, columns_width_x);
        }
    }
}
