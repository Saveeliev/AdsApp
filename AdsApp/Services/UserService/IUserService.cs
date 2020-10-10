using AdsApp.DTO;
using AdsApp.Models.DTO;
using AdsApp.Models.DTO.ActionResult;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdsApp.Services
{
    public interface IUserService
    {
        Task<IActionResult> Register(RegisterRequest request);
        LoginResult Login(LoginRequest request);
    }
}