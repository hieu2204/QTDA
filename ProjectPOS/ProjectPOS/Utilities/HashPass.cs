using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Utilities
{
    public class HashPass
    {
        public static string HashPassword(string password)
        {
            // Tạo đối tượng SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Tính toán hash từ chuỗi đầu vào, chuyển sang mảng byte
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển mảng byte thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
