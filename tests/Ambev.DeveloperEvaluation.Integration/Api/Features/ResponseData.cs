using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Integration.Api.Features
{
    public class ResponseData
    {
        public User User { get; set; }
        public Product Product { get; set; }    
        public Branch Branch { get; set; }
    }
}
