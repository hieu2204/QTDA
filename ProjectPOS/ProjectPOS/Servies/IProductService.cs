using ProjectPOS.Models;
using ProjectPOS.Models.DTOs;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Servies
{
    public class IProductService
    {

        public static void InsertProduct(string name,int category, double price, string description, string imageurl)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("AddProduct", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@CategoryID", category);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ImageURL", imageurl);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException sqlEx)
            {
                Console.WriteLine("Error SQL: " +  sqlEx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static List<ProductDTO> GetListProduct(int pagenumber, int pagesize)
        {
            List<ProductDTO> lstSup = new List<ProductDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pagenumber);
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstSup.Add(new ProductDTO
                                {
                                    ProductID = reader["ProductID"] is DBNull ? 0 : Convert.ToInt32(reader["ProductID"]),
                                    ProductName = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString(),
                                    CategoryName = reader["CategoryName"] is DBNull ? string.Empty : reader["CategoryName"].ToString(),
                                    SupplierName = reader["SupplierName"] is DBNull ? string.Empty : reader["SupplierName"].ToString(),
                                    Price = reader["Price"] is DBNull ? 0 : Convert.ToDouble(reader["Price"]),
                                    StockQuantity = reader["AvailableStock"] is DBNull ? 0 : Convert.ToInt32(reader["AvailableStock"]),
                                    Description = reader["Description"] is DBNull ? string.Empty : reader["Description"].ToString(),
                                    ImageURL = reader["ImageURL"] is DBNull ? string.Empty : reader["ImageURL"].ToString(),

                                    // Xử lý DateTime
                                    CreateAt = reader["CreatedAt"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdateAt = reader["UpdatedAt"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["UpdatedAt"]),

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
            return lstSup;
        }
        public static int GetTotalPageProduct(int pagesize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPageProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
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
        public static void UpdateProduct(string name,int categoryID,double price, string description, string imageURL, int productid)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@CategoryID",categoryID);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ImageURL", imageURL);
                        cmd.Parameters.AddWithValue("@ProductID", productid);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch( SqlException ex )
            {
                Console.WriteLine("Error SQL: " + ex.Message );
            }
            catch( Exception ex )
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static List<ProductDTO> GetListProduct()
        {
            List<ProductDTO> productDTOs = new List<ProductDTO>();
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetListProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productDTOs.Add(new ProductDTO
                                {
                                    ProductID = reader["ProductID"] is DBNull ? 0 : Convert.ToInt32(reader["ProductID"]),
                                    ProductName = reader["ProductName"] is DBNull ? string.Empty : reader["ProductName"].ToString(),
                                    CategoryName = reader["CategoryName"] is DBNull ? string.Empty : reader["CategoryName"].ToString(),
                                    Price = reader["Price"] is DBNull ? 0 : Convert.ToDouble(reader["Price"]),
                                    SupplierName = reader["SupplierName"] is DBNull ? string.Empty : reader["SupplierName"].ToString(),
                                    ImageURL = reader["ImageURL"] is DBNull ? string.Empty : reader["ImageURL"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch ( SqlException ex )
            {
                Console.WriteLine("Sql Error: " + ex.Message);
            }
            catch ( Exception ex )
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return productDTOs;
        }
        public static int GetTotalQuantity(int productID)
        {
            int total = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalQuantity", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        total = Convert.ToInt32(cmd.ExecuteScalar()); // lấy 1 giá trị duy nhất
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Sql Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return total;
        }
        public static List<ProductDTO> SearchProduct(string keyword)
        {
            List<ProductDTO> products = new List<ProductDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SearchProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", (object)keyword ?? DBNull.Value);  // Nếu không có từ khóa, truyền NULL

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new ProductDTO
                                {
                                    ProductID = reader["ProductID"] is DBNull ? 0 : Convert.ToInt32(reader["ProductID"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    CategoryName = reader["CategoryName"].ToString(),
                                    SupplierName = reader["SupplierName"].ToString(),
                                    Price = reader["Price"] is DBNull ? 0 : Convert.ToDouble(reader["Price"]),
                                    StockQuantity = reader["TotalStock"] is DBNull ? 0 : Convert.ToInt32(reader["TotalStock"]),
                                    Description = reader["Description"].ToString(),
                                    ImageURL = reader["ImageURL"].ToString(),
                                    CreateAt = reader["CreatedAt"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdateAt = reader["UpdatedAt"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedAt"])
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

            return products;
        }

    }
}
