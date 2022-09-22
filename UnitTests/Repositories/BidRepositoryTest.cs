using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnitTests.Repositories
{
    public class BidRepositoryTest
    {
        public List<Bid> SeedData()
        {
            var items = new List<Bid>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<Bid>();
            var item2 = fixture.Create<Bid>();
            var item3 = fixture.Create<Bid>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public Mock<PoseidonDBContext> MockContext()
        {
            return new Mock<PoseidonDBContext>();
        }

        public IRepository<Bid> initRepo(Mock<PoseidonDBContext> context)
        {
            return new BidRepository(context.Object);
        }

        [Fact]
        public void get()
        {
            //Arrange
            List<Bid> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Bids).ReturnsDbSet(data);
            IRepository<Bid> repo = initRepo(context);

            //Act
            int id = data[0].BidId;
            var actual = repo.Get(id);

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson  = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.BidId == id));
            Assert.NotNull(actualJson);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            List<Bid> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Bids).ReturnsDbSet(data);
            IRepository<Bid> repo = initRepo(context);

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
        public void save(Bid obj)
        {
            //Arrange
            List<Bid> data = SeedData();
            var context = MockContext();
            context.Setup(m => m.Add(It.IsAny<Bid>())).Callback<Bid>(data.Add);
            IRepository<Bid> repo = initRepo(context);

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
        public void update(Bid obj)
        {
            //Arrange
            List<Bid> data = SeedData();
            var context = MockContext();
            var toUpdate = data[0];
            var update = obj;
            update.BidId = toUpdate.BidId;
            context.Setup(m => m.Bids.Update(It.IsAny<Bid>())).Callback(() => {
                data.Remove(toUpdate);
                data.Add(update);
            });
            IRepository<Bid> repo = initRepo(context);

            //Act
            repo.Update(update);

            //Assert
            var newObj = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.BidId == update.BidId));
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
            List<Bid> data = SeedData();
            var context = MockContext();
            var obj = data[0];
            context.Setup(x => x.Bids).ReturnsDbSet(data);
            context.Setup(m => m.Remove(It.IsAny<Bid>())).Callback<Bid>(x => data.Remove(obj));
            IRepository<Bid> repo = initRepo(context);

            //Act
            int id = obj.BidId;
            repo.Delete(id);

            //Assert
            var deleted = data.FirstOrDefault(x => x.BidId == id);
            Assert.Null(deleted);
            Assert.NotEmpty(data);
            Assert.Equal(2, data.Count());
        }
    }
}
