using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharyApi.Data;
using SharyApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace SharyApi.Helpers
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        public AuthenticationHelper(IConfiguration configuration, IBusinessRepository businessRepository, IIndividualRepository individualRepository, IStationRepository stationRepository)
        {
            BusinessRepository = businessRepository;
            IndividualRepository = individualRepository;
            StationRepository = stationRepository;
            Configuration = configuration;
        }

        public IBusinessRepository BusinessRepository { get; }
        public IIndividualRepository IndividualRepository { get; }
        public IStationRepository StationRepository { get; }
        public IConfiguration Configuration { get; }

        public bool AuthenticateBusiness(Credentials credentials)
        {
            var principal = BusinessRepository.GetBusinessCredentialsByUsername(credentials.Username);
            if (principal == null)
                return false;
            return SlowEquals(principal.Password, HashPassword(credentials.Password, principal.Salt));
        }
        public bool AuthenticateIndividual(Credentials credentials)
        {
            var principal = IndividualRepository.GetIndividualCredentialsByUsername(credentials.Username);
            if (principal == null)
                return false;
            return SlowEquals(principal.Password, HashPassword(credentials.Password, principal.Salt));
        }

        public Tuple<string, string> HashPassword(string password)
        {
            var sBytes = new byte[256];
            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);
            var salt = Convert.ToBase64String(sBytes);
            var derivedBytes = new Rfc2898DeriveBytes(password, sBytes);
            return new Tuple<string, string>
            (
                Convert.ToBase64String(derivedBytes.GetBytes(256)),
                salt
            );
        }
        public string HashPassword(string password, string salt)
        {
            var sBytes = Convert.FromBase64String(salt);
            var derivedBytes = new Rfc2898DeriveBytes(password, sBytes);
            return Convert.ToBase64String(derivedBytes.GetBytes(256));
        }
        public bool SlowEquals(string hashedPassword1, string hashedPassword2)
        {
            byte[] a = Convert.FromBase64String(hashedPassword1);
            byte[] b = Convert.FromBase64String(hashedPassword2);
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
        public string GenerateJwt(Principal principal)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                                             principal.Id.ToString(),
                                             null,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool AuthenticateStation(Credentials credentials)
        {

            var principal = StationRepository.GetStationByUsername(credentials.Username);
            if (principal.HasNoValue)
                return false;
            return SlowEquals(principal.Value.Password, HashPassword(credentials.Password, principal.Value.Salt));
        }
    }
}
