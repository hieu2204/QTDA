using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS.Utilities
{
    public class FormatNumber
    {
        public static void FormatAsNumber(Guna2TextBox txt)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
                return;

            // Lưu vị trí con trỏ
            int selectionStart = txt.SelectionStart;
            int lengthBefore = txt.Text.Length;

            // Loại bỏ dấu phân cách hàng nghìn để có thể parse đúng
            string rawText = txt.Text.Replace(".", "").Replace(",", "");

            if (decimal.TryParse(rawText, out decimal number))
            {
                string formatted = number.ToString("N0", CultureInfo.GetCultureInfo("en-US"));

                if (txt.Text != formatted)
                {
                    txt.Text = formatted;

                    // Điều chỉnh lại vị trí con trỏ sau khi thay đổi text
                    int lengthAfter = formatted.Length;
                    int diff = lengthAfter - lengthBefore;
                    txt.SelectionStart = Math.Max(0, selectionStart + diff);
                }
            }
        }
        public static string FormatCurrency(string input)
        {
            return decimal.TryParse(input, out decimal value)
                ? value.ToString("N0")
                : "0";
        }
    }
}
