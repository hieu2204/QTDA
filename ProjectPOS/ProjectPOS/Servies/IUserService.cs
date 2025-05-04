using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Servies
{
    public class IUserService
    {
        private ConnectDatabase dbConnect = new ConnectDatabase();
        public static void InsertUser(string user,string pass, string name, string email,string phone, string gender, DateTime birth, string address, string image, double salary, string role)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", user);
                        cmd.Parameters.AddWithValue("@PasswordHash", pass);
                        cmd.Parameters.AddWithValue("@FullName", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@BirthDate", birth);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@ImageURL", image);
                        cmd.Parameters.AddWithValue("@Salary", salary);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error SQL: " +ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static int GetTotalPage(int pagesize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPageUser", conn))
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
        public static List<UserModel> GetListUser(int pagenumber, int pagesize)
        {
            List<UserModel> lstUser = new List<UserModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pagenumber);
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstUser.Add(new UserModel
                                {
                                    Id = reader["UserID"] is DBNull ? 0 : Convert.ToInt32(reader["UserID"]),
                                    UserName = reader["Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    Name = reader["FullName"] is DBNull ? string.Empty : reader["FullName"].ToString(),
                                    Email = reader["Email"] is DBNull ? string.Empty : reader["Email"].ToString(),
                                    Phone = reader["Phone"] is DBNull ? string.Empty : reader["Phone"].ToString(),
                                    Gender = reader["Gender"] is DBNull ? string.Empty : reader["Gender"].ToString(),
                                    BirthDate = reader["BirthDate"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                    Address = reader["Address"] is DBNull ? string.Empty : reader["Address"].ToString(),
                                    ImageURL = reader["ImageURL"] is DBNull ? String.Empty : reader["ImageURL"].ToString(),
                                    Salary = reader["Salary"] is DBNull ? 0 : Convert.ToDouble(reader["Salary"]),
                                    Status = reader["Status"] is DBNull ? 0 : Convert.ToInt32(reader["Status"]),
                                    Role = reader["Role"] is DBNull ? String.Empty : reader["Role"].ToString(),

                                    // Xử lý DateTime
                                    CreateAt = reader["CreatedAt"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdateAt = reader["UpdatedAt"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedAt"])
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
            return lstUser;
        }
        public static List<UserModel> GetListUser()
        {
            List<UserModel> lstUser = new List<UserModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetListUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstUser.Add(new UserModel
                                {
                                    Id = reader["UserID"] is DBNull ? 0 : Convert.ToInt32(reader["UserID"]),
                                    UserName = reader["Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    Name = reader["FullName"] is DBNull ? string.Empty : reader["FullName"].ToString(),
                                    Email = reader["Email"] is DBNull ? string.Empty : reader["Email"].ToString(),
                                    Phone = reader["Phone"] is DBNull ? string.Empty : reader["Phone"].ToString(),
                                    Gender = reader["Gender"] is DBNull ? string.Empty : reader["Gender"].ToString(),
                                    BirthDate = reader["BirthDate"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                    Address = reader["Address"] is DBNull ? string.Empty : reader["Address"].ToString(),
                                    ImageURL = reader["ImageURL"] is DBNull ? String.Empty : reader["ImageURL"].ToString(),
                                    Salary = reader["Salary"] is DBNull ? 0 : Convert.ToDouble(reader["Salary"]),
                                    Status = reader["Status"] is DBNull ? 0 : Convert.ToInt32(reader["Status"]),
                                    Role = reader["Role"] is DBNull ? String.Empty : reader["Role"].ToString(),

                                    // Xử lý DateTime
                                    CreateAt = reader["CreatedAt"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]),
                                    UpdateAt = reader["UpdatedAt"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedAt"])
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
            return lstUser;
        }
        public static bool IsUsernameExit(string username)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using(SqlCommand cmd = new SqlCommand("GetUserName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        int result = (int)cmd.ExecuteScalar();
                        return result > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQL:" + ex.Message);
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public static void UpdateUser(int id,string pass,string name, string email, string phone, string gender,
            DateTime birth, string address, string imageURL, double salary, int status, string role)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Pass", pass);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@BirthDate", birth);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@ImageURL", imageURL);
                        cmd.Parameters.AddWithValue("@Salary", salary);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQL:" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static bool IsEmailExists(string email)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckEmailExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        int result = (int)cmd.ExecuteScalar();
                        return result > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQL: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        public static bool IsPhoneExists(string phone)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckPhoneExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        int result = (int)cmd.ExecuteScalar();
                        return result > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error SQL: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public static (bool emailExists, bool phoneExists) IsEmailOrPhoneExists(string email, string phone, int userId)
        {
            bool emailExists = false;
            bool phoneExists = false;

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckEmailOrPhoneExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@UserID", userId);

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

        /*
         * Lợi ích của GetOrdinal():

            Tối ưu tốc độ 🚀: Chỉ tìm cột một lần duy nhất, sau đó lấy dữ liệu theo chỉ mục.

            Tránh lỗi chính tả: Nếu viết sai tên cột, lỗi sẽ xuất hiện ngay khi gọi GetOrdinal().
         */
        public static List<UserModel> SearchUsers(string keyword)
        {
            List<UserModel> users = new List<UserModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SearchUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", keyword);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new UserModel
                                {
                                    Id = reader["UserID"] is DBNull ? 0 : Convert.ToInt32(reader["UserID"]),
                                    UserName = reader["Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    Name = reader["FullName"] is DBNull ? string.Empty : reader["FullName"].ToString(),
                                    Email = reader["Email"] is DBNull ? string.Empty : reader["Email"].ToString(),
                                    Phone = reader["Phone"] is DBNull ? string.Empty : reader["Phone"].ToString(),
                                    Gender = reader["Gender"] is DBNull ? string.Empty : reader["Gender"].ToString(),
                                    BirthDate = reader["BirthDate"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"]),
                                    Address = reader["Address"] is DBNull ? string.Empty : reader["Address"].ToString(),
                                    ImageURL = reader["ImageURL"] is DBNull ? String.Empty : reader["ImageURL"].ToString(),
                                    Salary = reader["Salary"] is DBNull ? 0 : Convert.ToDouble(reader["Salary"]),
                                    Status = reader["Status"] is DBNull ? 0 : Convert.ToInt32(reader["Status"]),
                                    Role = reader["Role"] is DBNull ? String.Empty : reader["Role"].ToString(),
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

            return users;
        }

        public static int GetTotalEmployees()
        {
            int totalEmployees = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalEmployees", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        totalEmployees = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return totalEmployees;
        }



    }
}
