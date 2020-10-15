namespace DTO.Request
{
    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}