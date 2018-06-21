namespace ApiInstructions.DataInstructions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ApiInstructions.BaseEntities.Entities.Interfaces;
    using ApiInstructions.DataInstructions.Instructions;
    using ApiInstructions.DataInstructions.Tests.Exceptions;
    using ApiInstructions.DataInstructions.Tests.Mocks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;   
    using Moq;

    [TestClass]
    public class InstructionsTests
    {
        Mock<DbContextMock> dbContextMock = null;
        IDictionary<string, byte[]> rowVersions = new Dictionary<string, byte[]>();
        IList<CountryEntityMock> countryList = new List<CountryEntityMock>();

        [TestInitialize]
        public void Start()
        {
            Random rnd = new Random();
            for (var i = 0; i < 10; i++)
            {
                var rowVersion = UTF8Encoding.ASCII.GetBytes((i + 1).ToString());
                var country = new CountryEntityMock
                {
                    Id = (i + 1).ToString(),
                    Name = "Country " + i.ToString(),
                    Description = "Description " + i.ToString(),
                    RowVersion = rowVersion
                };

                rowVersions.Add((i + 1).ToString(), rowVersion);
                this.countryList.Add(country);
            }

            // TODO: Use neutral entity (not from DataWorkShop)
            var countriesDbSet = GetQueryableMockDbSet<CountryEntityMock, string>(this.countryList);
            this.dbContextMock = new Mock<DbContextMock>();
            this.dbContextMock.Setup(x => x.Countries).Returns(countriesDbSet);
            this.dbContextMock.Setup(x => x.Set<CountryEntityMock>()).Returns(countriesDbSet);
            this.dbContextMock.Setup(x => x.Remove(It.IsAny<CountryEntityMock>())).Callback<CountryEntityMock>((s) => {
                var initialRowVersion = this.rowVersions[s.Id];

                if (!initialRowVersion.SequenceEqual(s.RowVersion))
                {
                    throw new DbUpdateConcurrencyTestException();
                }

                countriesDbSet.Remove(s);
            });
        }

        [TestMethod]
        public async Task RemoveEntityByIdSuccessfullTest()
        {
            var context = this.dbContextMock.Object;
            Assert.AreEqual(context.Countries.Count(), 10);
            await new RemovalInstruction<CountryEntityMock, string>(context, "3", "Mw==").Execute();
            Assert.AreEqual(context.Countries.Count(), 9);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyTestException))]
        public async Task RemoveEntityByIdWronRowVersionTest()
        {
            var context = this.dbContextMock.Object;
            Assert.AreEqual(context.Countries.Count(), 10);
            await new RemovalInstruction<CountryEntityMock, string>(context, "3", "Dw==").Execute();
        }

        private static DbSet<TEntity> GetQueryableMockDbSet<TEntity, TId>(IList<TEntity> sourceList)
            where TEntity: class, IEntityWithId<TId>, new()
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<TEntity>>();
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>((s) => sourceList.Add(s));
            dbSet.Setup(d => d.Remove(It.IsAny<TEntity>())).Callback<TEntity>((s) => sourceList.Remove(s));
            dbSet.Setup(d => d.Attach(It.IsAny<TEntity>())).Callback<TEntity>((s) =>
            {
                var index = sourceList.IndexOf(sourceList.FirstOrDefault(x => x.Id.Equals(s.Id)));
                sourceList[index] = s;
            });

            return dbSet.Object;
        }
    }
}
