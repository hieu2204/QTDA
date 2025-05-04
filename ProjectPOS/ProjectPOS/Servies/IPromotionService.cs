using ProjectPOS.Models.DTOs;
using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing.Printing;

namespace ProjectPOS.Servies
{
    public class IPromotionService
    {
        public static void AddPromotion(string Name,string Description,string DiscountType,DateTime StartDate,DateTime EndDate,
            int? ProductID,int? MinQuantity,int? AppliesToOrder,int? GiftProductID,int? GiftQuantity,decimal? MinBillAmount,decimal? DiscountValue
            )
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertPromotion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@DiscountType", DiscountType);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@ProductID", ProductID);
                        cmd.Parameters.AddWithValue("@MinQuantity", MinQuantity);
                        cmd.Parameters.AddWithValue("@AppliesToOrder", AppliesToOrder);
                        cmd.Parameters.AddWithValue("@GiftProductID", GiftProductID);
                        cmd.Parameters.AddWithValue("@GiftQuantity", GiftQuantity);
                        cmd.Parameters.AddWithValue("@MinBillAmount", MinBillAmount);
                        cmd.Parameters.AddWithValue("@DiscountValue", DiscountValue);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Sql error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static List<PromotionModel> GetPromotionList(int pageNumber,int pageSize)
        {
            List<PromotionModel> promotions = new List<PromotionModel>();
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetAllPromotion", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                promotions.Add(new PromotionModel
                                {
                                    PromotionID = reader["PromotionID"] is DBNull ? 0 : Convert.ToInt32(reader["PromotionID"]),
                                    PromotionName = reader["PromotionName"] is DBNull ? string.Empty : reader["PromotionName"].ToString(),
                                    DiscountType = reader["DiscountType"] is DBNull ? string.Empty : reader["DiscountType"].ToString(),
                                    StartDate = reader["StartDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["StartDate"]),
                                    EndDate = reader["EndDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["EndDate"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Sql error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return promotions;
        }
        public static List<PromotionDTO> GetActivePromotions()
        {
            List<PromotionDTO> promotions = new List<PromotionDTO>();
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetActivePromotions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                promotions.Add(new PromotionDTO
                                {
                                    PromotionID = reader["PromotionID"] is DBNull ? 0 : Convert.ToInt32(reader["PromotionID"]),
                                    Name = reader["PromotionName"] is DBNull ? string.Empty : reader["PromotionName"].ToString(),
                                    Description = reader["Description"] is DBNull ? string.Empty : reader["Description"].ToString(),
                                    DiscountType = reader["DiscountType"] is DBNull ? string.Empty : reader["DiscountType"].ToString(),
                                    DiscountValue = reader["DiscountValue"] is DBNull ? (decimal?)null : Convert.ToDecimal(reader["DiscountValue"]),
                                    StartDate = reader["StartDate"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["StartDate"]),
                                    EndDate = reader["EndDate"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["EndDate"]),
                                    MinTotalAmount = reader["MinBillAmount"] is DBNull ? (decimal?)null : Convert.ToDecimal(reader["MinBillAmount"]),
                                    ProductID = reader["ProductID"] is DBNull ? (int?)null : Convert.ToInt32(reader["ProductID"]),
                                    MinQuantity = reader["MinQuantity"] is DBNull ? (int?)null : Convert.ToInt32(reader["MinQuantity"]),
                                    AppliesToOrder = reader["AppliesToOrder"] is DBNull ? 1 : Convert.ToInt32(reader["AppliesToOrder"]),
                                    GiftProductID = reader["GiftProductID"] is DBNull ? (int?)null : Convert.ToInt32(reader["GiftProductID"]),
                                    GiftQuantity = reader["GiftQuantity"] is DBNull ? (int?)null : Convert.ToInt32(reader["GiftQuantity"])
                                });
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
            return promotions;
        }
        public static int GetTotalPage (int pageSize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPagePromotion", conn))
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
    }
}
