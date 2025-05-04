using Newtonsoft.Json;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Models.Enitties;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Servies
{
    public class ISellService
    {
        public static int GetNextID()
        {
            int id = 1;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetNextSellID", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LastID", id);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return id;
        }
        public static string ConvertSellCartDataGridViewToJSON(DataGridView dgv)
        {
            var productList = new List<SellCartProductModel>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;
                Console.WriteLine("ProductID: " + row.Cells["ProductID"].Value);
                Console.WriteLine("Quantity: " + row.Cells["Quantity"].Value);
                Console.WriteLine("UnitPrice: " + row.Cells["UnitPrice"].Value);
                Console.WriteLine("GiftQuantity: " + row.Cells["GiftQuantity"].Value);

                // Kiểm tra các cột cần thiết (ProductID, Quantity, UnitPrice)
                if (row.Cells["ProductID"].Value != null &&
                    row.Cells["Quantity"].Value != null &&
                    row.Cells["UnitPrice"].Value != null)
                {
                    try
                    {
                        int productID = Convert.ToInt32(row.Cells["ProductID"].Value);
                        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        int giftQuantity = 0;
                        // Nếu có cột GiftQuantity, lấy giá trị, nếu không thì coi là 0
                        if (row.Cells["GiftQuantity"].Value != null)
                        {
                            giftQuantity = Convert.ToInt32(row.Cells["GiftQuantity"].Value);
                        }
                        int totalQuantity = quantity + giftQuantity;
                        double unitPrice = Convert.ToDouble(row.Cells["UnitPrice"].Value);
                        // Tổng tiền chỉ tính số lượng mua (không tính quà tặng)
                        double totalPrice = quantity * unitPrice;

                        productList.Add(new SellCartProductModel
                        {
                            ProductID = productID,
                            Quantity = quantity,
                            GiftQuantity = giftQuantity,
                            TotalQuantity = totalQuantity,
                            UnitPrice = unitPrice,
                            TotalPrice = totalPrice
                        });
                    }
                    catch
                    {
                        Console.WriteLine("Lỗi chuyển đổi dữ liệu");
                    }
                }
            }
            Console.WriteLine("List: " + JsonConvert.SerializeObject(productList));
            return JsonConvert.SerializeObject(productList);
        }
        public static void SubmitSellOrder(int userId, decimal totalAmount, decimal finalTotalAmount, DataGridView dgvCart, int? customerId, int paymentStatus)
        {
            // Convert dữ liệu giỏ hàng bán sang JSON (chỉ tính số lượng mua trong thành tiền, nhưng tổng số lượng = mua + quà tặng)
            string orderDetailsJson = ConvertSellCartDataGridViewToJSON(dgvCart);
            Console.WriteLine("JSON Result: " + orderDetailsJson);
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_InsertOrderFromJson", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm các tham số cho thủ tục
                        cmd.Parameters.AddWithValue("@CustomerID", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@FinalTotalAmount", finalTotalAmount);
                        cmd.Parameters.AddWithValue("@MethodID", paymentStatus);
                        cmd.Parameters.AddWithValue("@OrderDetailsJson", orderDetailsJson);

                        // Nếu bạn cần lấy OrderID vừa được tạo, sử dụng ExecuteScalar; nếu không, ExecuteNonQuery cũng được.
                        var result = cmd.ExecuteScalar();
                        int orderId = result != null ? Convert.ToInt32(result) : 0;

                        MessageBox.Show("Đơn hàng đã được lưu thành công! OrderID: " + orderId, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static List<SellDTO> GetListOrder(int pageNumber, int pageSize)
        {
            List<SellDTO> lstPur = new List<SellDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllOrder", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstPur.Add(new SellDTO
                                {
                                    OrderID = reader["OrderID"] is DBNull ? 0 : Convert.ToInt32(reader["OrderID"]),
                                    EmployeeName = reader["EmployeeName"] is DBNull ? string.Empty : reader["EmployeeName"].ToString(),
                                    CustomerName = reader["CustomerName"] is DBNull ? "Khách lẻ" : reader["CustomerName"].ToString(),
                                    OrderDate = reader["OrderDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["OrderDate"]),
                                    TotalAmount = reader["TotalAmount"] is DBNull ? 0 : Convert.ToDecimal(reader["TotalAmount"]),
                                    FinalTotalAmount = reader["FinalTotalAmount"] is DBNull ? 0 : Convert.ToDecimal(reader["FinalTotalAmount"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)  // Bắt lỗi SQL
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)  // Bắt các lỗi khác
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return lstPur;
        }
        public static int GetTotalPageOrder(int pageSize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPageOrder", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        totalPage = Convert.ToInt32(cmd.ExecuteScalar()); // lấy 1 giá trị duy nhất
                    }
                }
            }
            catch (SqlException ex)  // Bắt lỗi SQL
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)  // Bắt các lỗi khác
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return totalPage;
        }
        public static List<SellDTO> SearchOrder(string keyword)
        {
            List<SellDTO> orders = new List<SellDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SearchOrder", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Truyền tham số @Keyword vào thủ tục
                        cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : (object)keyword);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new SellDTO
                                {
                                    OrderID = Convert.ToInt32(reader["OrderID"]),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    CustomerName = reader["CustomerName"] == DBNull.Value ? "Khách lẻ" : reader["CustomerName"].ToString(),
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                    FinalTotalAmount = Convert.ToDecimal(reader["FinalTotalAmount"])
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return orders;
        }
        public static DataTable GetRevenue(string type)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetRevenueByTimePeriod", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", type);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt); // Đổ dữ liệu vào DataTable
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return dt;
        }
        public static int GetTotalOrders()
        {
            int totalOrders = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalOrders", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        totalOrders = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return totalOrders;
        }
        public static decimal GetTotalRevenue()
        {
            decimal totalRevenue = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalRevenue", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalRevenue = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return totalRevenue;
        }
    }
}
