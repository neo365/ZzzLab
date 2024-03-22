using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ZzzLab.Diff.Image
{
    public class LockBitmap : IDisposable
    {
        public int Width { private set; get; } = 0;
        public int Height { private set; get; } = 0;
        public int Depth { set; get; } = 0;
        private PixelFormat PixelFormat { set; get; }

        private Bitmap _BitmapSource = new Bitmap(1, 1);

        private Bitmap BitmapSource
        {
            set
            {
                if (_BitmapSource == value) return;

                if (_BitmapSource != null)
                {
                    _BitmapSource.Dispose();
                    _BitmapSource = null;
                }

                if (value == null)
                {
                    Width = 0;
                    Height = 0;
                }
                else
                {
                    Width = value.Width;
                    Height = value.Height;
                    Depth = System.Drawing.Image.GetPixelFormatSize(value.PixelFormat);
                }

                _BitmapSource = value;
            }
            get
            {
                return _BitmapSource;
            }
        }

        private BitmapData _BitmapData = null;
        private byte[] _Pixels = null;
        private bool _IsLocked = false;

        private LockBitmap(Bitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            BitmapSource = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format32bppArgb);
            PixelFormat = bitmap.PixelFormat;
        }

        public static LockBitmap Create(Bitmap bitmap)
            => new LockBitmap(bitmap);

        /// <summary>
        /// 파일에서 이미지를 불러 온다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>LockBitmap</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static LockBitmap Create(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (File.Exists(filePath) == false) throw new FileNotFoundException();

            return new LockBitmap(System.Drawing.Image.FromFile(filePath) as Bitmap);
        }

        public static LockBitmap Create(int width, int height)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(width));

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                return new LockBitmap(bitmap);
            }
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits(ImageLockMode lockmode = ImageLockMode.ReadWrite)
        {
            int PixelCount = Width * Height;

            if (Depth != 8 && Depth != 24 && Depth != 32) throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");

            _BitmapData = BitmapSource.LockBits(new Rectangle(0, 0, Width, Height), lockmode, BitmapSource.PixelFormat);

            int step = Depth / 8;
            _Pixels = new byte[PixelCount * step];

            Marshal.Copy(_BitmapData.Scan0, _Pixels, 0, _Pixels.Length);

            _IsLocked = true;
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            if (_IsLocked == false) return;

            if (BitmapSource == null) return;
            if (_BitmapData == null) return;

            if (_Pixels != null)
            {
                Marshal.Copy(_Pixels, 0, _BitmapData.Scan0, _Pixels.Length);
                BitmapSource.UnlockBits(_BitmapData);
            }

            _Pixels = null;
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            if (_IsLocked == false) throw new InvalidOperationException();
            if (_Pixels == null || _Pixels.Any() == false) return Color.Empty;

            Color clr = Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            int i = (y * Width + x) * cCount;

            if (i > _Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = _Pixels[i];
                byte g = _Pixels[i + 1];
                byte r = _Pixels[i + 2];
                byte a = _Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (Depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = _Pixels[i];
                byte g = _Pixels[i + 1];
                byte r = _Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (Depth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = _Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }

            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            if (_Pixels == null || _Pixels.Any() == false) return;

            int cCount = Depth / 8;
            int i = (y * Width + x) * cCount;

            if (Depth == 32)
            {
                _Pixels[i] = color.B;
                _Pixels[i + 1] = color.G;
                _Pixels[i + 2] = color.R;
                _Pixels[i + 3] = color.A;
            }
            else if (Depth == 24)
            {
                _Pixels[i] = color.B;
                _Pixels[i + 1] = color.G;
                _Pixels[i + 2] = color.R;
            }
            else if (Depth == 8)
            {
                _Pixels[i] = color.B;
            }
        }

        public Bitmap GetBitmap()
            => BitmapSource.Clone(new Rectangle(0, 0, BitmapSource.Width, BitmapSource.Height), PixelFormat);

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_Pixels != null) _Pixels = null;

                    if (_BitmapData != null)
                    {
                        UnlockBits();
                        _BitmapData = null;
                    }

                    if (_BitmapSource != null)
                    {
                        UnlockBits();
                        _BitmapSource.Dispose();
                        _BitmapSource = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}