using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPOS.Models.DTOs;

namespace ProjectPOS.Servies
{
    public class ISupplierService
    {
        public static List<SupplierModel> GetListSupplier(int pagenumber, int pagesize)
        {
            List<SupplierModel> lstSup = new List<SupplierModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pagenumber);
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstSup.Add(new SupplierModel
                                {
                                    SupplierID = reader["SupplierID"] is DBNull ? 0 : Convert.ToInt32(reader["SupplierID"]),
                                    SupplierName = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString(),
                                    Email = reader["Email"] is DBNull ? string.Empty : reader["Email"].ToString(),
                                    Phone = reader["Phone"] is DBNull ? string.Empty : reader["Phone"].ToString(),
                                    Address = reader["Address"] is DBNull ? string.Empty : reader["Address"].ToString(),
                                    Status = reader["Status"] is DBNull ? 0 : Convert.ToInt32(reader["Status"]),

                                    // Xử lý DateTime
                                    CreateAt = reader["CreatedAt"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdateAt = reader["UpdatedAt"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["UpdatedAt"])
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
        public static int GetTotalPageSupplier(int pagesize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPageSupplier", conn))
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
        public static void UpdateSupplier(int ID, string Name, string Phone, string Email, string Address, int Status)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Phone", Phone);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@UpdateAt", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQl: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static void InsertSup(string name, string phone, string email, string address)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQl: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static List<SupplierDTO> GetSuppliers()
        {
            List<SupplierDTO> suppliers = new List<SupplierDTO>();
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetListSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppliers.Add(new SupplierDTO
                                {
                                    SupplierID = reader["SupplierID"] is DBNull ? 0 : Convert.ToInt32(reader["SupplierID"]),
                                    SupplierName = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch( SqlException ex)
            {
                Console.WriteLine("Sql Error: " + ex.Message);
            }
            catch( Exception ex )
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return suppliers;
        }
        public static (bool emailExists, bool phoneExists) IsEmailOrPhoneExistsForSupplier(string email, string phone, int supplierId)
        {
            bool emailExists = false;
            bool phoneExists = false;

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckSupplierEmailOrPhoneExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@SupplierID", supplierId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                emailExists = reader.GetBoolean(0);
                                phoneExists = reader.GetBoolean(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return (emailExists, phoneExists);
        }
        public static void ArchiveSupplier(int supplierID)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("ArchiveSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                        cmd.ExecuteNonQuery();
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
        }
        public static List<SupplierModel> SearchSupplier(string keyword)
        {
            List<SupplierModel> suppliers = new List<SupplierModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SearchSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", (object)keyword ?? DBNull.Value);  // Nếu không có từ khóa, truyền NULL

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                suppliers.Add(new SupplierModel
                                {
                                    SupplierID = reader["SupplierID"] is DBNull ? 0 : Convert.ToInt32(reader["SupplierID"]),
                                    SupplierName = reader["Name"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Status = reader["Status"] is DBNull ? 0 : Convert.ToInt32(reader["Status"]),
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

            return suppliers;
        }


    }
}
