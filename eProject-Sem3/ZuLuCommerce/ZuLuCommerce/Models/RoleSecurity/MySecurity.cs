using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZuLuCommerce.Models
{
    public class MySecurity
    {
        public static string EncryptPass(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] data = Encoding.UTF8.GetBytes(password);
            return BitConverter.ToString(sha.ComputeHash(data)).Replace("-", "").ToLower();
        }
    }
}