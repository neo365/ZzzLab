using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ZzzLab.Drawing
{
    public static class BitmapExtension
    {
        /// <summary>
        /// 두개의 이미지를 비교한다. 알파값까지 모두 비교하므로 주의
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="targetStream"></param>
        /// <param name="maskColor">다른 부분을 표시해줄 색. Color.Empty로 지정시 대상의 색을 그대로 가져온다.</param>
        /// <param name="overWrite">원본위에 표시한다. false로 지정되면 다른 부분만 표시한다.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Bitmap ImageDiff(Stream sourceStream, Stream targetStream, Color? maskColor = null, bool overWrite = false)
        {
            if (sourceStream == null) throw new ArgumentNullException(nameof(sourceStream));
            if (targetStream == null) throw new ArgumentNullException(nameof(targetStream));

            using (Bitmap source = (Bitmap)Image.FromStream(sourceStream))
            {
                using (Bitmap target = (Bitmap)Image.FromStream(targetStream))
                {
                    return ImageDiff(source, target, maskColor, overWrite);
                }
            }
        }

        /// <summary>
        /// 두개의 이미지를 비교한다. 알파값까지 모두 비교하므로 주의
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="maskColor">다른 부분을 표시해줄 색. Color.Empty로 지정시 대상의 색을 그대로 가져온다.</param>
        /// <param name="overWrite">원본위에 표시한다. false로 지정되면 다른 부분만 표시한다.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static Bitmap ImageDiff(string sourcePath, string targetPath, Color? maskColor = null, bool overWrite = false)
        {
            if (string.IsNullOrWhiteSpace(sourcePath)) throw new ArgumentNullException(nameof(sourcePath));
            if (string.IsNullOrWhiteSpace(targetPath)) throw new ArgumentNullException(nameof(targetPath));
            if (File.Exists(sourcePath) == false) throw new FileNotFoundException("File Not Found.", sourcePath);
            if (File.Exists(targetPath) == false) throw new FileNotFoundException("File Not Found.", targetPath);

            using (Bitmap source = (Bitmap)Image.FromFile(sourcePath))
            {
                using (Bitmap target = (Bitmap)Image.FromFile(targetPath))
                {
                    return ImageDiff(source, target, maskColor, overWrite);
                }
            }
        }

        /// <summary>
        /// 두개의 이미지를 비교한다. 알파값까지 모두 비교하므로 주의
        /// </summary>
        /// <param name="source">원본</param>
        /// <param name="target">대상</param>
        /// <param name="maskColor">다른 부분을 표시해줄 색. Color.Empty로 지정시 대상의 색을 그대로 가져온다.</param>
        /// <param name="overWrite">원본위에 표시한다. false로 지정되면 다른 부분만 표시한다.</param>
        /// <returns></returns>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Bitmap ImageDiff(this Bitmap source, Bitmap target, Color? maskColor = null, bool overWrite = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));

            int diff = 0;

            int maxWidth = Math.Max(source.Width, target.Width);
            int maxHeight = Math.Max(source.Height, target.Height);
            int minWidth = Math.Min(source.Width, target.Width);
            int minHeight = Math.Min(source.Height, target.Height);

            Color mask = maskColor ?? Color.Empty;

            using (LockBitmap sourceImage = LockBitmap.Create(source))
            {
                using (LockBitmap targetImage = LockBitmap.Create(target))
                {
                    using (LockBitmap compareImage = LockBitmap.Create(new Bitmap(maxWidth, maxHeight, PixelFormat.Format32bppArgb)))
                    {
                        sourceImage.LockBits(ImageLockMode.ReadWrite);
                        targetImage.LockBits(ImageLockMode.ReadWrite);
                        compareImage.LockBits(ImageLockMode.ReadWrite);

                        Parallel.For(0, maxHeight, y =>
                        {
                            Parallel.For(0, maxWidth, x =>
                            {
                                Color color;
                                if (x < sourceImage.Width && y < sourceImage.Height)
                                {
                                    color = sourceImage.GetPixel(x, y);
                                    if (color.A == 0) color = Color.Transparent;
                                }
                                else color = Color.Transparent;

                                if (x < targetImage.Width && y < targetImage.Height)
                                {
                                    Color tc = targetImage.GetPixel(x, y);
                                    if (tc.A == 0) tc = Color.Transparent;

                                    if (color.R == tc.R
                                        && color.G == tc.G
                                        && color.B == tc.B
                                        && color.A == tc.A)
                                    {
                                        if (overWrite) compareImage.SetPixel(x, y, color);
                                    }
                                    else
                                    {
                                        diff++;
                                        compareImage.SetPixel(x, y, (mask != Color.Empty ? mask : tc));
                                    }
                                }
                                else
                                {
                                    if (overWrite) compareImage.SetPixel(x, y, color);
                                }
                            });
                        });

                        sourceImage.UnlockBits();
                        targetImage.UnlockBits();
                        compareImage.UnlockBits();

                        return compareImage.GetBitmap();
                    }
                }
            }
        }

        public static Bitmap ImageMerge(this Bitmap source, params Bitmap[] images)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (images == null || images.Length == 0) throw new ArgumentNullException(nameof(images));

            int width = source.Width;
            int height = source.Height;

            foreach (Bitmap bitmap in images)
            {
                width = Math.Max(bitmap.Width, width);
                height = Math.Max(bitmap.Height, height);
            }

            using (Bitmap dst = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(dst))
                {
                    g.DrawImage(source, 0, 0);

                    foreach (Bitmap bitmap in images)
                    {
                        g.DrawImage(bitmap, 0, 0);
                    }

                    return (Bitmap)dst.Clone();
                }
            }
        }
    }
}