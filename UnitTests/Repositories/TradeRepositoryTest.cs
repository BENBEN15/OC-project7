using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class TradeRepositoryTest
    {
        public List<Trade> SeedData()
        {
            var items = new List<Trade>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<Trade>();
            var item2 = fixture.Create<Trade>();
            var item3 = fixture.Create<Trade>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public Mock<PoseidonDBContext> MockContext()
        {
            return new Mock<PoseidonDBContext>();
        }

        public IRepository<Trade> initRepo(Mock<PoseidonDBContext> context)
        {
            return new TradeRepository(context.Object);
        }

        [Fact]
        public void get()
        {
            //Arrange
            List<Trade> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Trades).ReturnsDbSet(data);
            IRepository<Trade> repo = initRepo(context);

            //Act
            int id = data[0].TradeId;
            var actual = repo.Get(id);

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.TradeId == id));
            Assert.NotNull(actualJson);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            List<Trade> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Trades).ReturnsDbSet(data);
            IRepository<Trade> repo = initRepo(context);

            //Act
            var actual = repo.GetAll();

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(data);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory, AutoData]
        public void save(Trade obj)
        {
            //Arrange
            List<Trade> data = SeedData();
            var context = MockContext();
            context.Setup(m => m.Add(It.IsAny<Trade>())).Callback<Trade>(data.Add);
            IRepository<Trade> repo = initRepo(context);

            //Act
            var actual = repo.Save(obj);

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(obj);
            Assert.NotNull(actual);
            Assert.NotEmpty(data);
            Assert.Equal(4, data.Count());
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory, AutoData]
        public void update(Trade obj)
        {
            //Arrange
            List<Trade> data = SeedData();
            var context = MockContext();
            var toUpdate = data[0];
            var update = obj;
            update.TradeId = toUpdate.TradeId;
            context.Setup(m => m.Trades.Update(It.IsAny<Trade>())).Callback(() => {
                data.Remove(toUpdate);
                data.Add(update);
            });
            IRepository<Trade> repo = initRepo(context);

            //Act
            repo.Update(update);

            //Assert
            var newObj = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.TradeId == update.TradeId));
            var oldObj = JsonConvert.SerializeObject(toUpdate);
            var expectedJson = JsonConvert.SerializeObject(update);

            Assert.NotNull(data);
            Assert.NotEmpty(data);
            Assert.Equal(3, data.Count());
            Assert.NotNull(newObj);
            Assert.Equal(expectedJson, newObj);
            Assert.NotEqual(oldObj, newObj);
        }

        [Fact]
        public void delete()
        {
            //Arrange
            List<Trade> data = SeedData();
            var context = MockContext();
            var obj = data[0];
            context.Setup(x => x.Trades).ReturnsDbSet(data);
            context.Setup(m => m.Remove(It.IsAny<Trade>())).Callback<Trade>(x => data.Remove(obj));
            IRepository<Trade> repo = initRepo(context);

            //Act
            int id = obj.TradeId;
            repo.Delete(id);

            //Assert
            var deleted = data.FirstOrDefault(x => x.TradeId == id);
            Assert.Null(deleted);
            Assert.NotEmpty(data);
            Assert.Equal(2, data.Count());
        }
    }
}
