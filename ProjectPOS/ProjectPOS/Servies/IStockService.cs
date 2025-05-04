using ProjectPOS.Models;
using ProjectPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Data;
using ProjectPOS.Models.DTOs;

namespace ProjectPOS.Servies
{
    internal class IStockService
    {
        public static int GetNextInvoiceID()
        {
            int id = 1;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetNextInvoiceID", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LastID", id);
                        object result = cmd.ExecuteScalar();
                        if( result != null  && result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch( SqlException ex )
            {
                Console.WriteLine("SQL error: " + ex.Message);
            }
            catch( Exception ex )
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return id;
        }
        public static string ConvertDataGridViewToJSON(DataGridView dgv)
        {
            var productList = new List<ProductModel>();
            foreach(DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["ProductID"].Value != null && row.Cells["Quantity"].Value != null && row.Cells["UnitPrice"].Value != null && row.Cells["SupplierID"] != null)
                {
                    try
                    {
                        productList.Add(new ProductModel
                        {
                            ProductID = Convert.ToInt32(row.Cells["ProductID"].Value),
                            Quantity = Convert.ToInt32(row.Cells["Quantity"].Value),
                            SupplierID = Convert.ToInt32(row.Cells["SupplierID"].Value),
                            UnitPrice = Convert.ToDouble(row.Cells["UnitPrice"].Value)

                        });
                    }
                    catch
                    {
                        Console.WriteLine("Lỗi chuyển đổi dữ liệu");
                    }
                }
            }
            return JsonConvert.SerializeObject(productList);
        }
        public static void SubmitStockReceipt(int userid, decimal totalamount, DataGridView dgv)
        {
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertStockReceiptWithDetails_JSON", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userid);
                        cmd.Parameters.AddWithValue("@ToTalAmount", totalamount);
                        cmd.Parameters.AddWithValue("@ProductList", ConvertDataGridViewToJSON(dgv));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public static List<PurInvoiceDTO> GetListPurchaseInvoice(int pagenumber, int pagesize)
        {
            List<PurInvoiceDTO> lstPur = new List<PurInvoiceDTO>();

            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllStock", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageNumber", pagenumber);
                        cmd.Parameters.AddWithValue("@PageSize", pagesize);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstPur.Add(new PurInvoiceDTO
                                {
                                    ReceiptID = reader["ReceiptID"] is DBNull ? 0 : Convert.ToInt32(reader["ReceiptID"]),
                                    EmployeeName = reader["EmployeeName"] is DBNull ? string.Empty : reader["EmployeeName"].ToString(),
                                    TotalAmount = reader["TotalAmount"] is DBNull ? 0 : Convert.ToDecimal(reader["TotalAmount"]),
                                    ReceiptDate = reader["ReceiptDate"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["ReceiptDate"])
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
        public static int GetTotalPagePurchaseInvoice(int pagesize)
        {
            int totalPage = 0;
            try
            {
                using (var conn = new ConnectDatabase().GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetTotalPageStock", conn))
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
    }
}
