using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Integration.Data
{
    public class DatabaseInMemory
    {
        public DefaultContext Context { get; private set; }

        public DatabaseInMemory() 
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase("test")
                .Options;

            if (Context == null)
            {
                Context = new DefaultContext(options);
            }
        }

    }
}
