using ProjectPOS.Models;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Controllers
{
    public class ProductController
    {
        public void InsertProduct(string name, int category, double price, string description,string imageurl)
        {
            IProductService.InsertProduct(name, category, price, description, imageurl);
        }
        public List<ProductDTO> GetProducts(int number,int size)
        {
            return IProductService.GetListProduct(number, size);
        }
        public int GetTotalPage(int size)
        {
            return IProductService.GetTotalPageProduct(size);
        }
        public void UpdateProduct(string name, int categoryid, double price, string description, string imageURL, int productid)
        {
            IProductService.UpdateProduct(name, categoryid, price, description, imageURL, productid);
        }
        public List<ProductDTO> GetListProduct()
        {
            return IProductService.GetListProduct();
        }
        public virtual int GetToTalQuantity(int productID)
        {
            return IProductService.GetTotalQuantity(productID);
        }
        public List<ProductDTO> SearchProduct(string keyword)
        {
            return IProductService.SearchProduct(keyword);
        }

    }
}
