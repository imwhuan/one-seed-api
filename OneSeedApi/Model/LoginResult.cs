using LibFrame.DTOModel;

namespace OneSeedApi.Model
{
    public class LoginResult
    {
        public string Token { get; set; } = "";
        public UserAccountInfo? User { get; set; }
    }
}
