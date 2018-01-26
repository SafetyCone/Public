using System;
using System.Drawing;
using System.Drawing.Imaging;

using Public.Common.Lib.IO.Serialization;
using Public.Common.Lib.Logging;
using LoggingUtilities = Public.Common.Lib.Logging.Utilities;


namespace Public.Common.Lib.Visuals.MSWindows
{
    /// <summary>
    /// De/serializes RGB byte images to/from all formats supported by the MS Windows Bitmap object.
    /// </summary>
    public class RgbByteImageSerializer : IInstrumentedFileSerializer<RgbByteImage>
    {
        #region Static

        public static RgbByteImage Deserialize(string filePath, LoggingTiming loggingTimimg = default(LoggingTiming))
        {
            // Ensure we have a logger. From a static context, use the session log.
            var logger = Loggers.GetLoggerOrDefault(loggingTimimg.SessionLogger);

            // Timing node.
            var deserialize = StopwatchTimingNode.GetList($@"Deserialize RGB Byte Image: {filePath}", loggingTimimg.TimingNode);

            // Load the bitmap.
            logger.Info($@"Deserializing to RGB Byte Image: {filePath}...");

            var loadBitmap = StopwatchTimingNode.GetLeaf(@"Load image.", deserialize);

            Bitmap bitmap = new Bitmap(filePath);
            loadBitmap.Stop();

            int rows = bitmap.Height;
            int columns = bitmap.Width;
            LoggingUtilities.LogDuration(logger, loadBitmap, Level.Info, $@"Loaded image. Size: rows (height): {rows.ToString()}, rolumns (width): {columns.ToString()}.");

            // Create the output.
            logger.Info(@"Creating RGB Byte Image...");

            var createRgbByteImage = StopwatchTimingNode.GetLeaf(@"Create RGB Byte Image.", deserialize);

            RgbByteImage output = new RgbByteImage(rows, columns);
            byte[] destinationData = output.Data;

            // Copy data from bitmap internals to the RGB byte image byte array.
            Rectangle rect = new Rectangle(0, 0, columns, rows);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                int destinationBytesPerPixel = 3;
                int sourceBytesPerPixel;
                switch (bitmap.PixelFormat)
                {
                    case PixelFormat.Format24bppRgb:
                        sourceBytesPerPixel = 3;
                        break;

                    case PixelFormat.Format32bppArgb:
                        sourceBytesPerPixel = 4;
                        break;

                    default:
                        throw new FormatException($@"Unhandled pixel format: {bitmap.PixelFormat.ToString()}");
                }

                int blueOffset = 0;
                int greenOffset = 1;
                int redOffset = 2;

                unsafe
                {
                    int iSourceByte = 0;
                    int iDestinationByte = 0;
                    byte r;
                    byte g;
                    byte b;
                    byte* sourcePointer = (byte*)bitmapData.Scan0;
                    for (int iRow = 0; iRow < rows; iRow++)
                    {
                        iSourceByte = iRow * bitmapData.Stride;
                        for (int iCol = 0; iCol < columns; iCol++)
                        {
                            b = sourcePointer[iSourceByte + blueOffset];
                            g = sourcePointer[iSourceByte + greenOffset];
                            r = sourcePointer[iSourceByte + redOffset];

                            destinationData[iDestinationByte + 0] = r;
                            destinationData[iDestinationByte + 1] = g;
                            destinationData[iDestinationByte + 2] = b;

                            iSourceByte += sourceBytesPerPixel;
                            iDestinationByte += destinationBytesPerPixel;
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            createRgbByteImage.Stop();

            LoggingUtilities.LogDuration(logger, createRgbByteImage, Level.Info, @"Created RGB byte image.");

            deserialize.Stop();

            LoggingUtilities.LogDuration(logger, deserialize, Level.Info, $@"Deserialized RGB byte image: {filePath}");

            return output;
        }

        public static void Serialize(string filePath, RgbByteImage image, LoggingTiming logggingTimimg = default(LoggingTiming), bool overwrite = true)
        {
            int rows = image.Rows;
            int columns = image.Columns;

            byte[] source = image.Data;
            int sourceBytesPerPixel = 3;

            Bitmap bitmap = new Bitmap(columns, rows, PixelFormat.Format24bppRgb);

            Rectangle rect = new Rectangle(0, 0, columns, rows);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int destinationBytesPerPixel = 3;

            int blueOffset = 0;
            int greenOffset = 1;
            int redOffset = 2;

            unsafe
            {
                int iSourceByte = 0;
                int iDestinationByte = 0;
                byte r;
                byte g;
                byte b;
                byte* destinationPointer = (byte*)bitmapData.Scan0;
                for (int iRow = 0; iRow < rows; iRow++)
                {
                    iDestinationByte = iRow * bitmapData.Stride;
                    for (int iCol = 0; iCol < columns; iCol++)
                    {
                        r = source[iSourceByte + 0];
                        g = source[iSourceByte + 1];
                        b = source[iSourceByte + 2];

                        destinationPointer[iDestinationByte + blueOffset] = b;
                        destinationPointer[iDestinationByte + greenOffset] = g;
                        destinationPointer[iDestinationByte + redOffset] = r;

                        iSourceByte += sourceBytesPerPixel;
                        iDestinationByte += destinationBytesPerPixel;
                    }
                }
            }

            // Determine the image format.
            System.Drawing.Imaging.ImageFormat imageFormat = FileExtensions.DetermineImageFormatFromFileExtension(filePath);

            bitmap.Save(filePath, imageFormat);
        }

        #endregion


        private ILogger Logger { get; }


        public RgbByteImageSerializer(ILogger logger)
        {
            this.Logger = logger;

            this.Logger.Info($@"Created new {nameof(RgbByteImageSerializer)}");
        }

        public RgbByteImageSerializer()
            :this(Loggers.GetDefaultLogger())
        {
        }

        public RgbByteImage this[string filePath, LoggingTiming loggingTiming = default(LoggingTiming), bool overwrite = true]
        {
            get
            {
                string endMessage = $@"Deserialized {filePath}";
                RgbByteImage output = LoggingUtilities.FunctionLogDuration(this.Logger, () => RgbByteImageSerializer.Deserialize(filePath, loggingTiming), Level.Info, endMessage);
                return output;
            }
            set
            {
                string endMessage = $@"Serialized {filePath}";
                LoggingUtilities.ActionLogDuration(this.Logger, () => RgbByteImageSerializer.Serialize(filePath, value, loggingTiming, overwrite), Level.Info, endMessage);
            }
        }
    }
}
