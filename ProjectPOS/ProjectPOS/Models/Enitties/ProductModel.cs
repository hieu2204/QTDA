using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public ProductModel() { }
        public ProductModel(int id, string name, int categoryName, int supplierName, double price, int stockQuantity, string description, string imageURL, DateTime creatAt, DateTime updateAt)
        {
            ProductID = id;
            Name = name;
            CategoryID = categoryName;
            SupplierID = supplierName;
            UnitPrice = price;
            Quantity = stockQuantity;
            Description = description;
            ImageURL = imageURL;
            CreateAt = creatAt;
            UpdateAt = updateAt;
        }
    }
}
