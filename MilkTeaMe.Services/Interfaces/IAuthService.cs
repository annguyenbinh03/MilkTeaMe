﻿using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> Login(string username, string password);
        Task<User> GetOrCreateAccountByEmail(string email);
        Task<User> Register(UserModel model);
        Task SendPasswordResetLink(string email);
	}
}
