using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class CurvePointRepositoryTest
    {
        public List<CurvePoint> SeedData()
        {
            var items = new List<CurvePoint>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<CurvePoint>();
            var item2 = fixture.Create<CurvePoint>();
            var item3 = fixture.Create<CurvePoint>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public Mock<PoseidonDBContext> MockContext()
        {
            return new Mock<PoseidonDBContext>();
        }

        public IRepository<CurvePoint> initRepo(Mock<PoseidonDBContext> context)
        {
            return new CurvePointRepository(context.Object);
        }

        [Fact]
        public void get()
        {
            //Arrange
            List<CurvePoint> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.CurvePoints).ReturnsDbSet(data);
            IRepository<CurvePoint> repo = initRepo(context);

            //Act
            int id = data[0].CurvePointId;
            var actual = repo.Get(id);

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.CurvePointId == id));
            Assert.NotNull(actualJson);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            List<CurvePoint> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.CurvePoints).ReturnsDbSet(data);
            IRepository<CurvePoint> repo = initRepo(context);

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
        public void save(CurvePoint obj)
        {
            //Arrange
            List<CurvePoint> data = SeedData();
            var context = MockContext();
            context.Setup(m => m.Add(It.IsAny<CurvePoint>())).Callback<CurvePoint>(data.Add);
            IRepository<CurvePoint> repo = initRepo(context);

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
        public void update(CurvePoint obj)
        {
            //Arrange
            List<CurvePoint> data = SeedData();
            var context = MockContext();
            var toUpdate = data[0];
            var update = obj;
            update.CurvePointId = toUpdate.CurvePointId;
            context.Setup(m => m.CurvePoints.Update(It.IsAny<CurvePoint>())).Callback(() => {
                data.Remove(toUpdate);
                data.Add(update);
            });
            IRepository<CurvePoint> repo = initRepo(context);

            //Act
            repo.Update(update);

            //Assert
            var newObj = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.CurvePointId == update.CurvePointId));
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
            List<CurvePoint> data = SeedData();
            var context = MockContext();
            var obj = data[0];
            context.Setup(x => x.CurvePoints).ReturnsDbSet(data);
            context.Setup(m => m.Remove(It.IsAny<CurvePoint>())).Callback<CurvePoint>(x => data.Remove(obj));
            IRepository<CurvePoint> repo = initRepo(context);

            //Act
            int id = obj.CurvePointId;
            repo.Delete(id);

            //Assert
            var deleted = data.FirstOrDefault(x => x.CurvePointId == id);
            Assert.Null(deleted);
            Assert.NotEmpty(data);
            Assert.Equal(2, data.Count());
        }
    }
}
