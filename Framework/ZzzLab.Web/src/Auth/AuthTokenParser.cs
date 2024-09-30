using ZzzLab.Crypt;

namespace ZzzLab.Web.Auth
{
    /// <summary>
    /// Access Token에 대한 파서
    /// </summary>
    public class AuthTokenParser
    {
        private const string _Header = "yyyyMMddHHmmssff";

        public string? Token { get; }
        public DateTime RegDate { protected set; get; }
        public string? Uuid { protected set; get; }
        public string? UserId { protected set; get; }
        public string? ClientIp { protected set; get; }
        public string? Message { protected set; get; }
        public bool Success => string.IsNullOrWhiteSpace(Message);

        private AuthTokenParser(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));
            if (token.ContainsIgnoreCase("|")) throw new InvalidArgumentException(nameof(token));

            this.Token = token;

            Parsing();
        }

        private string DecryptToken()
        {
            if (string.IsNullOrWhiteSpace(Configurator.CryptKey)) return Base64Crypt.FromBase64UrlSafe(Token);
            else return AESCrypt.Create(Configurator.CryptKey).DecryptUrlSafe(Token);
        }

        public static AuthTokenParser Decrypt(string? token)
            => new AuthTokenParser(token);

        protected void Parsing()
        {
            try
            {
                string token = DecryptToken();

                if (token.ContainsIgnoreCase('|') == false)
                {
                    Message = "token 값에 오류가 있습니다.";
                    return;
                }

                string[] tokenArr = token.Split('|');

                if (tokenArr.Length < 4)
                {
                    Message = "token 값에 오류가 있습니다.";
                    return;
                };

                this.RegDate = DateTime.ParseExact(tokenArr[0], _Header, null);
                this.Uuid = tokenArr[1];
                this.UserId = tokenArr[2];
                this.ClientIp = tokenArr[3];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.Message = ex.Message;
            }
        }

        public static string Encrypt(string uuid, string userId, string clientIp)
        {
            string tokenBase = $"{DateTime.Now.ToString(_Header)}|{uuid}|{userId}|{clientIp}";

            if (string.IsNullOrWhiteSpace(Configurator.CryptKey))
            {
                return Base64Crypt.ToBase64UrlSafe(tokenBase);
            }

            return AESCrypt.Create(Configurator.CryptKey).EncryptUrlSafe(tokenBase);
        }
    }
}