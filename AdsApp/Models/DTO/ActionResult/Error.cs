namespace AdsApp.Models.DTO.ActionResult
{
    public class Error : ActionResult
    {
        public Error(string message)
        {
            Message = message;
        }
    }
}
