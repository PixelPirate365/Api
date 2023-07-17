using System.Security.Cryptography;
using System.Text;

namespace web_api.Extensions {
    public static class StringExts {
        public static string Hash(this string str) {
            //Create the hash to a byte array 

            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(str));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
