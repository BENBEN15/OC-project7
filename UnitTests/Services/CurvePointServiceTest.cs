namespace UnitTests.Services
{
    public class CurvePointServiceTest
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

        public Mock<IRepository<CurvePoint>> MockRepo()
        {
            return new Mock<IRepository<CurvePoint>>();
        }

        public CurvePointService initService(IRepository<CurvePoint> repo, IMapper mapper)
        {
            return new CurvePointService(repo, mapper);
        }

        [Fact]
        public void get()
        {
            //Arrange
            var mapper = mapperCreation();
            List<CurvePoint> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Get(data[0].CurvePointId)).Returns(data[0]);
            var service = initService(mockRepo.Object, mapper);

            //Act
            var result = service.Get(data[0].CurvePointId);

            //Assert
            var actualJson = JsonConvert.SerializeObject(result);
            var expected = mapper.Map<CurvePointDTO>(data[0]);
            var expectedJson = JsonConvert.SerializeObject(expected);

            Assert.NotNull(result);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var mapper = mapperCreation();
            List<CurvePoint> data = SeedData();
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
        public void save(CurvePointDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<CurvePoint> data = SeedData();
            var repoReturn = mapper.Map<CurvePoint>(dto);
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Save(It.IsAny<CurvePoint>())).Callback<CurvePoint>(x => data.Add(repoReturn)).Returns(repoReturn);
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
        public void update(CurvePointDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<CurvePoint> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Update(It.IsAny<CurvePoint>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            service.Update(dto);

            //Assert
            mockRepo.Verify(mock => mock.Update(It.IsAny<CurvePoint>()), Times.Once());
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var mapper = mapperCreation();
            List<CurvePoint> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Delete(It.IsAny<int>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            int idToDelete = data[0].CurvePointId;
            service.Delete(idToDelete);

            //Assert
            mockRepo.Verify(mock => mock.Delete(It.IsAny<int>()));
        }
    }
}
