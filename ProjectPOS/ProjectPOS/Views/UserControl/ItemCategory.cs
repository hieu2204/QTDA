using ProjectPOS.Models;
using ProjectPOS.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS
{
    public partial class ItemCategory : UserControl
    {
        CategoryModel _category = new CategoryModel();
        public event Action OnLoadClick;
        public ItemCategory()
        {
            InitializeComponent();
        }
        public void Init(CategoryModel category)
        {
            _category = category;
            if(category != null )
            {
                txtID.Text = category.id.ToString();
                txtName.Text = category.name;
                txtDescription.Text = category.description;
            }
        }

        private void ptbUpdate_Click(object sender, EventArgs e)
        {
            UpdateCategory updateCategory = new UpdateCategory();
            updateCategory.Init(_category);
            updateCategory._OnLoadCate += () =>
            {
                OnLoadClick?.Invoke(); // // Gọi event khi cập nhật xong
            };
            updateCategory.ShowDialog();
        }
    }
}
