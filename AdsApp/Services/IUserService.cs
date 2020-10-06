using AdsApp.DTO;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public interface IUserService
    {
        Task Register(RegisterRequest request);
    }
}
