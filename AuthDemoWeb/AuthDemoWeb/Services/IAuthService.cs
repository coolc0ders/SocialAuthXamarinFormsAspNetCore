using AuthDemoWeb.Models;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace AuthDemoWeb.Services
{
    public interface IAuthService
    {
        Task<AppUser> Authenticate(UserTokenIdModel token);
    }
}
