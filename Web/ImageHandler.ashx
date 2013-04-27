<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
/// <summary>
/// 根据需要制作/显示需要的图片
/// </summary>
public class ImageHandler : IHttpHandler {

   // NBiz.ThumbnailMaker thumbnailMaker = new NBiz.ThumbnailMaker();
    public void ProcessRequest (HttpContext context) {
        string imageName =context.Server.UrlDecode(context.Request["imagename"]);
        string paramWidth = context.Request["width"];
        string paramHeight = context.Request["height"];
        string paramType = context.Request["scalingType"];
      
        string physicalPath = context.Server.MapPath("/ProductImages/");
        int width = Convert.ToInt32(paramWidth);
        int height = Convert.ToInt32(paramHeight);
        NBiz.ThumbnailType tt = NBiz.ThumbnailType.GeometricScalingByWidth;// (NBiz.ThumbnailType)Enum.Parse(typeof(NBiz.ThumbnailType), paramType);
       string thumbnailName = NBiz.ThumbnailMaker.Make(physicalPath + "\\original\\", physicalPath + "\\thumbnail\\", imageName, width, height, tt);

        context.Response.ContentType = "image/png";
       // context.Response.TransmitFile(physicalPath + imageName);
        context.Response.WriteFile(thumbnailName);
      //  context.Response.WriteFile(@"E:\workspace\code\resources\VirtualDirectory\NTSBase\ProductImages\1080271.JPG");
    }
 
    
     
    public bool IsReusable {
        get {
            return false;
        }
    }

}