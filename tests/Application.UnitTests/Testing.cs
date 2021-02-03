using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests
{
    [TestFixture]
    public class Testing
    {
        public Testing()
        {

        }

        public InMemoryAplicationDbContext Context { get; private set; }

        [SetUp]
        public void EnsureCreatedDatabase()
        {
            Context = new InMemoryAplicationDbContext();
            Context.Database.EnsureCreated();            
        }

        [TearDown]
        public void EnsureDeletedDatabase()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
