// FileInfo
// File:"ImageHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Image
// 1.Watermark(Image oriImage,Bitmap water,int offsetX,int offsetY)
// 2.Watermark(Image oriImage, string waterString, Font font,Color fontColor,int offsetX, int offsetY)
// 3.ChangeImageLight(Bitmap image, int val)
// 4.InvertedImageColor(Bitmap image)
// 5.Emboss(Bitmap image, int Width, int Height)
// 6.CaptureImage(Image image, int offsetX, int offsetY, int width, int height)
// 7.ResizeImage(Bitmap bmp, int width, int height)
// 8.FilterImage(Bitmap image, Color pixel)
// 9.RotateImage(ref Bitmap image,RotateFlipType rotateType)
// 10.GetEncoderInfo(String mimeType)
// 11.Compress(Bitmap srcBitmap, Stream destStream, long level)
// 12.Compress(Bitmap srcBitMap, string destFile, long level)
// 13.ConvertToColerlessImage(Bitmap image)
// 14.GetFrames(string path, string savedPath)
//
// File Lines:228
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Jund.NETHelper.MultiMediaHelper
{
    public class ImageHelper
    {
        public static Image Watermark(Image oriImage,Bitmap water,int offsetX,int offsetY)
        {
            System.Drawing.Image img = new System.Drawing.Bitmap(oriImage.Width, oriImage.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);

            g.Clear(Color.Transparent);

            Bitmap waterImage = ConvertToColerlessImage(water);

            g.DrawImage(oriImage, 0, 0, oriImage.Width, oriImage.Height);
            g.DrawImage(waterImage, offsetX, offsetY, water.Width, water.Height);

            return img;
        }
        public static Image Watermark(Image oriImage, string waterString, Font font,Color fontColor,int offsetX, int offsetY)
        {
            Image img = new Bitmap(oriImage.Width, oriImage.Height);
            Graphics g = System.Drawing.Graphics.FromImage(img);

            g.Clear(Color.Transparent);
            Brush brush = new SolidBrush(fontColor);

            g.DrawImage(oriImage, 0, 0, oriImage.Width, oriImage.Height);
            g.DrawString(waterString, font,brush,offsetX, offsetY);

            return img;
        }
        public Bitmap ChangeImageLight(Bitmap image, int val)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);//获取当前坐标的像素值
                    newImage.SetPixel(x, y, Color.FromArgb(pixel.R+val, pixel.G+val, pixel.B+val));//绘图
                }
            }
            return newImage;
        }
        public Bitmap InvertedImageColor(Bitmap image)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);//获取当前坐标的像素值
                    newImage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));//绘图
                }
            }
            return newImage;
        }
        public Bitmap Emboss(Bitmap image, int Width, int Height)
        {
            Bitmap newImage = new Bitmap(Width, Height);

            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    Color color1 = image.GetPixel(x, y);
                    Color color2 = image.GetPixel(x + 1, y + 1);
                    int r = Math.Abs(color1.R - color2.R + 128);
                    int g = Math.Abs(color1.G - color2.G + 128);
                    int b = Math.Abs(color1.B - color2.B + 128);
                  
                    newImage.SetPixel(x, y, Color.FromArgb(r>255?255:r, g>255?255:g, b>255?255:b));
                }
            }
            return newImage;
        }
        public static Image CaptureImage(Image image, int offsetX, int offsetY, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.DrawImage(image, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            Image newImage = Image.FromHbitmap(bitmap.GetHbitmap());

            graphic.Dispose();
            bitmap.Dispose();
            return newImage;
        }
        public static Bitmap ResizeImage(Bitmap bmp, int width, int height)
        {
            try
            {
                Bitmap image = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(image);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return image;
            }
            catch
            {
                return null;
            }
        }
        public Bitmap FilterImage(Bitmap image, Color pixel)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            int x, y;

            for (x = 0; x < image.Width; x++)
            {
                for (y = 0; y < image.Height; y++)
                {
                    pixel = image.GetPixel(x, y);//获取当前坐标的像素值
                    newImage.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//绘图
                }
            }
            return newImage;
        }
        public void RotateImage(ref Bitmap image,RotateFlipType rotateType)=> image.RotateFlip(rotateType);
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        public static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with 给定的 quality level
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }
        public static Bitmap ConvertToColerlessImage(Bitmap image)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            Color pixel;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    pixel = image.GetPixel(x, y);//获取当前坐标的像素值
                    int result = (pixel.R + pixel.G + pixel.B) / 3;//取红绿蓝三色的平均值
                    newImage.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return newImage;
        }
        public void GetFrames(string path, string savedPath)
        {
            Image gif = Image.FromFile(path);
            FrameDimension frame = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(frame); //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)
            for (int i = 0; i < count; i++)    //以Jpeg格式保存各帧
            {
                gif.SelectActiveFrame(frame, i);
                gif.Save(savedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
    }
}
