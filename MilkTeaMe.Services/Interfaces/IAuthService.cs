﻿using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Employee?> Login(string username, string password);
    }
}
