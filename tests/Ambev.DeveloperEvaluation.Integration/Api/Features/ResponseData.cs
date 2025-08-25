using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features
{
    public class ResponseData
    {
        public ResponseData()
        {
            User = new User();
            Product = new Product();
            Branch = new Branch();
            AuthUser = new AuthenticateUserResponse();
        }

        public User User { get; set; }
        public Product Product { get; set; }    
        public Branch Branch { get; set; }
        public AuthenticateUserResponse AuthUser { get; set; }
    }
}
