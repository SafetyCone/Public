using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;


namespace Public.Common.Lib.Visuals.MSWindows
{
    // Adapted from here: https://www.codeproject.com/Tips/240428/Work-with-bitmap-faster-with-Csharp
    // And here: http://csharpexamples.com/fast-image-processing-c/

    /// <summary>
    /// A class used for fast (much faster than Bitmap.GetPixel() and Bitmap.SetPixel()) bitmap reading and writing.
    /// </summary>
    /// <remarks>
    /// An unsafe context is used for byte transfer instead of a marshall. This was done for speed, but requires compilation with the unsafe option.
    /// </remarks>
    public class BitmapLocker : IDisposable, IEnumerable<Tuple<int, int>>
    {
        public const int ArgbBitDepth = 32;
        public const int RgbBitDepth = 24;
        public const int C256BitDepth = 8;


        #region Static

        public static int GetColorDepth(PixelFormat pixelFormat)
        {
            int output = Bitmap.GetPixelFormatSize(pixelFormat);
            if (!(BitmapLocker.ArgbBitDepth == output || BitmapLocker.RgbBitDepth == output || BitmapLocker.C256BitDepth == output))
            {
                string message = String.Format(@"Unsupported format {0} - {1} bits.", pixelFormat, output);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static int GetColorDepth(Bitmap bitmap)
        {
            int output = BitmapLocker.GetColorDepth(bitmap.PixelFormat);
            return output;
        }

        public static PixelFormat GetPixelFormat(int colorDepth)
        {
            PixelFormat output;
            switch (colorDepth)
            {
                case BitmapLocker.ArgbBitDepth:
                    output = PixelFormat.Format32bppArgb;
                    break;

                case BitmapLocker.C256BitDepth:
                    output = PixelFormat.Format8bppIndexed;
                    break;

                case BitmapLocker.RgbBitDepth:
                    output = PixelFormat.Format24bppRgb;
                    break;

                default:
                    throw new ArgumentException(String.Format(@"Unknown color depth: {0}.", colorDepth));
            }

            return output;
        }

        #endregion

        #region IDisposable Members

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
                    // Call Dispose() on any contained managed disposable resources here.
                }

                // Clean-up unmanaged resources here.
                this.UnlockBits();
            }

            this.zDisposed = true;
        }

        ~BitmapLocker()
        {
            this.CleanUp(false);
        }

        #endregion


        private Bitmap zBitmap;
        private BitmapData zBitmapData;
        public byte[] PixelBytes { get; private set; }
        public int ColorDepth { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public bool Locked { get; private set; }
        public Color this[int row, int column]
        {
            get
            {
                Color output = this.GetColor(row, column);
                return output;
            }
            set
            {
                this.SetColor(row, column, value);
            }
        }


        public BitmapLocker(string imageFilePath) : this(new Bitmap(imageFilePath), true) { }

        public BitmapLocker(Bitmap bitmap, bool lockBits)
        {
            this.SetBitmap(bitmap);

            if (lockBits)
            {
                this.LockBits();
            }
        }

        public BitmapLocker(Bitmap bitmap) : this(bitmap, true) { }

        public BitmapLocker(int rows, int columns, int colorDepth)
        {
            PixelFormat pixelFormat = BitmapLocker.GetPixelFormat(colorDepth);

            Bitmap bitmap = new Bitmap(columns, rows, pixelFormat);
            this.SetBitmap(bitmap);

            this.LockBits();
        }

        public BitmapLocker(int rows, int columns) : this(rows, columns, BitmapLocker.RgbBitDepth) { }

        public BitmapLocker(int rows, int columns, PixelFormat pixelFormat) : this(rows, columns, BitmapLocker.GetColorDepth(pixelFormat)) { }

        private void SetBitmap(Bitmap bitmap)
        {
            this.zBitmap = bitmap;
            this.ColorDepth = BitmapLocker.GetColorDepth(this.zBitmap);
            this.Rows = this.zBitmap.Height;
            this.Columns = this.zBitmap.Width;
        }

        public void LockBits()
        {
            if (this.Locked)
            {
                return;
            }

            Rectangle rect = new Rectangle(0, 0, this.Columns, this.Rows);
            this.zBitmapData = this.zBitmap.LockBits(rect, ImageLockMode.ReadWrite, this.zBitmap.PixelFormat);

            int byteCount = this.Rows * this.zBitmapData.Stride; // Stride is different than columns * bytes per pixel.
            this.PixelBytes = new byte[byteCount];

            unsafe
            {
                byte* firstPixelPointer = (byte*)this.zBitmapData.Scan0;
                for (int iByte = 0; iByte < byteCount; iByte++)
                {
                    this.PixelBytes[iByte] = firstPixelPointer[iByte];
                }
            }

            this.Locked = true;
        }

        public void UnlockBits()
        {
            if (!this.Locked)
            {
                return;
            }

            int numBytes = this.PixelBytes.Length;

            unsafe
            {
                byte* firstPixelPointer = (byte*)this.zBitmapData.Scan0;
                for (int iByte = 0; iByte < numBytes; iByte++)
                {
                    firstPixelPointer[iByte] = this.PixelBytes[iByte];
                }
            }

            this.zBitmap.UnlockBits(this.zBitmapData);

            this.Locked = false;
        }

        public Color GetColor(int row, int column)
        {
            int bytesPerPixel = this.ColorDepth / 8;

            int iByte = row * this.zBitmapData.Stride + column * bytesPerPixel;
            if (iByte > this.PixelBytes.Length - bytesPerPixel)
            {
                throw new IndexOutOfRangeException();
            }

            Color color;
            switch (this.ColorDepth)
            {
                case BitmapLocker.ArgbBitDepth:
                    {
                        byte b = this.PixelBytes[iByte];
                        byte g = this.PixelBytes[iByte + 1];
                        byte r = this.PixelBytes[iByte + 2];
                        byte a = this.PixelBytes[iByte + 3];
                        color = Color.FromArgb(a, r, g, b);
                    }
                    break;

                case BitmapLocker.C256BitDepth:
                    {
                        byte c = this.PixelBytes[iByte];
                        color = Color.FromArgb(c, c, c);
                    }
                    break;

                case BitmapLocker.RgbBitDepth:
                    {
                        byte b = this.PixelBytes[iByte];
                        byte g = this.PixelBytes[iByte + 1];
                        byte r = this.PixelBytes[iByte + 2];
                        color = Color.FromArgb(r, g, b);
                    }
                    break;

                default:
                    throw new ArgumentException(String.Format(@"Unknown color depth: {0}.", this.ColorDepth));
            }

            return color;
        }

        public void SetColor(int row, int column, Color color)
        {
            int bytesPerPixel = this.ColorDepth / 8;

            int iByte = row * this.zBitmapData.Stride + column * bytesPerPixel;
            if (iByte > this.PixelBytes.Length - bytesPerPixel)
            {
                throw new IndexOutOfRangeException();
            }

            switch (this.ColorDepth)
            {
                case BitmapLocker.ArgbBitDepth:
                    this.PixelBytes[iByte] = color.B;
                    this.PixelBytes[iByte + 1] = color.G;
                    this.PixelBytes[iByte + 2] = color.R;
                    this.PixelBytes[iByte + 3] = color.A;
                    break;

                case BitmapLocker.C256BitDepth:
                    this.PixelBytes[iByte] = color.B;
                    break;

                case BitmapLocker.RgbBitDepth:
                    this.PixelBytes[iByte] = color.B;
                    this.PixelBytes[iByte + 1] = color.G;
                    this.PixelBytes[iByte + 2] = color.R;
                    break;

                default:
                    throw new ArgumentException(String.Format(@"Unknown color depth: {0}.", this.ColorDepth));
            }
        }

        public Bitmap GetBitmap()
        {
            this.UnlockBits();

            return this.zBitmap;
        }

        public IEnumerator<Tuple<int, int>> GetEnumerator()
        {
            for (int iRow = 0; iRow < this.Rows; iRow++)
            {
                for (int iCol = 0; iCol < this.Columns; iCol++)
                {
                    yield return new Tuple<int, int>(iRow, iCol);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
