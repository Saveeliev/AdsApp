using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Options
{
    public class AuthOptions
    {
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }
        public int LIFETIME { get; set; }

        const string KEY = "EbUjCRoQxbdya3TS";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}