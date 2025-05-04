using ProjectPOS.Models;
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
    public partial class ItemUser : UserControl
    {
        private UserModel _user = new UserModel();
        private string absolutePath = "";
        public event Action OnLoadClick;
        public ItemUser()
        {
            InitializeComponent();
            Init(_user);
        }
        public void Init(UserModel user)
        {
            if (user != null)
            {
                _user = user;
                txtName.Text = user.Name;
                txtPhone.Text = user.Phone;
                txtAddress.Text = user.Address;
                // Kiểm tra xem ImageURL có giá trị hợp lệ không
                if (!string.IsNullOrEmpty(user.ImageURL))
                {
                    absolutePath = Path.Combine(Application.StartupPath, user.ImageURL);
                    // Kiểm tra file có tồn tại không trước khi load
                    if (File.Exists(absolutePath))
                    {
                        ptbAvatar.Image = Image.FromFile(absolutePath);
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại: " + absolutePath);
                    }
                }
            }
        }

        private void ptbAvatar_Click(object sender, EventArgs e)
        {
            UpdateUser updateUser = new UpdateUser();
            updateUser.Init(_user);
            updateUser._OnLoadUser += () =>
            {
                OnLoadClick?.Invoke(); // // Gọi event khi cập nhật xong
            };
            updateUser.ShowDialog();

        }
    }
}
