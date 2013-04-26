using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBiz
{
    /// <summary>
    /// 根据不同的文件夹结构提取 图片信息
    /// </summary>
   public interface IImageExtractRule
    {
       IList<ImageInfo> ExtractImageInfo(string supplierFolderName);
    }
}
