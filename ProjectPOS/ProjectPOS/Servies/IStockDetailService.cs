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
    public class IStockDetailService
    {
        public static List<PurDetailDTO> GetListPurchaseDetail(int receiptid)
        {
            List<PurDetailDTO> lstPur = new List<PurDetailDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetListPurchaseDetail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ReceiptID", receiptid);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstPur.Add(new PurDetailDTO
                                {
                                    ProductName = reader["ProductName"] is DBNull ? string.Empty : reader["ProductName"].ToString(),
                                    CategoryName = reader["CategoryName"] is DBNull ? string.Empty : reader["CategoryName"].ToString(),
                                    SupplierName = reader["SupplierName"] is DBNull ? string.Empty : reader["SupplierName"].ToString(),
                                    Quantity = reader["Quantity"] is DBNull ? 0 : Convert.ToInt32(reader["Quantity"]),
                                    UnitPrice = reader["Price"] is DBNull ? 0 : Convert.ToDecimal(reader["Price"]),
                                    
                                });
                                Console.WriteLine("ProductName: " + reader["ProductName"]);
                                Console.WriteLine("Quantity: " + reader["Quantity"]);
                                Console.WriteLine("UnitPrice: " + reader["Price"]);
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
