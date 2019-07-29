using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;

namespace Utility
{
    public static class ImageUtils
    {
        const int ALPHA_255 = 255 << 24;

        public static Bitmap CreateIndexedPng(this Bitmap orig)
        {
            var rgbValues = orig.ToByteArray();
            var block = orig.DetermineBlockSize();
            var colorSet = CreateColorSet(rgbValues, block, orig.Width, orig.Height);

            var result = new Bitmap(orig.Width, orig.Height, determinePixelFormat(colorSet.Count));

            // setup palette and color map
            var pal = result.Palette;
            var colorToIndex = new Dictionary<int, int>();
            int index = 0;
            foreach (int color in colorSet)
            {
                pal.Entries[index] = Color.FromArgb(color);
                colorToIndex[color] = index;
                index++;
            }
            result.Palette = pal;

            // fill bits
            var data = result.LockBits(new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.ReadWrite, result.PixelFormat);

            byte[] colorData = new byte[data.Stride * data.Height];
            int len = rgbValues.Length;
            if (block == 3)
            {
                if (result.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    fillBitmapData8bppIndexedNoAlpha(rgbValues, colorToIndex, colorData, len, orig.Width, orig.Height, data.Stride);
                }
                else if(result.PixelFormat == PixelFormat.Format4bppIndexed)
                {
                    fillBitmapData4bppIndexedNoAlpha(rgbValues, colorToIndex, data, colorData, len, orig.Width);
                }
                else if (result.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    throw new NotImplementedException(PixelFormat.Format1bppIndexed + " is not supported");
                }
            }
            else
            {
                if (result.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    fillBitmapData8bppIndexedAlpha(rgbValues, colorToIndex, colorData, len, orig.Width, orig.Height, data.Stride);
                }
                else if (result.PixelFormat == PixelFormat.Format4bppIndexed)
                {
                    fillBitmapData8bppIndexedAlpha(rgbValues, colorToIndex, colorData, len, orig.Width, orig.Height, data.Stride);
                }
                else if (result.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    throw new NotImplementedException(PixelFormat.Format1bppIndexed + " is not supported");
                }
            }

            Marshal.Copy(colorData, 0, data.Scan0, colorData.Length);

            result.UnlockBits(data);

            return result;
        }

        private static void fillBitmapData8bppIndexedAlpha(byte[] rgbValues, Dictionary<int, int> colorToIndex, 
                                                              byte[] colorData, int len, int width, int height, int outStride)
        {
            int stride = len / height;
            int colLimit = width * 4;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < colLimit; col += 4)
                {
                    int offset = row * stride + col;
                    int b = rgbValues[offset];
                    int g = rgbValues[offset + 1];
                    int r = rgbValues[offset + 2];
                    int a = rgbValues[offset + 3];
                    int index = row * outStride + col / 4;
                    colorData[index] = (byte)colorToIndex[to32BitIntARGB(a, r, g, b)];
                }
            }
        }

        private static void fillBitmapData4bppIndexedAlpha(byte[] rgbValues, Dictionary<int, int> colorToIndex, 
                                                                      BitmapData data, byte[] colorData, int len, int width)
        {
            int outStride = data.Stride;
            int height = data.Height;
            int stride = len / height;
            int colLimit = width * 4;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < colLimit; col += 8)
                {
                    int offset = row * stride + col;
                    int b = rgbValues[offset];
                    int g = rgbValues[offset + 1];
                    int r = rgbValues[offset + 2];
                    int a = rgbValues[offset + 3];
                    int color1 = to32BitIntARGB(r, g, b);
                    if (col / 4 + 1 >= width)
                    {
                        int index1 = row * outStride + col / 8;
                        colorData[index1] = (byte)(colorToIndex[color1] << 4);
                        continue;
                    }

                    b = rgbValues[offset + 4];
                    g = rgbValues[offset + 5];
                    r = rgbValues[offset + 6];
                    a = rgbValues[offset + 7];
                    int color2 = to32BitIntARGB(r, g, b);
                    int index2 = row * outStride + col / 8;
                    colorData[index2] = (byte)(colorToIndex[color1] << 4 | colorToIndex[color2]);
               }
            }
        }

        private static void fillBitmapData8bppIndexedNoAlpha(byte[] rgbValues, Dictionary<int, int> colorToIndex, 
                                                                byte[] colorData, int len, int width, int height, int outStride)
        {
            int stride = len / height;
            int colLimit = width * 3;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < colLimit; col += 3)
                {
                    int offset = row * stride + col;
                    int b = rgbValues[offset];
                    int g = rgbValues[offset + 1];
                    int r = rgbValues[offset + 2];
                    int index = row * outStride + col / 3;
                    colorData[index] = (byte)colorToIndex[to32BitIntARGB(r, g, b)];
                }
            }
        }

        private static void fillBitmapData4bppIndexedNoAlpha(byte[] rgbValues, Dictionary<int, int> colorToIndex, 
                                                                          BitmapData data, byte[] colorData, int len, int width)
        {
            int outStride = data.Stride;
            int height = data.Height;
            int stride = len / height;
            int colLimit = width * 3;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < colLimit; col += 6)
                {
                    int offset = row * stride + col;
                    int b = rgbValues[offset];
                    int g = rgbValues[offset + 1];
                    int r = rgbValues[offset + 2];
                    int color1 = to32BitIntARGB(r, g, b);
                    if (col / 3 + 1 >= width)
                    {
                        int index1 = row * outStride + col / 6;
                        colorData[index1] = (byte)(colorToIndex[color1] << 4);
                        continue;
                    }

                    b = rgbValues[offset + 3];
                    g = rgbValues[offset + 4];
                    r = rgbValues[offset + 5];
                    int color2 = to32BitIntARGB(r, g, b);
                    int index2 = row * outStride + col / 6;
                    colorData[index2] = (byte)(colorToIndex[color1] << 4 | colorToIndex[color2]);

                }
            }
        }

        private static HashSet<int> CreateColorSet(byte[] rgbValues, int block, int width, int height)
        {
            var colorSet = new HashSet<int>();
            int len = rgbValues.Length;
            int stride = len / height;
            if (block == 3)
            {
                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width * 3; col += 3)
                    {
                        int offset = row * stride + col;
                        int b = rgbValues[offset];
                        int g = rgbValues[offset + 1];
                        int r = rgbValues[offset + 2];
                        colorSet.Add(to32BitIntARGB(r,g,b));
                    }
                }

            }
            else
            {
                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width * 4; col += 4)
                    {
                        int offset = row * stride + col;
                        int b = rgbValues[offset];
                        int g = rgbValues[offset + 1];
                        int r = rgbValues[offset + 2];
                        int a = rgbValues[offset + 3];
                        colorSet.Add(to32BitIntARGB(a, r, g, b));
                    }
                }
            }
            return colorSet;
        }

        public static PixelFormat determinePixelFormat(int colorCount)
        {
            if (colorCount <= 2)
            {
                // you can use Format1bppIndexed but for simplicity return Format4bppIndexed
                return PixelFormat.Format4bppIndexed;
            }
            else if (colorCount <= 16)
            {
                return PixelFormat.Format4bppIndexed;
            }
            else if (colorCount <= 255)
            {
                return PixelFormat.Format8bppIndexed;
            }
            else
            {
                throw new ArgumentException("number of colors in image must equal or less than " + 256);
            }
        }

        public static int DetermineBlockSize(this Bitmap bmp)
        {
            var pixelFormat = bmp.PixelFormat;
            if (pixelFormat.HasFlag(PixelFormat.Format24bppRgb))
            {
                return 3;
            }
            else if (pixelFormat.HasFlag(PixelFormat.Format32bppArgb))
            {
                return 4;
            }
            else
            {
                throw new NotImplementedException("Unsupported type of image:" + pixelFormat);
            }
        }

        private static int to32BitIntARGB(int a, int r, int g, int b)
        {
            return (a << 24) | (r << 16) | (g << 8) | (b);
        }

        private static int to32BitIntARGB(int r, int g, int b)
        {
            return ALPHA_255 | (r << 16) | (g << 8) | (b);
        }

        private static byte[] ToByteArray(this Bitmap src)
        {
            var rect = new Rectangle(0, 0, src.Width, src.Height);
            var bmpData = src.LockBits(rect, ImageLockMode.ReadOnly, src.PixelFormat);

            var ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * src.Height;
            var rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            src.UnlockBits(bmpData);
            return rgbValues;
        }
    }
}