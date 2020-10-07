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
            await _dataProvider.Insert(new UserDb { Login = request.Login.ToLower(), Name = request.Name, Password = request.Password });
        }

        public bool IsUserExist(LoginRequest request)
        {
            var user = _dataProvider.Get<UserDb>(i => i.Login == request.Login).SingleOrDefault();

            if (user == null)
                return false;

            return true;
        }
    }
}