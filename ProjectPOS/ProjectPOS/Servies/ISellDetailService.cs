using ProjectPOS.Models.DTOs;
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
    public class ISellDetailService
    {
        public static List<SellDetailDTO> GetListSellDetail(int orderID)
        {
            List<SellDetailDTO> lstPur = new List<SellDetailDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetOrderDetail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstPur.Add(new SellDetailDTO
                                {
                                    ProductName = reader["ProductName"] is DBNull ? string.Empty : reader["ProductName"].ToString(),
                                    Quantity = reader["Quantity"] is DBNull ? 0 : Convert.ToInt32(reader["Quantity"]),
                                    UnitPrice = reader["UnitPrice"] is DBNull ? 0 : Convert.ToDecimal(reader["UnitPrice"]),

                                });
                                //Console.WriteLine("ProductName: " + reader["ProductName"]);
                                //Console.WriteLine("Quantity: " + reader["Quantity"]);
                                //Console.WriteLine("UnitPrice: " + reader["Price"]);
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
    }
}
