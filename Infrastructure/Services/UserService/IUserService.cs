using DTO.ActionResult;
using DTO.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task Register(RegisterRequest request);
        LoginResult Login(LoginRequest request);
    }
}