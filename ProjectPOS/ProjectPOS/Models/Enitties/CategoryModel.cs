using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Models
{
    public class CategoryModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public CategoryModel() { }
        public CategoryModel(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
    }
}
