using ProjectPOS.Models;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Utilities;
using ProjectPOS.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS
{
    public partial class ItemProduct : UserControl
    {
        private ProductDTO _product = new ProductDTO();
        private string absolutePath = string.Empty;
        public event Action OnLoadClick;
        public ItemProduct()
        {
            InitializeComponent();
        }
        public void Init(ProductDTO product)
        {
            if(product != null)
            {
                _product = product;
                txtName.Text = product.ProductName;
                txtPrice.Text = product.Price.ToString();
                FormatNumber.FormatAsNumber(txtPrice);
                if (!string.IsNullOrEmpty(product.ImageURL))
                {
                    absolutePath = Path.Combine(Application.StartupPath, product.ImageURL);
                    // Kiểm tra file có tồn tại không trước khi load
                    if (File.Exists(absolutePath))
                    {
                        ptbImageProduct.Image = Image.FromFile(absolutePath);
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại: " + absolutePath);
                    }
                }
            }
        }

        private void ptbImageProduct_Click(object sender, EventArgs e)
        {
            UpdateProduct updateProduct = new UpdateProduct();
            updateProduct.Init(_product);
            updateProduct._OnLoadProduct += () =>
            {
                OnLoadClick?.Invoke();
            };
            updateProduct.ShowDialog();
            
        }
    }
}
