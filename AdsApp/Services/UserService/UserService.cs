using AdsApp.DTO;
using AdsApp.Models;
using AdsApp.Models.DTO;
using AdsApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AdsApp
{
    public class UserService : IUserService
    {
        private readonly IDataProvider _dataProvider;

        public UserService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new System.ArgumentNullException(nameof(dataProvider));
        }

        public async Task Register(RegisterRequest request)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _dataProvider.Insert(new UserDb { Login = request.Login.ToLower(), Name = request.Name, Password = hash });
        }

        public bool IsCorrectPassword(LoginRequest request)
        {
            var user = _dataProvider.Get<UserDb>(i => i.Login == request.Login).SingleOrDefault();
            var varify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (varify == false)
                return false;

            return true;
        }
    }
}