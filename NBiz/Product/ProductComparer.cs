using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
namespace NBiz
{
    public class ProductComparer:IEqualityComparer<Product>
    {

        public bool Equals(Product x, Product y)
        {
            return x.SupplierCode == y.SupplierCode && x.ModelNumber == y.ModelNumber;
        }

        public int GetHashCode(Product product)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null. 
            int hashProductName = product.Name == null ? 0 : product.Name.GetHashCode();

            //Get hash code for the Code field. 
            int hashProductCode = product.ModelNumber.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName ^ hashProductCode;
        }
    }
}
