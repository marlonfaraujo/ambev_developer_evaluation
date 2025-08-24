using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Data.Repositories
{
    public class BranchRepositoryTests
    {
        private DefaultContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new DefaultContext(options);
        }

        [Fact(DisplayName = "Should create a branch successfully")]
        public async Task CreateAsync_ShouldAddBranch()
        {
            using var context = CreateContext();
            var repository = new BranchRepository(context);
            var branch = new Branch(Guid.NewGuid(), "Branch 1", "Description 1");

            var result = await repository.CreateAsync(branch);

            Assert.NotNull(await context.Branchs.FindAsync(branch.Id));
            Assert.Equal(branch.Name, result.Name);
        }

        [Fact(DisplayName = "Should return branch by id when it exists")]
        public async Task GetByIdAsync_ShouldReturnBranch_WhenExists()
        {
            using var context = CreateContext();
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
            using var context = CreateContext();
            var repository = new BranchRepository(context);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should delete branch when it exists")]
        public async Task DeleteAsync_ShouldRemoveBranch_WhenExists()
        {
            using var context = CreateContext();
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
            using var context = CreateContext();
            var repository = new BranchRepository(context);

            var deleted = await repository.DeleteAsync(Guid.NewGuid());

            Assert.False(deleted);
        }

        [Fact(DisplayName = "Should throw NotImplementedException on update")]
        public async Task UpdateAsync_ShouldThrowNotImplementedException()
        {
            using var context = CreateContext();
            var repository = new BranchRepository(context);
            var branch = new Branch(Guid.NewGuid(), "Branch 4", "Description 4");

            await Assert.ThrowsAsync<NotImplementedException>(() => repository.UpdateAsync(branch));
        }
    }
}
