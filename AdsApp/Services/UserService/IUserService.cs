using AdsApp.DTO;
using AdsApp.Models.DTO;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public interface IUserService
    {
        Task Register(RegisterRequest request);
        bool Login(LoginRequest request);
        bool IsUserExist(RegisterRequest request);
    }
}