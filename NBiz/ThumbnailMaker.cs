using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
namespace NBiz
{
    public class ThumbnailMaker
    {
        
        public static string Make(string virtualDirectory, string originalName,int width,int height,ThumbnailType type)
        {
            string thumbnailName = Path.GetFileNameWithoutExtension(originalName) + "_" + width + "-" + height + Path.GetExtension(originalName);
            string thumbnailPath = virtualDirectory +"/thumbnails/"+ thumbnailName;
            if (!File.Exists(thumbnailPath))
            {
                MakeThumbnail(virtualDirectory + originalName, thumbnailPath, type, width, height);
            }
            return thumbnailName;

        }


        private static void MakeThumbnail(string originalImagePath,string thumbnailPath, ThumbnailType tt, int width, int height)
        {
            
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (tt)
            {
                case ThumbnailType.ScalingByBoth://指定高宽缩放（可能变形）                 
                    break;
                case ThumbnailType.GeometricScalingByWidth://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailType.GeometricScalingByHeight://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                default://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;

            }
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); 
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            } 

        }

        private Bitmap DrawThumbnail(Image photo, int resizeToWidth, int resizeToHeight)
        {

            Bitmap bmp = new Bitmap(resizeToWidth, resizeToHeight);
            Graphics graphic = Graphics.FromImage(bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.DrawImage(photo, 0, 0, resizeToWidth, resizeToHeight);
            return bmp;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ThumbnailType
    {
        //按照给定的 长 或者 宽等比缩放
        GeometricScalingByWidth,
        GeometricScalingByHeight,
        ScalingByBoth

    }
}
