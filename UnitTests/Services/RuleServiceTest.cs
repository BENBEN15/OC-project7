using Newtonsoft.Json;

namespace UnitTests.Services
{
    public class RuleServiceTest
    {
        private IMapper mapperCreation()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return mapper;
        }

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

        public Mock<IRepository<Rule>> MockRepo()
        {
            return new Mock<IRepository<Rule>>();
        }

        public RuleService initService(IRepository<Rule> repo, IMapper mapper)
        {
            return new RuleService(repo, mapper);
        }

        [Fact]
        public void get()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Rule> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Get(data[0].RuleId)).Returns(data[0]);
            var service = initService(mockRepo.Object, mapper);

            //Act
            var result = service.Get(data[0].RuleId);

            //Assert
            var actualJson = JsonConvert.SerializeObject(result);
            var expected = mapper.Map<RuleDTO>(data[0]);
            var expectedJson = JsonConvert.SerializeObject(expected);

            Assert.NotNull(result);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Rule> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.GetAll()).Returns(data);
            var service = initService(mockRepo.Object, mapper);

            //Act
            var result = service.GetAll();

            //Assert
            var actualJson = JsonConvert.SerializeObject(result);
            var expectedJson = JsonConvert.SerializeObject(data);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory, AutoData]
        public void save(RuleDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<Rule> data = SeedData();
            var repoReturn = mapper.Map<Rule>(dto);
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Save(It.IsAny<Rule>())).Callback<Rule>(x => data.Add(repoReturn)).Returns(repoReturn);
            var service = initService(mockRepo.Object, mapper);

            //Act
            var result = service.Save(dto);

            //Assert
            var actualJson = JsonConvert.SerializeObject(result);
            var expectedJson = JsonConvert.SerializeObject(dto);

            Assert.NotNull(result);
            Assert.NotEmpty(data);
            Assert.Equal(4, data.Count());
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory, AutoData]
        public void update(RuleDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<Rule> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Update(It.IsAny<Rule>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            service.Update(dto);

            //Assert
            mockRepo.Verify(mock => mock.Update(It.IsAny<Rule>()), Times.Once());
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Rule> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Delete(It.IsAny<int>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            int idToDelete = data[0].RuleId;
            service.Delete(idToDelete);

            //Assert
            mockRepo.Verify(mock => mock.Delete(It.IsAny<int>()));
        }
    }
}
