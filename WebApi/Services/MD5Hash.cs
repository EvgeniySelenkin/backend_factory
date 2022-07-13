using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;

namespace WebApi.Services
{
    public class MD5Hash
    {
        public string source { get; set; }

        public MD5Hash(string input)
        {
            source = input;
        }

        public string GetMd5Hash()
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public bool VerifyMd5Hash(string hash)
        {
            MD5 md5Hash = MD5.Create();
            string hashOfInput = GetMd5Hash();
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }
    }
}
