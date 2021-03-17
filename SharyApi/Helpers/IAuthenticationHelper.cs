using SharyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Helpers
{
    public interface IAuthenticationHelper
    {
        Tuple<string, string> HashPassword(string password);
        string HashPassword(string password,string salt);
        bool SlowEquals(string hashedPassword1, string hashedPassword2);
        bool AuthenticateBusiness(Credentials credentials);
        public bool AuthenticateIndividual(Credentials credentials);
        public string GenerateJwt(Principal principal);
    }
}
