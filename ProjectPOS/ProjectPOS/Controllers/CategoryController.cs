using ProjectPOS.Models;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Controllers
{
    public class CategoryController
    {
        public List<CategoryModel> GetAllCategory(int pagenumber, int pagesize)
        {
            return ICategoryService.GetListCategory(pagenumber, pagesize);
        }
        public List<CategoryModel> GetAllCategoryName()
        {
            return ICategoryService.GetListCategoryName();
        }
        public int GetTotalPageCategory(int pagesize)
        {
            return ICategoryService.GetTotalPageCategory(pagesize);
        }
        public void UpdateCategory(int ID, string Name, string Description)
        {
            ICategoryService.UpdateCategory(ID, Name, Description);
        }
        public void InsertCategory(string name, string description)
        {
            ICategoryService.InsertCategory(name, description);
        }
        public List<CategoryModel> SearchCategory(string keyword)
        {
            return ICategoryService.SearchCategoryByName(keyword);
        }

    }
}
