using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class RatingRepositoryTest
    {
        public List<Rating> SeedData()
        {
            var items = new List<Rating>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<Rating>();
            var item2 = fixture.Create<Rating>();
            var item3 = fixture.Create<Rating>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public Mock<PoseidonDBContext> MockContext()
        {
            return new Mock<PoseidonDBContext>();
        }

        public IRepository<Rating> initRepo(Mock<PoseidonDBContext> context)
        {
            return new RatingRepository(context.Object);
        }

        [Fact]
        public void get()
        {
            //Arrange
            List<Rating> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Ratings).ReturnsDbSet(data);
            IRepository<Rating> repo = initRepo(context);

            //Act
            int id = data[0].RatingId;
            var actual = repo.Get(id);

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.RatingId == id));
            Assert.NotNull(actualJson);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            List<Rating> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Ratings).ReturnsDbSet(data);
            IRepository<Rating> repo = initRepo(context);

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
        public void save(Rating obj)
        {
            //Arrange
            List<Rating> data = SeedData();
            var context = MockContext();
            context.Setup(m => m.Add(It.IsAny<Rating>())).Callback<Rating>(data.Add);
            IRepository<Rating> repo = initRepo(context);

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
        public void update(Rating obj)
        {
            //Arrange
            List<Rating> data = SeedData();
            var context = MockContext();
            var toUpdate = data[0];
            var update = obj;
            update.RatingId = toUpdate.RatingId;
            context.Setup(m => m.Ratings.Update(It.IsAny<Rating>())).Callback(() => {
                data.Remove(toUpdate);
                data.Add(update);
            });
            IRepository<Rating> repo = initRepo(context);

            //Act
            repo.Update(update);

            //Assert
            var newObj = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.RatingId == update.RatingId));
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
            List<Rating> data = SeedData();
            var context = MockContext();
            var obj = data[0];
            context.Setup(x => x.Ratings).ReturnsDbSet(data);
            context.Setup(m => m.Remove(It.IsAny<Rating>())).Callback<Rating>(x => data.Remove(obj));
            IRepository<Rating> repo = initRepo(context);

            //Act
            int id = obj.RatingId;
            repo.Delete(id);

            //Assert
            var deleted = data.FirstOrDefault(x => x.RatingId == id);
            Assert.Null(deleted);
            Assert.NotEmpty(data);
            Assert.Equal(2, data.Count());
        }
    }
}
