namespace BBox.Client
{
    public class AuthContext
    {
        public AuthContext(string user, string password, long? nonce)
        {
            User = user;
            Password = password;
            Nonce = nonce;
            Md5Password = Md5Helper.Hash(password);
        }
            
        public string User { get; }
        public long? Nonce { get; set; }
        public string Password { get; }
        public string Md5Password { get;  }
    }
}