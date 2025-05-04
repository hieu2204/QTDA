using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Servies
{
    public class ICategoryService
    {
        public static List<CategoryModel> GetListCategory(int pagenumber, int pagesize)
        {
            List<CategoryModel> lstCat = new List<CategoryModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pagenumber);
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstCat.Add(new CategoryModel
                                {
                                    id = reader["CategoryID"] is DBNull ? 0 : Convert.ToInt32(reader["CategoryID"]),
                                    name = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString(),
                                    description = reader["Description"] is DBNull ? string.Empty : reader["Description"].ToString()
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
            return lstCat;
        }
        public static List<CategoryModel> GetListCategoryName()
        {
            List<CategoryModel> lstCat = new List<CategoryModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllCategoryName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstCat.Add(new CategoryModel
                                {
                                    id = reader["CategoryID"] is DBNull ? 0 : Convert.ToInt32(reader["CategoryID"]),
                                    name = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString()
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
            return lstCat;
        }
        public static int GetTotalPageCategory(int pagesize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPageCategory", conn))
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
        public static void UpdateCategory(int ID, string Name, string Description)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Description", Description);
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
        public static void InsertCategory(string name, string description)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Description", description);
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
        public static bool IsCategoryExists(string name, int categoryId)
        {
            bool exists = false;

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CheckCategoryExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                        exists = (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return exists;
        }
        public static List<CategoryModel> SearchCategoryByName(string keyword)
        {
            List<CategoryModel> categories = new List<CategoryModel>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SearchCategoryByName", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", keyword);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new CategoryModel
                                {
                                    id = reader["CategoryID"] is DBNull ? 0 : Convert.ToInt32(reader["CategoryID"]),
                                    name = reader["Name"].ToString(),
                                    description = reader["Description"] is DBNull ? string.Empty : reader["Description"].ToString()
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

            return categories;
        }

    }
}
