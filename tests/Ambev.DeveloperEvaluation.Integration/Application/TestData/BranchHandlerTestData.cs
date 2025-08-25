using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Data;
using Ambev.DeveloperEvaluation.ORM.Repositories;

namespace Ambev.DeveloperEvaluation.Integration.Application.TestData
{
    public static class BranchHandlerTestData
    {
        public static Branch GetBranch()
        {
            using var context = new DatabaseInMemory().Context;
            var repository = new BranchRepository(context);
            var branch = new Branch(Guid.NewGuid(), "Branch 1", "Description 1");

            var result = repository.CreateAsync(branch).Result;
            return result;
        }
    }
}
