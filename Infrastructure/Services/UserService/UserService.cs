using AdsApp;
using DataBase.Models;
using DTO.ActionResult;
using DTO.Request;
using Infrastructure.Helpers.TokenHelper;
using Infrastructure.Options;
using Infrastructure.Services.DataProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IDataProvider _dataProvider;
        private readonly ITokenHelper _tokenHelper;

        public UserService(IDataProvider dataProvider, ITokenHelper tokenHelper)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _tokenHelper = tokenHelper ?? throw new ArgumentNullException(nameof(tokenHelper));
        }

        public async Task Register(RegisterRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var transaction = _dataProvider.CreateTransaction(IsolationLevel.Serializable);

            var lowerLogin = request.Login.ToLower();

            var user = _dataProvider.Get<UserDb>(i => i.Login == lowerLogin).SingleOrDefault();
           
            if (user != null)
                throw new Exception("User is already exist");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var userDb = new UserDb
            {
                Login = lowerLogin,
                Name = request.Name,
                Password = hash
            };

            await _dataProvider.Insert(userDb);

            transaction.Commit();
        }

        public LoginResult Login(LoginRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var lowerLogin = request.Login.ToLower();

            var user = _dataProvider.Get<UserDb>(i => i.Login == lowerLogin).SingleOrDefault();

            if (user == null)
                return null;

            var varify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!varify)
                return null;

            var token = _tokenHelper.GenerateToken(user.Id);

            return new LoginResult { Token = token };
        }
    }
}