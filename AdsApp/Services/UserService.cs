using AdsApp.DTO;
using AdsApp.Models;
using AdsApp.Services;
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
            await _dataProvider.Insert(new UserDb { Login = request.Login, Name = request.Name, Password = request.Password });
        }
    }
}