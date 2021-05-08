using BlazorCore.Interfaces;
using BlazorCore.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCore.Models
{
    public class LoginModel : IAccessModel
    {
        public LoginModel()
        {
            Accesses = new List<AccessModel>();
        }
        public UserModel User { get; set; }
        public List<AccessModel> Accesses { get; set; }
        public string RefreshToken { get; set; }

        public string GetAuthToken() => Accesses.FirstOrDefault()?.AccessToken;
        public string GetRefreshToken() => RefreshToken;
    }
}
