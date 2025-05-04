using Guna.UI2.WinForms;
using ProjectPOS.Controllers;
using ProjectPOS.Models;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Servies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjectPOS.Views.Forms
{
    public partial class Function : Form
    {
        private int _currentPage = 1;
        private int _pageSize = 8;
        private CustomerController _customerController = new CustomerController();
        private UserController _userController = new UserController();
        private CategoryController _categoryController = new CategoryController();
        private SupplierController _supplierController = new SupplierController();
        private ProductController _productController = new ProductController();
        private UserModel _user = new UserModel();
        private StockController _stockController = new StockController();
        private PromotionController _promotionController = new PromotionController();
        private SellController _sellController = new SellController();
        private List<UserModel> _allUsers = new List<UserModel>(); // Danh sách gốc
        private List<CustomerModel> _allCustomers = new List<CustomerModel>();
        private List<ProductDTO> _allProducts = new List<ProductDTO>();
        private List<CategoryModel> _allCategories = new List<CategoryModel>();
        private List<SupplierModel> _allSuppliers = new List<SupplierModel>();
        private List<PurInvoiceDTO> _allPurchaseInvoices = new List<PurInvoiceDTO>();
        private List<SellDTO> _allSells = new List<SellDTO>();

        public Function()
        {
            InitializeComponent();
            btnCusManager_Click(btnCusManager, EventArgs.Empty); // Giả lập sự kiện Click khi form mở
        }
        public void DisplayScreen(Guna2Panel pn, Guna2Button btn = null)
        {
            // Đặt lại màu cho tất cả các nút
            foreach (Control control in pnFunction.Controls)
            {
                if (control is Guna2Button button)
                {
                    button.ForeColor = Color.White;
                    button.FillColor = Color.MediumSlateBlue;
                    button.BorderColor = Color.White;
                    button.BorderThickness = 2;
                }
            }

            // Nếu có button được truyền vào thì thay đổi màu của nó
            if (btn != null)
            {
                btn.ForeColor = Color.MediumSlateBlue;
                btn.FillColor = Color.White;
            }

            // Ẩn tất cả các panel
            foreach (Control control in pnScreen.Controls)
            {
                if (control is Guna2Panel panel)
                {
                    panel.Visible = false;
                }
            }

            // Hiển thị panel được truyền vào
            pn.Visible = true;
            pn.BringToFront();
        }

        public void init(UserModel user)
        {
            _user = user;
            if(user != null)
            {
                btnAvatar.Text = user.Name;
                if (!string.IsNullOrEmpty(user.ImageURL))
                {
                    ptbAvatar.Image = Image.FromFile(user.ImageURL);
                }
            }
        }

        public void CheckRole(UserModel user)
        {
            if (user.Role.Equals("Admin"))
            {
                // Admin có toàn quyền
                DisplayScreen(pnHome); // Hiển thị Dashboard
                ptbDashBoard_Click(null, EventArgs.Empty); // Giả lập click vào Dashboard
            }
            else if (user.Role.Equals("Manager"))
            {
                // Quản lý không được quản lý nhân viên
                DisplayScreen(pnHome); // Hiển thị Dashboard
                ptbDashBoard_Click(null, EventArgs.Empty);
                btnEmployee.Visible = false; // Ẩn quản lý nhân viên
            }
            else
            {
                // Nhân viên chỉ xem khách hàng
                ptbDashBoard.Visible = false; // Ẩn dashboard
                btnCusManager_Click(null, EventArgs.Empty); // Load màn hình khách hàng
                btnEmployee.Visible = false;
                btnPromotion.Visible = false;
            }
        }






        // Load Data Customer
        public void LoadDataCus()
        {
            pnListCus.Controls.Clear();

            // Lấy danh sách tất cả khách hàng từ controller
            _allCustomers = _customerController.GetAllCustomer(_currentPage, _pageSize);

            if (_allCustomers == null || _allCustomers.Count == 0)
            {
                MessageBox.Show("Không có khách hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hiển thị danh sách khách hàng
            DisplayCustomers(_allCustomers);

            // Cập nhật nút phân trang
            UpdateButtons(_customerController.GetTotalPageCus(_pageSize), _currentPage);
        }
        // Load Data User
        public void LoadDataUser()
        {
            pnListUser.Controls.Clear();
            _allUsers = _userController.GetAllUser(_currentPage, _pageSize);

            if (_allUsers == null || _allUsers.Count == 0)
                return;

            // Hiển thị danh sách gốc
            DisplayUsers(_allUsers);
            UpdateButtons(_userController.GetTotalPageUser(_pageSize), _currentPage);
            //Console.WriteLine("Page user: " + _userController.GetTotalPageUser(_pageSize));
        }
        // Load Data Product
        public void LoadDataProduct()
        {
            pnListProduct.Controls.Clear();
            // Lấy danh sách tất cả sản phẩm từ controller
            _allProducts = _productController.GetProducts(_currentPage, _pageSize);

            if (_allProducts == null || _allProducts.Count == 0)
            {
                MessageBox.Show("Không có sản phẩm nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hiển thị danh sách sản phẩm
            DisplayProducts(_allProducts);
            UpdateButtons(_productController.GetTotalPage(_pageSize), _currentPage);
        }
        public void LoadDataPromotion()
        {
            pnListPromotion.Controls.Clear();
            List<PromotionModel> promotions = _promotionController.GetListPromotion(_currentPage, _pageSize);
            foreach (PromotionModel promotion in promotions)
            {
                ItemPromotion item = new ItemPromotion();
                item.Init(promotion);
                pnListPromotion.Controls.Add(item);
            }
            UpdateButtons(_promotionController.GetTotalPage(_pageSize), _currentPage);
        }
        private void btnPre_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                Action loadData = GetLoadDataHandler();
                loadData?.Invoke(); // Chỉ gọi nếu loadData khác null
            }
        }
        /*
         * Kiểm tra panel đang hiển thị và trả về delegate tương ứng (Action)
         * Trả về null nếu không có panel nào được hiển thị (xử lý trường hợp edge case).
        */
        private Action GetLoadDataHandler()
        {
            if (pnDisplayCategory.Visible) return LoadDataCategory;
            if (pnDisplayEmployee.Visible) return LoadDataUser;
            if (pnDisplayListCus.Visible) return LoadDataCus;
            if (pnDisplaySupplier.Visible) return LoadDataSupplier;
            if (pnDisplayProduct.Visible) return LoadDataProduct;
            if (pnDisplayPurchase.Visible) return LoadDataPurchaseInvoice;
            if (pnDisplayPromotion.Visible) return LoadDataPromotion;
            if (pnDisplaySell.Visible) return LoadDataSell;

            return null; // Trả về null nếu không có panel nào hiển thị
        }

        // Load Data Category
        public void LoadDataCategory()
        {
            pnListCategory.Controls.Clear();
            _allCategories = _categoryController.GetAllCategory(_currentPage, _pageSize);
            if (_allCategories == null)
            {
                _allCategories = new List<CategoryModel>();
            }

            DisplayCategories(_allCategories); // Hiển thị danh mục sau khi lấy dữ liệu từ database
            UpdateButtons(_categoryController.GetTotalPageCategory(_pageSize), _currentPage);
        }

        // Load Data supplier
        public void LoadDataSupplier()
        {
            pnListSupplier.Controls.Clear();
            _allSuppliers = _supplierController.GetAllSupplier(_currentPage, _pageSize);
            if (_allSuppliers == null)
            {
                _allSuppliers = new List<SupplierModel>();
            }

            DisplaySuppliers(_allSuppliers); // Hiển thị danh sách nhà cung cấp
            UpdateButtons(_supplierController.GetTotalPageSupp(_pageSize), _currentPage);

        }
        public void LoadDataPurchaseInvoice()
        {
            pnListPurchase.Controls.Clear();
            _allPurchaseInvoices = _stockController.GetAllPurchaseInvoice(_currentPage, _pageSize);
            if (_allPurchaseInvoices == null)
            {
                _allPurchaseInvoices = new List<PurInvoiceDTO>();
            }

            DisplayPurchaseInvoices(_allPurchaseInvoices); // Hiển thị danh sách hóa đơn nhập hàng
            UpdateButtons(_stockController.GetTotalPagePurchaseInvoice(_pageSize), _currentPage);
        }
        public void LoadDataSell()
        {
            _allSells = new SellController().GetAllOrder(_currentPage, _pageSize);

            if (_allSells == null || _allSells.Count == 0)
                return;

            DisplaySells(_allSells);
            UpdateButtons(new SellController().GetTotalPageOrder(_pageSize), _currentPage);
        }
        /*
         * Lấy handlers phù hợp dựa vào panel đang hiển thị
         * Kiểm tra điều kiện trang hiện tại và thực hiện tăng trang + load data
        */
        private void btnNext_Click(object sender, EventArgs e)
        {
            var (getTotalPage, loadData) = GetCurrentHandlers();
            if (getTotalPage == null || loadData == null) return;

            int totalPage = getTotalPage();
            if (_currentPage < totalPage)
            {
                _currentPage++;
                loadData();
            }
        }
        /*
         * Phương thức GetCurrentHandlers:

           Kiểm tra lần lượt các panel và trả về cặp delegate tương ứng

           Func<int>: Đại diện cho phương thức tính tổng số trang

           Action: Đại diện cho phương thức load dữ liệu

           Sử dụng lambda expression để truyền tham số _pageSize
         
        */
        private (Func<int> getTotalPage, Action loadData) GetCurrentHandlers()
        {
            if (pnDisplayEmployee.Visible)
                return (() => _userController.GetTotalPageUser(_pageSize), LoadDataUser);

            if (pnDisplayListCus.Visible)
                return (() => _customerController.GetTotalPageCus(_pageSize), LoadDataCus);

            if (pnDisplayCategory.Visible)
                return (() => _categoryController.GetTotalPageCategory(_pageSize), LoadDataCategory);

            if (pnDisplaySupplier.Visible)
                return (() => _supplierController.GetTotalPageSupp(_pageSize), LoadDataSupplier);
           
            if (pnDisplayProduct.Visible)
                return (() => _productController.GetTotalPage(_pageSize), LoadDataProduct);
            if (pnDisplayPurchase.Visible)
                return (() => _stockController.GetTotalPagePurchaseInvoice(_pageSize), LoadDataPurchaseInvoice);
            if (pnDisplayPromotion.Visible)
                return (() => _promotionController.GetTotalPage(_pageSize), LoadDataPromotion);
            if (pnDisplaySell.Visible)
                return (() => _sellController.GetTotalPageOrder(_pageSize), LoadDataSell);
            return (null, null);
        }
        public void UpdateButtons(int totalPage, int currentPage)
        {
            pnPage.Visible = true;
            // Nếu không có trang nào thì ẩn hết các nút phân trang
            if (totalPage <= 0)
            {
                foreach (Guna2Button btn in pnPage.Controls)
                    btn.Visible = false;
                return;
            }
            else
            {

                // Cập nhật nút điều hướng Pre và Next
                btnPre.Visible = (currentPage > 1);
                btnNext.Visible = (currentPage < totalPage);

                // Cập nhật nút số trang
                Guna2Button[] pageButtons = { btnPage1, btnPage2, btnPage3, btnPage4, btnPage5 };
                int maxButtons = pageButtons.Length;
                int startPage = Math.Max(1, currentPage - 2);
                int endPage = Math.Min(totalPage, startPage + maxButtons - 1);
                startPage = Math.Max(1, endPage - maxButtons + 1);

                for (int i = 0; i < maxButtons; i++)
                {
                    int pageNumber = startPage + i;
                    if (pageNumber <= totalPage)
                    {
                        pageButtons[i].Text = pageNumber.ToString();
                        pageButtons[i].Visible = true;
                        pageButtons[i].FillColor = (pageNumber == currentPage) ? Color.OrangeRed : Color.White;
                    }
                    else
                    {
                        pageButtons[i].Visible = false;
                    }
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(pnDisplayListCus.Visible)
            {
                AddCustomer addCustomer = new AddCustomer();
                addCustomer._OnLoadCus += LoadDataCus;
                addCustomer.ShowDialog();
            }
            if(pnDisplayEmployee.Visible)
            {
                AddUser addUser = new AddUser();
                addUser._OnLoadUser += LoadDataUser;
                addUser.ShowDialog();
            }
            if (pnDisplayCategory.Visible)
            {
                AddCategory addCategory = new AddCategory();
                addCategory._OnLoadCate += LoadDataCategory;
                addCategory.ShowDialog();
            }
            if (pnDisplaySupplier.Visible)
            {
                AddSupplier addSupplier = new AddSupplier();
                addSupplier._OnLoadSup += LoadDataSupplier;
                addSupplier.ShowDialog();
            }
            if (pnDisplayProduct.Visible)
            {
                AddProduct addProduct = new AddProduct();
                addProduct._OnLoadProduct += LoadDataProduct;
                addProduct.ShowDialog();
            }
            if (pnDisplayPurchase.Visible)
            {
                AddStock addStock = new AddStock();
                addStock.LoadEmployee(_user);
                addStock._OnLoadPurchase += LoadDataPurchaseInvoice;
                addStock.ShowDialog();
            }
            if (pnDisplayPromotion.Visible)
            {
                AddPromotion addPromotion = new AddPromotion();
                addPromotion.OnLoadPromotion += LoadDataPromotion;
                addPromotion.ShowDialog();
            }
            if (pnDisplaySell.Visible)
            {
                AddSell addSell = new AddSell();
                addSell.LoadEmployee(_user);
                addSell.OnLoadSell += LoadDataSell;
                addSell.ShowDialog();
            }
        }

        private void ptbDashBoard_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnHome);
            cbTimeFilter.SelectedIndex = 0;  // Chọn "Hôm nay"
            LoadRevenueChart("Day"); // Cập nhật biểu đồ ngay lập tức
            txtEmployee.Text = IUserService.GetTotalEmployees().ToString();
            txtCustomer.Text = ICustomerService.GetTotalCustomers().ToString();
            txtOrder.Text = ISellService.GetTotalOrders().ToString();
            pnEmployees.FillColor = Color.FromArgb(0, 123, 255); // Xanh dương (Nhân viên)
            pnCustomers.FillColor = Color.FromArgb(40, 167, 69); // Xanh lá (Khách hàng)
            pnOrders.FillColor = Color.FromArgb(255, 193, 7);    // Vàng (Đơn hàng)
            txtEmployee.FillColor = Color.FromArgb(0, 123, 255);
            txtCustomer.FillColor = Color.FromArgb(40, 167, 69); 
            txtOrder.FillColor = Color.FromArgb(255, 193, 7);
            lbTotal.Text = ISellService.GetTotalRevenue().ToString("N0");
            pnPage.Visible = false;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplaySell, btnOrder);
            _currentPage = 1;
            _pageSize = 7;
            LoadDataSell();
            lbTitle.Text = "Order";
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplayProduct, btnProduct);
            _currentPage = 1;
            _pageSize = 8;
            LoadDataProduct();
            lbTitle.Text = "Product";
        }

        private void btnCusManager_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplayListCus, btnCusManager);
            _currentPage = 1;
            _pageSize = 8;
            LoadDataCus();
            lbTitle.Text = "Customer";
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplayEmployee, btnEmployee);
            _currentPage = 1;
            _pageSize = 6;
            LoadDataUser();
            lbTitle.Text = "Employee";
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplayCategory, btnCategory);
            _currentPage = 1;
            _pageSize = 9;
            LoadDataCategory();
            lbTitle.Text = "Category";
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplaySupplier, btnSupplier);
            _currentPage = 1;
            _pageSize = 4;
            LoadDataSupplier();
            lbTitle.Text = "Supplier";
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplayPurchase, btnStock);
            lbTitle.Text = "Stock";
            _currentPage = 1;
            _pageSize = 7;
            LoadDataPurchaseInvoice();
        }

        private void btnPromotion_Click(object sender, EventArgs e)
        {
            DisplayScreen(pnDisplayPromotion, btnPromotion);
            LoadDataPromotion();
            _currentPage = 1;
            _pageSize = 7;
            lbTitle.Text = "Promotion";
        }
        // Find
        private void DisplayUsers(List<UserModel> users)
        {
            pnListUser.Controls.Clear();


            foreach (var user in users)
            {
                ItemUser item = new ItemUser();
                item.Init(user);
                item.OnLoadClick += LoadDataUser;
                pnListUser.Controls.Add(item);
            }
        }

        private void DisplayCustomers(List<CustomerModel> customers)
        {
            foreach (var customer in customers)
            {
                ItemCus item = new ItemCus();
                item.init(customer);
                item.OnLoadClick += LoadDataCus;
                pnListCus.Controls.Add(item);
            }
        }
        private void DisplayProducts(List<ProductDTO> products)
        {
            pnListProduct.Controls.Clear();
            foreach (var product in products)
            {
                ItemProduct item = new ItemProduct();
                item.Init(product);
                item.OnLoadClick += LoadDataProduct;
                pnListProduct.Controls.Add(item);
            }
        }
        private void DisplayCategories(List<CategoryModel> categories)
        {
            pnListCategory.Controls.Clear();

            foreach (var category in categories)
            {
                ItemCategory item = new ItemCategory();
                item.Init(category);
                item.OnLoadClick += LoadDataCategory;
                pnListCategory.Controls.Add(item);
            }
        }
        private void DisplaySuppliers(List<SupplierModel> suppliers)
        {
            pnListSupplier.Controls.Clear();

            foreach (var supplier in suppliers)
            {
                ItemSupplier item = new ItemSupplier();
                item.Init(supplier);
                item.OnLoadClick += LoadDataSupplier;
                pnListSupplier.Controls.Add(item);
            }
        }
        private void DisplayPurchaseInvoices(List<PurInvoiceDTO> invoices)
        {
            pnListPurchase.Controls.Clear();

            foreach (var invoice in invoices)
            {
                ItemStock item = new ItemStock();
                item.Init(invoice);
                pnListPurchase.Controls.Add(item);
            }
        }
        public void DisplaySells(List<SellDTO> sells)
        {
            pnListSell.Controls.Clear();

            if (sells == null || sells.Count == 0)
            {
                MessageBox.Show("Không có đơn hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var sell in sells)
            {
                ItemOrder itemOrder = new ItemOrder();
                itemOrder.Init(sell);
                pnListSell.Controls.Add(itemOrder);
            }
        }

        private Action<string> GetSearchHandler()
        {
            if (pnDisplayEmployee.Visible) return SearchUser;
            if (pnDisplayListCus.Visible) return SearchCustomer;
            if (pnDisplayCategory.Visible) return SearchCategory;
            if (pnDisplaySupplier.Visible) return SearchSupplier;
            if (pnDisplayProduct.Visible) return SearchProduct;
            if (pnDisplayPurchase.Visible) return SearchPurchaseInvoice;
            //if (pnDisplayPromotion.Visible) return SearchPromotion;
            if (pnDisplaySell.Visible) return SearchSell;
            return null;
        }
        private void ShowNotFoundMessage(FlowLayoutPanel pn)
        {
            pn.Controls.Clear();

            Label lblNotFound = new Label
            {
                Text = "Không tìm thấy kết quả!",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                Location = new Point(10, 10)
            };

            pn.Controls.Add(lblNotFound);
        }

        public void SearchUser(string keyword)
        {
            pnListUser.Controls.Clear();
            UserController userController = new UserController();
            List<UserModel> filteredUsers;

            // Nếu không nhập từ khóa thì lấy tất cả người dùng
            if (string.IsNullOrEmpty(keyword))
            {
                filteredUsers = _allUsers; 
            }
            else
            {
                filteredUsers = userController.SearchUser(keyword);
            }

            // Kiểm tra danh sách trả về có rỗng không
            if (filteredUsers == null || filteredUsers.Count == 0)
            {
                ShowNotFoundMessage(pnListUser);
                return;
            }

            // Hiển thị danh sách đã lọc hoặc toàn bộ từ database
            DisplayUsers(filteredUsers);
        }



        public void SearchCustomer(string keyword)
        {
            pnListCus.Controls.Clear();

            // Nếu không nhập gì thì hiển thị tất cả khách hàng
            if (string.IsNullOrWhiteSpace(keyword))
            {
                DisplayCustomers(_allCustomers);
                return;
            }

            // Lọc danh sách khách hàng theo tên, số điện thoại hoặc email
            var filteredCustomers = _allCustomers.Where(c =>
                                   (c.Name != null && c.Name.IndexOf(keyword.Trim(), StringComparison.OrdinalIgnoreCase) >= 0) ||
                                   (c.Phone != null && c.Phone.IndexOf(keyword.Trim(), StringComparison.OrdinalIgnoreCase) >= 0) ||
                                   (c.Email != null && c.Email.IndexOf(keyword.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                                   ).ToList();

            // Nếu không tìm thấy kết quả, hiển thị thông báo
            if (filteredCustomers.Count == 0)
            {
                MessageBox.Show("Không tìm thấy khách hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hiển thị danh sách khách hàng đã lọc
            DisplayCustomers(filteredCustomers);
        }
        public void SearchProduct(string keyword)
        {
            pnListProduct.Controls.Clear();
            ProductController productController = new ProductController();
            List<ProductDTO> filteredProducts;

            // Tìm kiếm sản phẩm theo từ khóa
            filteredProducts = productController.SearchProduct(keyword);

            // Kiểm tra danh sách trả về có rỗng không
            if (filteredProducts == null || filteredProducts.Count == 0)
            {
                ShowNotFoundMessage(pnListProduct); // Hiển thị thông báo không tìm thấy
                return;
            }

            // Hiển thị danh sách đã lọc hoặc toàn bộ từ database
            DisplayProducts(filteredProducts);
        }

        public void SearchCategory(string keyword)
        {
            pnListCategory.Controls.Clear();
            CategoryController categoryController = new CategoryController();
            List<CategoryModel> filteredCategories;

            // Nếu không nhập từ khóa thì lấy tất cả danh mục
            if (string.IsNullOrEmpty(keyword))
            {
                filteredCategories = _allCategories;
            }
            else
            {
                filteredCategories = categoryController.SearchCategory(keyword);
            }

            // Kiểm tra danh sách trả về có rỗng không
            if (filteredCategories == null || filteredCategories.Count == 0)
            {
                ShowNotFoundMessage(pnListCategory);
                return;
            }

            // Hiển thị danh sách đã lọc hoặc toàn bộ từ database
            DisplayCategories(filteredCategories);
        }


        public void SearchSupplier(string keyword)
        {
            pnListSupplier.Controls.Clear();
            SupplierController supplierController = new SupplierController();
            List<SupplierModel> filteredSuppliers;

            // Tìm kiếm nhà cung cấp theo từ khóa
            filteredSuppliers = supplierController.SearchSupplier(keyword);

            // Kiểm tra danh sách trả về có rỗng không
            if (filteredSuppliers == null || filteredSuppliers.Count == 0)
            {
                ShowNotFoundMessage(pnListSupplier); // Hiển thị thông báo không tìm thấy
                return;
            }

            // Hiển thị danh sách đã lọc hoặc toàn bộ từ database
            DisplaySuppliers(filteredSuppliers);
        }

        public void SearchPurchaseInvoice(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                DisplayPurchaseInvoices(_allPurchaseInvoices); // Hiển thị toàn bộ nếu không nhập gì
                return;
            }

            var filteredInvoices = _allPurchaseInvoices.Where(p =>
                p.ReceiptID.ToString().Contains(keyword) ||  // Mã hóa đơn (int -> string)
                (!string.IsNullOrEmpty(p.EmployeeName) && p.EmployeeName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) || // Tên nhân viên
                p.ReceiptDate.ToString("yyyy-MM-dd").Contains(keyword) // Ngày nhập
            ).ToList();

            if (filteredInvoices.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn nhập hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DisplayPurchaseInvoices(filteredInvoices);
        }


        public void SearchSell(string keyword)
        {
            pnListSell.Controls.Clear(); // Làm sạch danh sách đơn hàng hiện tại

            SellController sellController = new SellController();
            List<SellDTO> filteredSells;

            // Gọi searchOrder từ controller
            filteredSells = sellController.SearchOrder(keyword);

            // Kiểm tra danh sách trả về có rỗng không
            if (filteredSells == null || filteredSells.Count == 0)
            {
                ShowNotFoundMessage(pnListSell); // Hiển thị thông báo không tìm thấy
                return;
            }

            // Hiển thị các đơn hàng đã lọc
            DisplaySells(filteredSells);
        }





        private void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string keyword = txtFind.Text.Trim();
                Action<string> searchHandler = GetSearchHandler();
                searchHandler?.Invoke(keyword);

                // Ngăn tiếng 'ding' mặc định khi nhấn Enter
                e.SuppressKeyPress = true;
            }
        }
        private void LoadRevenueChart(string filterType)
        {
            DataTable dt = ISellService.GetRevenue(filterType);

            // Xóa dữ liệu cũ
            chartRevenue.Series.Clear();
            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 3;
            series.Color = System.Drawing.Color.Blue;

            foreach (DataRow row in dt.Rows)
            {
                string label = (filterType == "Day") ? Convert.ToDateTime(row["Ngay"]).ToString("dd/MM") :
                              (filterType == "Week") ? "Tuần " + row["Tuan"] :
                              "Tháng " + row["Thang"];
                series.Points.AddXY(label, Convert.ToDouble(row["DoanhThu"]));
            }

            chartRevenue.Series.Add(series);
            chartRevenue.ChartAreas[0].AxisX.Title = filterType;
            chartRevenue.ChartAreas[0].AxisY.Title = "Doanh thu";
            chartRevenue.ChartAreas[0].AxisX.Interval = 1;
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = cbTimeFilter.SelectedItem.ToString();
            string filterType = selectedValue == "Hôm nay" ? "Day" :
                                selectedValue == "Tuần này" ? "Week" : "Month";
            LoadRevenueChart(filterType);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất",
                                          MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                this.Hide(); // Ẩn form Function
                Login loginForm = new Login();
                loginForm.ShowDialog(); // Mở lại form đăng nhập
                this.Close(); // Đóng hoàn toàn form Function khi đăng nhập lại xong
            }
        }
    }
}
