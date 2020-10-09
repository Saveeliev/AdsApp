using System;

namespace AdsApp.Models.DTO
{
    public class LoginRequest
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}