using System.Security.Claims;

namespace AdsApp.Models.ViewModels
{
    public class TokenDto
    {
        public ClaimsIdentity Identity { get; set; }
        public string Token { get; set; }
    }
}
