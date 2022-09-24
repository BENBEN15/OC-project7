﻿namespace UnitTests.Repositories
{
    public class RuleRepositoryTest
    {
        public List<Rule> SeedData()
        {
            var items = new List<Rule>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<Rule>();
            var item2 = fixture.Create<Rule>();
            var item3 = fixture.Create<Rule>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public Mock<PoseidonDBContext> MockContext()
        {
            return new Mock<PoseidonDBContext>();
        }

        public IRepository<Rule> initRepo(Mock<PoseidonDBContext> context)
        {
            return new RuleRepository(context.Object);
        }

        [Fact]
        public void get()
        {
            //Arrange
            List<Rule> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Rules).ReturnsDbSet(data);
            IRepository<Rule> repo = initRepo(context);

            //Act
            int id = data[0].RuleId;
            var actual = repo.Get(id);

            //Assert
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.RuleId == id));
            Assert.NotNull(actualJson);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            List<Rule> data = SeedData();
            var context = MockContext();
            context.Setup(x => x.Rules).ReturnsDbSet(data);
            IRepository<Rule> repo = initRepo(context);

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
        public void save(Rule obj)
        {
            //Arrange
            List<Rule> data = SeedData();
            var context = MockContext();
            context.Setup(m => m.Add(It.IsAny<Rule>())).Callback<Rule>(data.Add);
            IRepository<Rule> repo = initRepo(context);

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
        public void update(Rule obj)
        {
            //Arrange
            List<Rule> data = SeedData();
            var context = MockContext();
            var toUpdate = data[0];
            var update = obj;
            update.RuleId = toUpdate.RuleId;
            context.Setup(m => m.Rules.Update(It.IsAny<Rule>())).Callback(() => {
                data.Remove(toUpdate);
                data.Add(update);
            });
            IRepository<Rule> repo = initRepo(context);

            //Act
            repo.Update(update);

            //Assert
            var newObj = JsonConvert.SerializeObject(data.FirstOrDefault(x => x.RuleId == update.RuleId));
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
            List<Rule> data = SeedData();
            var context = MockContext();
            var obj = data[0];
            context.Setup(x => x.Rules).ReturnsDbSet(data);
            context.Setup(m => m.Remove(It.IsAny<Rule>())).Callback<Rule>(x => data.Remove(obj));
            IRepository<Rule> repo = initRepo(context);

            //Act
            int id = obj.RuleId;
            repo.Delete(id);

            //Assert
            var deleted = data.FirstOrDefault(x => x.RuleId == id);
            Assert.Null(deleted);
            Assert.NotEmpty(data);
            Assert.Equal(2, data.Count());
        }
    }
}
