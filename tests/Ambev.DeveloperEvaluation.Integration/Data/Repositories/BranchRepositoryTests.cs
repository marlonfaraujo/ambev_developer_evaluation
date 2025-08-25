using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Data.Repositories
{
    public class BranchRepositoryTests
    {
        [Fact(DisplayName = "Should create a branch successfully")]
        public async Task CreateAsync_ShouldAddBranch()
        {
            using var context = new DatabaseInMemory().Context;
            var repository = new BranchRepository(context);
            var branch = new Branch(Guid.NewGuid(), "Branch 1", "Description 1");

            var result = await repository.CreateAsync(branch);

            Assert.NotNull(await context.Branchs.FindAsync(branch.Id));
            Assert.Equal(branch.Name, result.Name);
        }

        [Fact(DisplayName = "Should return branch by id when it exists")]
        public async Task GetByIdAsync_ShouldReturnBranch_WhenExists()
        {
            using var context = new DatabaseInMemory().Context;
            var branch = new Branch(Guid.NewGuid(), "Branch 2", "Description 2");
            context.Branchs.Add(branch);
            context.SaveChanges();
            var repository = new BranchRepository(context);

            var result = await repository.GetByIdAsync(branch.Id);

            Assert.NotNull(result);
            Assert.Equal(branch.Id, result.Id);
        }

        [Fact(DisplayName = "Should return null when branch does not exist")]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            using var context = new DatabaseInMemory().Context;
            var repository = new BranchRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should delete branch when it exists")]
        public async Task DeleteAsync_ShouldRemoveBranch_WhenExists()
        {
            using var context = new DatabaseInMemory().Context;
            var branch = new Branch(Guid.NewGuid(), "Branch 3", "Description 3");
            context.Branchs.Add(branch);
            context.SaveChanges();
            var repository = new BranchRepository(context);

            var deleted = await repository.DeleteAsync(branch.Id);

            Assert.True(deleted);
            Assert.Null(await context.Branchs.FindAsync(branch.Id));
        }

        [Fact(DisplayName = "Should return false when deleting non-existent branch")]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            using var context = new DatabaseInMemory().Context;
            var repository = new BranchRepository(context);

            var deleted = await repository.DeleteAsync(Guid.NewGuid());

            Assert.False(deleted);
        }

        [Fact(DisplayName = "Should return equal when updating branch name")]
        public async Task UpdateAsync_ShouldReturnEqual_WhenBranchName()
        {
            using var context = new DatabaseInMemory().Context;
            var branch = new Branch(Guid.NewGuid(), "Branch 4", "Description 4");
            context.Branchs.Add(branch);
            context.SaveChanges();

            branch.Name = "Branch 4 Edited";
            var repository = new BranchRepository(context);
            var updated = await repository.UpdateAsync(branch);

            var current = await repository.GetByIdAsync(branch.Id);
            Assert.Equal(current.Name, updated.Name);
        }
    }
}
