using System.Security.Cryptography;
using System.Text;

namespace BBox.Client
{
    public static class Md5Helper
    {
        public static string Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
 
            var sb = new StringBuilder();
            foreach (var t in hash)
                sb.Append(t.ToString("X2"));
            return sb.ToString().ToLower();
        }
    }
}