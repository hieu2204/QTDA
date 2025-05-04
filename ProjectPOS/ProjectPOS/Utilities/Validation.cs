using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using ProjectPOS.Servies;

namespace ProjectPOS.Utilities
{
    public class Validation
    {
        private readonly IUserServiceWrapper _userServiceWrapper;

        // Constructor nhận đối tượng IUserServiceWrapper
        public Validation(IUserServiceWrapper userServiceWrapper)
        {
            _userServiceWrapper = userServiceWrapper;
        }
        public static bool ValidationInfoCustomer(
    string name, string phone, string email, string address, string loyalty, int customerID,
    Guna2HtmlLabel lbName, Guna2HtmlLabel lbPhone, Guna2HtmlLabel lbEmail, Guna2HtmlLabel lbAddress, Guna2HtmlLabel lbLoyalty)
        {
            bool isValid = true;

            // Xóa lỗi trước đó
            lbName.Text = lbPhone.Text = lbEmail.Text = lbAddress.Text = lbLoyalty.Text = "";

            if (string.IsNullOrWhiteSpace(name))
            {
                lbName.Text = "Tên không được để trống!";
                lbName.ForeColor = Color.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                lbPhone.Text = "Số điện thoại không được để trống!";
                lbPhone.ForeColor = Color.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                lbEmail.Text = "Email không được để trống!";
                lbEmail.ForeColor = Color.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                lbAddress.Text = "Địa chỉ không được để trống!";
                lbAddress.ForeColor = Color.Red;
                isValid = false;
            }
            if (!int.TryParse(loyalty, out _))
            {
                lbLoyalty.Text = "Điểm tích lũy phải là số!";
                lbLoyalty.ForeColor = Color.Red;
                isValid = false;
            }

            // Kiểm tra email và số điện thoại có bị trùng không
            var (emailExists, phoneExists) = ICustomerService.IsCustomerEmailOrPhoneExists(email, phone, customerID);

            if (emailExists)
            {
                lbEmail.Text = "Email đã tồn tại!";
                lbEmail.ForeColor = Color.Red;
                isValid = false;
            }
            if (phoneExists)
            {
                lbPhone.Text = "Số điện thoại đã tồn tại!";
                lbPhone.ForeColor = Color.Red;
                isValid = false;
            }

            return isValid;
        }


        public static bool ValidationAddCustomer(
      string name, string phone, string email, string address,
      Guna2HtmlLabel lbName, Guna2HtmlLabel lbPhone, Guna2HtmlLabel lbEmail, Guna2HtmlLabel lbAddress)
        {
            bool isValid = true;

            lbName.Text = lbPhone.Text = lbEmail.Text = lbAddress.Text = "";

            if (string.IsNullOrWhiteSpace(name))
            {
                lbName.Text = "Tên không được để trống!";
                lbName.ForeColor = Color.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                lbPhone.Text = "Số điện thoại không được để trống!";
                lbPhone.ForeColor = Color.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                lbEmail.Text = "Email không được để trống!";
                lbEmail.ForeColor = Color.Red;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                lbAddress.Text = "Địa chỉ không được để trống!";
                lbAddress.ForeColor = Color.Red;
                isValid = false;
            }

            // Kiểm tra email và số điện thoại có bị trùng không
            var (emailExists, phoneExists) = ICustomerService.IsCustomerEmailOrPhoneExists(email, phone, 0);

            if (emailExists)
            {
                lbEmail.Text = "Email đã tồn tại!";
                lbEmail.ForeColor = Color.Red;
                isValid = false;
            }
            if (phoneExists)
            {
                lbPhone.Text = "Số điện thoại đã tồn tại!";
                lbPhone.ForeColor = Color.Red;
                isValid = false;
            }

            return isValid;
        }

        public bool ValidationAddUser(string user, string pass, string name, string email, string phone, string address, string salary,
      Guna2HtmlLabel lbCheckUser, Guna2HtmlLabel lbCheckPass, Guna2HtmlLabel lbCheckName,
      Guna2HtmlLabel lbCheckPhone, Guna2HtmlLabel lbCheckSalary, Guna2HtmlLabel lbCheckEmail, Guna2HtmlLabel lbCheckAddress)
        {
            // Xóa thông báo lỗi cũ
            lbCheckUser.Text = lbCheckPass.Text = lbCheckName.Text = lbCheckEmail.Text =
            lbCheckPhone.Text = lbCheckSalary.Text = lbCheckAddress.Text = string.Empty;

            // Kiểm tra username
            if (string.IsNullOrEmpty(user))
            {
                lbCheckUser.Text = "User không được để trống.";
                lbCheckUser.ForeColor = Color.Red;
                return false;
            }

            var usernamePattern = @"^[a-zA-Z0-9_]+$"; // Chỉ cho phép chữ cái, số và dấu gạch dưới
            if (!Regex.IsMatch(user, usernamePattern))
            {
                lbCheckUser.Text = "User không được chứa ký tự đặc biệt.";
                lbCheckUser.ForeColor = Color.Red;
                return false;
            }

            if (IUserService.IsUsernameExit(user))
            {
                lbCheckUser.Text = "User đã tồn tại.";
                lbCheckUser.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra mật khẩu
            if (string.IsNullOrEmpty(pass))
            {
                lbCheckPass.Text = "Password không được để trống.";
                lbCheckPass.ForeColor = Color.Red;
                return false;
            }

            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"; // Ít nhất 8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt
            if (!Regex.IsMatch(pass, passwordPattern))
            {
                lbCheckPass.Text = "Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt.";
                lbCheckPass.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra tên
            if (string.IsNullOrEmpty(name))
            {
                lbCheckName.Text = "Tên không được để trống.";
                lbCheckName.ForeColor = Color.Red;
                return false;
            }

            var namePattern = @"^[a-zA-ZÀ-ỹ\s]+$"; // Chỉ cho phép chữ cái và khoảng trắng
            if (!Regex.IsMatch(name, namePattern))
            {
                lbCheckName.Text = "Tên không được chứa ký tự đặc biệt hoặc số.";
                lbCheckName.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrEmpty(phone))
            {
                lbCheckPhone.Text = "Số điện thoại không được để trống.";
                lbCheckPhone.ForeColor = Color.Red;
                return false;
            }

            var phonePattern = @"^\+?[\d\s\-]{7,15}$";
            if (!Regex.IsMatch(phone, phonePattern))
            {
                lbCheckPhone.Text = "Số điện thoại không hợp lệ.";
                lbCheckPhone.ForeColor = Color.Red;
                return false;
            }

            if (IUserService.IsPhoneExists(phone))
            {
                lbCheckPhone.Text = "Số điện thoại đã tồn tại.";
                lbCheckPhone.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra lương
            if (!double.TryParse(salary, out double _salary) || _salary < 0)
            {
                lbCheckSalary.Text = "Lương phải là số nguyên dương.";
                lbCheckSalary.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra email
            if (string.IsNullOrEmpty(email))
            {
                lbCheckEmail.Text = "Email không được để trống.";
                lbCheckEmail.ForeColor = Color.Red;
                return false;
            }

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                lbCheckEmail.Text = "Email không hợp lệ.";
                lbCheckEmail.ForeColor = Color.Red;
                return false;
            }

            if (IUserService.IsEmailExists(email))
            {
                lbCheckEmail.Text = "Email đã tồn tại.";
                lbCheckEmail.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra địa chỉ
            if (string.IsNullOrEmpty(address))
            {
                lbCheckAddress.Text = "Địa chỉ không được để trống.";
                lbCheckAddress.ForeColor = Color.Red;
                return false;
            }

            return true;
        }


        public static bool ValidationUpdateUser(int userId, string pass, string name, string email, string phone, string address, string salary,
     Guna2HtmlLabel lbCheckPass, Guna2HtmlLabel lbCheckName, Guna2HtmlLabel lbCheckPhone,
     Guna2HtmlLabel lbCheckSalary, Guna2HtmlLabel lbCheckEmail, Guna2HtmlLabel lbCheckAddress)
        {
            lbCheckPass.Text = lbCheckName.Text = lbCheckEmail.Text = lbCheckPhone.Text = lbCheckSalary.Text = lbCheckAddress.Text = String.Empty;

            if (string.IsNullOrEmpty(pass))
            {
                lbCheckPass.Text = "Password không được để trống.";
                lbCheckPass.ForeColor = Color.Red;
                return false;
            }

            if (string.IsNullOrEmpty(name))
            {
                lbCheckName.Text = "Tên không được để trống.";
                lbCheckName.ForeColor = Color.Red;
                return false;
            }

            if (string.IsNullOrEmpty(phone))
            {
                lbCheckPhone.Text = "Số điện thoại không được để trống.";
                lbCheckPhone.ForeColor = Color.Red;
                return false;
            }

            var phonePattern = @"^\+?[\d\s\-]{7,15}$";
            if (!Regex.IsMatch(phone, phonePattern))
            {
                lbCheckPhone.Text = "Số điện thoại không hợp lệ.";
                lbCheckPhone.ForeColor = Color.Red;
                return false;
            }

            if (!double.TryParse(salary, out double _salary) || _salary < 0)
            {
                lbCheckSalary.Text = "Lương phải là số nguyên dương.";
                lbCheckSalary.ForeColor = Color.Red;
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                lbCheckEmail.Text = "Email không được để trống.";
                lbCheckEmail.ForeColor = Color.Red;
                return false;
            }

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                lbCheckEmail.Text = "Email không hợp lệ.";
                lbCheckEmail.ForeColor = Color.Red;
                return false;
            }

            if (string.IsNullOrEmpty(address))
            {
                lbCheckAddress.Text = "Địa chỉ không được để trống.";
                lbCheckAddress.ForeColor = Color.Red;
                return false;
            }

            // Kiểm tra email và phone có bị trùng không
            var (emailExists, phoneExists) = IUserService.IsEmailOrPhoneExists(email, phone, userId);

            if (emailExists)
            {
                lbCheckEmail.Text = "Email đã được sử dụng.";
                lbCheckEmail.ForeColor = Color.Red;
            }

            if (phoneExists)
            {
                lbCheckPhone.Text = "Số điện thoại đã được sử dụng.";
                lbCheckPhone.ForeColor = Color.Red;
            }

            // Nếu có bất kỳ lỗi nào, return false
            return !(emailExists || phoneExists);
        }

        public static bool ValidationUpdateCategory(string name, int categoryId, Guna2HtmlLabel lbCheckName)
        {
            lbCheckName.Text = String.Empty;

            // Kiểm tra tên danh mục có bị trống không
            if (String.IsNullOrEmpty(name))
            {
                lbCheckName.ForeColor = Color.Red;
                lbCheckName.Text = "Tên danh mục không được để trống.";
                return false;
            }

            // Kiểm tra danh mục đã tồn tại
            bool categoryExists = ICategoryService.IsCategoryExists(name, categoryId);
            if (categoryExists)
            {
                lbCheckName.ForeColor = Color.Red;
                lbCheckName.Text = "Tên danh mục đã tồn tại.";
                return false;
            }

            return true;
        }

        public static bool ValditionInfoSup(string name, string phone, string email, string address, int supplierId,
    Guna2HtmlLabel lbCheckName, Guna2HtmlLabel lbCheckPhone, Guna2HtmlLabel lbCheckEmail, Guna2HtmlLabel lbCheckAddress)
        {
            lbCheckName.Text = lbCheckAddress.Text = lbCheckPhone.Text = lbCheckEmail.Text = String.Empty;

            bool isValid = true;

            if (string.IsNullOrEmpty(name))
            {
                lbCheckName.Text = "Tên không được để trống.";
                lbCheckName.ForeColor = Color.Red;
                isValid = false;
            }

            if (string.IsNullOrEmpty(phone))
            {
                lbCheckPhone.Text = "Số điện thoại không được để trống.";
                lbCheckPhone.ForeColor = Color.Red;
                isValid = false;
            }
            else
            {
                var phonePattern = @"^\+?[\d\s\-]{7,15}$";
                if (!Regex.IsMatch(phone, phonePattern))
                {
                    lbCheckPhone.Text = "Số điện thoại không hợp lệ.";
                    lbCheckPhone.ForeColor = Color.Red;
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(address))
            {
                lbCheckAddress.Text = "Địa chỉ không được để trống.";
                lbCheckAddress.ForeColor = Color.Red;
                isValid = false;
            }

            if (string.IsNullOrEmpty(email))
            {
                lbCheckEmail.Text = "Email không được để trống.";
                lbCheckEmail.ForeColor = Color.Red;
                isValid = false;
            }
            else
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    lbCheckEmail.Text = "Email không hợp lệ.";
                    lbCheckEmail.ForeColor = Color.Red;
                    isValid = false;
                }
            }

            // Nếu dữ liệu không hợp lệ thì không kiểm tra database
            if (!isValid) return false;

            // Kiểm tra email & số điện thoại có tồn tại trong database không
            var (emailExists, phoneExists) = ISupplierService.IsEmailOrPhoneExistsForSupplier(email, phone, supplierId);

            if (emailExists)
            {
                lbCheckEmail.Text = "Email đã được sử dụng.";
                lbCheckEmail.ForeColor = Color.Red;
                isValid = false;
            }

            if (phoneExists)
            {
                lbCheckPhone.Text = "Số điện thoại đã được sử dụng.";
                lbCheckPhone.ForeColor = Color.Red;
                isValid = false;
            }

            return isValid;
        }


        public static bool ValidationAddProduct(string name,string category, string price, Guna2HtmlLabel lbCheckName,Guna2HtmlLabel lbCheckCategory, Guna2HtmlLabel lbCheckPrice)
        {
            lbCheckPrice.Text = lbCheckName.Text = lbCheckCategory.Text = String.Empty;
            if (string.IsNullOrEmpty(name)){
                lbCheckName.Text = "Tên sản phẩm không được để trống.";
                lbCheckName.ForeColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(category))
            {
                lbCheckCategory.Text = "Tên danh mục không được để trống.";
                lbCheckCategory.ForeColor = Color.Red;
                return false;
            }
            if (!double.TryParse(price, out double _price) || _price <= 0)
            {
                lbCheckPrice.Text = "Giá phải là số nguyên dương.";
                lbCheckPrice.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        public static bool ValidationAddCartStock(string name, string category,string supplier,string quantity, string price, Guna2HtmlLabel lbCheckProduct,
            Guna2HtmlLabel lbCheckCategory,Guna2HtmlLabel lbCheckSupplier,Guna2HtmlLabel lbCheckQuantity, Guna2HtmlLabel lbCheckPrice)
        {
            lbCheckPrice.Text = lbCheckProduct.Text = lbCheckCategory.Text = lbCheckSupplier.Text = lbCheckQuantity.Text = String.Empty;
            if (string.IsNullOrEmpty(name))
            {
                lbCheckProduct.Text = "Tên sản phẩm không được để trống.";
                lbCheckProduct.ForeColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(category))
            {
                lbCheckCategory.Text = "Tên danh mục không được để trống.";
                lbCheckCategory.ForeColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(supplier))
            {
                lbCheckSupplier.Text = "Tên nhà cung cấp không được để trống.";
                lbCheckSupplier.ForeColor = Color.Red;
                return false;
            }
            if (!int.TryParse(quantity, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out int _quantity) || _quantity <= 0)
            {
                lbCheckQuantity.Text = "Số lượng phải là số nguyên dương.";
                lbCheckQuantity.ForeColor = Color.Red;
                return false;
            }
            if (!decimal.TryParse(price, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal _price) || _price <= 0)
            {
                lbCheckPrice.Text = "Giá phải là số nguyên dương.";
                lbCheckPrice.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
       
        
    }
}
