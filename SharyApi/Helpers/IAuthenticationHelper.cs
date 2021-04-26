using SharyApi.Models;
using System;

namespace SharyApi.Helpers
{
    public interface IAuthenticationHelper
    {
        Tuple<string, string> HashPassword(string password);
        string HashPassword(string password, string salt);
        bool SlowEquals(string hashedPassword1, string hashedPassword2);
        bool AuthenticateBusiness(Credentials credentials);
        bool AuthenticateIndividual(Credentials credentials);
        bool AuthenticateStation(Credentials credentials);
        string GenerateJwt(Principal principal);
    }
}
