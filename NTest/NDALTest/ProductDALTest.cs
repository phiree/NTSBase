using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDAL;
namespace NTest.NDALTest
{
   public class ProductDALTest
    {
       public void QueryWithPhoto()
       {
           DalBase<NModel.Product> dalProduct = new DalBase<NModel.Product>();
           string query = "select p from Product p where p.ProductImageUrls.size>0";
           var pl= dalProduct.GetList(query);
           Console.WriteLine(pl.Count);

           
       }
    }
}
