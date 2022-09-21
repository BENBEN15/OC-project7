using Newtonsoft.Json;

namespace UnitTests.Services
{
    public class TradeServiceTests
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

        public Mock<IRepository<Trade>> MockRepo()
        {
            return new Mock<IRepository<Trade>>();
        }

        public TradeService initService(IRepository<Trade> repo, IMapper mapper)
        {
            return new TradeService(repo, mapper);
        }

        [Fact]
        public void get()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Trade> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Get(data[0].TradeId)).Returns(data[0]);
            var service = initService(mockRepo.Object, mapper);

            //Act
            var result = service.Get(data[0].TradeId);

            //Assert
            var actualJson = JsonConvert.SerializeObject(result);
            var expected = mapper.Map<TradeDTO>(data[0]);
            var expectedJson = JsonConvert.SerializeObject(expected);

            Assert.NotNull(result);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Trade> data = SeedData();
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
        public void save(TradeDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<Trade> data = SeedData();
            var repoReturn = mapper.Map<Trade>(dto);
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Save(It.IsAny<Trade>())).Callback<Trade>(x => data.Add(repoReturn)).Returns(repoReturn);
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
        public void update(TradeDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<Trade> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Update(It.IsAny<Trade>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            service.Update(dto);

            //Assert
            mockRepo.Verify(mock => mock.Update(It.IsAny<Trade>()), Times.Once());
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Trade> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Delete(It.IsAny<int>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            int idToDelete = data[0].TradeId;
            service.Delete(idToDelete);

            //Assert
            mockRepo.Verify(mock => mock.Delete(It.IsAny<int>()));
        }
    }
}
