using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Water_Environment.Core
{
    public static class Extension
    {
        public static string NonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                        "đ",
                                        "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                        "í","ì","ỉ","ĩ","ị",
                                        "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                        "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                        "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                        "d",
                                        "e","e","e","e","e","e","e","e","e","e","e",
                                        "i","i","i","i","i",
                                        "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                        "u","u","u","u","u","u","u","u","u","u","u",
                                        "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Chuyển đổi chuỗi đầu vào thành mảng byte và tính toán mã MD5
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Tạo một StringBuilder để chứa các byte đã mã hóa
                StringBuilder sBuilder = new StringBuilder();

                // Lặp qua mỗi byte của mã MD5 và định dạng nó thành dạng hex
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}