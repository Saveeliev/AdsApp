using System;

namespace DTO.Request
{
    public class LoginRequest
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}