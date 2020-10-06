namespace AdsApp.DTO
{
    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}