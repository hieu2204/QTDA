using ProjectPOS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPOS.Utilities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.Xml.Linq;
using ProjectPOS.Models.DTOs;

namespace ProjectPOS.Servies
{
    public class ICustomerService
    {
        private ConnectDatabase dbConnect = new ConnectDatabase();
        public static List<CustomerModel> GetListCus(int pagenumber,int pagesize)
        {
            List<CustomerModel> lstCus = new List<CustomerModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pagenumber);
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstCus.Add(new CustomerModel
                                {
                                    ID = reader["CustomerID"] is DBNull ? 0 : Convert.ToInt32(reader["CustomerID"]),
                                    Name = reader["FullName"] is DBNull ? string.Empty : reader["FullName"].ToString(),
                                    Email = reader["Email"] is DBNull ? string.Empty : reader["Email"].ToString(),
                                    Phone = reader["Phone"] is DBNull ? string.Empty : reader["Phone"].ToString(),
                                    Address = reader["Address"] is DBNull ? string.Empty : reader["Address"].ToString(),
                                    LoyaltyPoint = reader["LoyaltyPoints"] is DBNull ? 0 : Convert.ToInt32(reader["LoyaltyPoints"]),
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
            return lstCus;
        }
        public static List<CustomerDTO> GetListCustomer()
        {
            List<CustomerDTO> lstCus = new List<CustomerDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetListCus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstCus.Add(new CustomerDTO
                                {
                                    CustomerID = reader["CustomerID"] is DBNull ? 0 : Convert.ToInt32(reader["CustomerID"]),
                                    CustomerName = reader["FullName"] is DBNull ? string.Empty : reader["FullName"].ToString(),
                                    Phone = reader["Phone"] is DBNull ? string.Empty : reader["Phone"].ToString(),
                                    LoyaltyPoints = reader["LoyaltyPoints"] is DBNull ? 0: Convert.ToInt32(reader["LoyaltyPoints"]),
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
            return lstCus;
        }
        public static int GetTotalPage(int pagesize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPage", conn))
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
        public static void UpdateCus(string ID,string Name,string Phone,string Email,string Address,int Status, int LoyaltyPoint)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateCus", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Phone",Phone);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Address", Address);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@LoyaltyPoints", LoyaltyPoint);
                        cmd.Parameters.AddWithValue("@UpdateAt", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch( SqlException ex)
            {
                Console.WriteLine("Error SQl: " + ex.Message);
            }
            catch( Exception ex )
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static void InsertCus(string name, string phone, string email, string address)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertCus", conn))
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
        public static (bool EmailExists, bool PhoneExists) IsCustomerEmailOrPhoneExists(string email, string phone, int customerID)
        {
            bool emailExists = false;
            bool phoneExists = false;

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckCustomerEmailOrPhoneExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                emailExists = Convert.ToBoolean(reader["EmailExists"]);
                                phoneExists = Convert.ToBoolean(reader["PhoneExists"]);
                            }
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

            return (emailExists, phoneExists);
        }
        public static void ArchiveCustomer(int customerID)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("ArchiveCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
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
        public static int GetTotalCustomers()
        {
            int totalCustomers = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        totalCustomers = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return totalCustomers;
        }


    }
}
