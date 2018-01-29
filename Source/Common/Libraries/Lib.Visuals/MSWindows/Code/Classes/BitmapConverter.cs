using System;
using System.Drawing;
using System.Drawing.Imaging;


namespace Public.Common.Lib.Visuals.MSWindows
{
    public class BitmapConverter
    {
        #region Static

        public static Bitmap ToBitmap(RgbByteImage rgbByteImage)
        {
            int rows = rgbByteImage.Rows;
            int columns = rgbByteImage.Columns;
            byte[] source = rgbByteImage.Data;

            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
            Bitmap bitmap = new Bitmap(columns, rows, pixelFormat);

            Rectangle rect = new Rectangle(0, 0, columns, rows);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int sourceBytesPerPixel = RgbColor.NumberOfRgbColorChannels;
            int destinationBytesPerPixel = pixelFormat.BytesPerPixel();

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
                        // Order is R, G, B.
                        r = source[iSourceByte + 0];
                        g = source[iSourceByte + 1];
                        b = source[iSourceByte + 2];

                        // Order is B, G, R.
                        destinationPointer[iDestinationByte + 0] = b;
                        destinationPointer[iDestinationByte + 1] = g;
                        destinationPointer[iDestinationByte + 2] = r;

                        iSourceByte += sourceBytesPerPixel;
                        iDestinationByte += destinationBytesPerPixel;
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static RgbByteImage ToRgbByteImage(Bitmap bitmap)
        {
            int rows = bitmap.Height;
            int columns = bitmap.Width;

            RgbByteImage output = new RgbByteImage(rows, columns);
            byte[] destinationData = output.Data;

            // Copy data from bitmap internals to the RGB byte image byte array.
            Rectangle rect = new Rectangle(0, 0, columns, rows);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int destinationBytesPerPixel = RgbColor.NumberOfRgbColorChannels;
            int sourceBytesPerPixel = bitmap.PixelFormat.BytesPerPixel();

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
                        // Bitmap order is B, G, R.
                        b = sourcePointer[iSourceByte + 0];
                        g = sourcePointer[iSourceByte + 1];
                        r = sourcePointer[iSourceByte + 2];

                        // Destination order ir R, G, B.
                        destinationData[iDestinationByte + 0] = r;
                        destinationData[iDestinationByte + 1] = g;
                        destinationData[iDestinationByte + 2] = b;

                        iSourceByte += sourceBytesPerPixel;
                        iDestinationByte += destinationBytesPerPixel;
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);

            return output;
        }

        public static Bitmap ToBitmap(RgbFloatImage rgbFloatImage)
        {
            int rows = rgbFloatImage.Rows;
            int columns = rgbFloatImage.Columns;
            float[] source = rgbFloatImage.Data;

            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
            Bitmap bitmap = new Bitmap(columns, rows, pixelFormat);

            Rectangle rect = new Rectangle(0, 0, columns, rows);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int sourceBytesPerPixel = RgbColor.NumberOfRgbColorChannels;
            int destinationBytesPerPixel = pixelFormat.BytesPerPixel();

            unsafe
            {
                int iSourceFloat = 0;
                int iDestinationByte = 0;
                byte r;
                byte g;
                byte b;
                float byteMax = 255.0f;
                byte* destinationPointer = (byte*)bitmapData.Scan0;
                for (int iRow = 0; iRow < rows; iRow++)
                {
                    iDestinationByte = iRow * bitmapData.Stride;
                    for (int iCol = 0; iCol < columns; iCol++)
                    {
                        // Order is R, G, B.
                        float rValue = source[iSourceFloat + 0];
                        float gValue = source[iSourceFloat + 1];
                        float bValue = source[iSourceFloat + 2];

                        if (rValue >= RgbFloatColor.MinChannelValue && rValue <= RgbFloatColor.MaxChannelValue)
                        {
                            r = Convert.ToByte(rValue * byteMax);
                        }
                        else
                        {
                            r = (byte)0;
                        }

                        if (gValue >= RgbFloatColor.MinChannelValue && gValue <= RgbFloatColor.MaxChannelValue)
                        {
                            g = Convert.ToByte(gValue * byteMax);
                        }
                        else
                        {
                            g = (byte)0;
                        }

                        if (bValue >= RgbFloatColor.MinChannelValue && bValue <= RgbFloatColor.MaxChannelValue)
                        {
                            b = Convert.ToByte(bValue * byteMax);
                        }
                        else
                        {
                            b = (byte)0;
                        }

                        // Order is B, G, R.
                        destinationPointer[iDestinationByte + 0] = b;
                        destinationPointer[iDestinationByte + 1] = g;
                        destinationPointer[iDestinationByte + 2] = r;

                        iSourceFloat += sourceBytesPerPixel;
                        iDestinationByte += destinationBytesPerPixel;
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static RgbFloatImage ToRgbFloatImage(Bitmap bitmap)
        {
            int rows = bitmap.Height;
            int columns = bitmap.Width;

            RgbFloatImage output = new RgbFloatImage(rows, columns);
            float[] destinationData = output.Data;

            // Copy data from bitmap internals to the RGB byte image byte array.
            Rectangle rect = new Rectangle(0, 0, columns, rows);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int destinationBytesPerPixel = RgbColor.NumberOfRgbColorChannels;
            int sourceBytesPerPixel = bitmap.PixelFormat.BytesPerPixel();

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
                        // Bitmap order is B, G, R.
                        b = sourcePointer[iSourceByte + 0];
                        g = sourcePointer[iSourceByte + 1];
                        r = sourcePointer[iSourceByte + 2];

                        // Destination order ir R, G, B.
                        destinationData[iDestinationByte + 0] = Convert.ToSingle(r) / RgbFloatColor.ByteMax;
                        destinationData[iDestinationByte + 1] = Convert.ToSingle(g) / RgbFloatColor.ByteMax;
                        destinationData[iDestinationByte + 2] = Convert.ToSingle(b) / RgbFloatColor.ByteMax;

                        iSourceByte += sourceBytesPerPixel;
                        iDestinationByte += destinationBytesPerPixel;
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);

            return output;
        }

        #endregion
    }
}
