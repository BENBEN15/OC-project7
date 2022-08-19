using Newtonsoft.Json;

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

        private List<CurvePoint> SeedData()
        {
            var curvepoints = new List<CurvePoint>{
            new CurvePoint
            {
                CurvePointId = 1,
                CurveId = 2,
                AsOfDate = new DateTime(2022,01,01),
                CreationDate = new DateTime(2022,01,01),
                Term = 1.0,
                Value = 1.0,
            },
            new CurvePoint
            {
                CurvePointId = 2,
                CurveId = 3,
                AsOfDate = new DateTime(2022,02,02),
                CreationDate = new DateTime(2022,02,02),
                Term = 2.0,
                Value = 2.0,
            },
            new CurvePoint
            {
                CurvePointId = 3,
                CurveId = 4,
                AsOfDate = new DateTime(2022,03,03),
                CreationDate = new DateTime(2022,03,03),
                Term = 3.0,
                Value = 3.0,
            },
        };

            return curvepoints;
        }

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            var mapper = mapperCreation();
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);
            CurvePointService curvePointService = new CurvePointService(curvePointRepository, mapper);

            //Act
            int id = 1;
            var curvePointResult = JsonConvert.SerializeObject(curvePointService.Get(id));
            var curvePointDto = mapper.Map<CurvePointDTO>(SeedData().FirstOrDefault(x => x.CurvePointId == id));
            var curvePoint = JsonConvert.SerializeObject(curvePointDto);

            //Assert
            Assert.NotNull(curvePointResult);
            Assert.Equal(curvePoint, curvePointResult);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            var mapper = mapperCreation();
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);
            CurvePointService curvePointService = new CurvePointService(curvePointRepository, mapper);

            //Act
            var curvePointResult = JsonConvert.SerializeObject(curvePointService.GetAll());
            var dtoList = new List<CurvePointDTO>();
            var curvePointsDB = contextMock.Object.CurvePoints.Where(b => b.CurvePointId > 0);
            foreach (CurvePoint curvePoint in curvePointsDB)
            {
                var dto = mapper.Map<CurvePointDTO>(curvePoint);
                dtoList.Add(dto);
            }
            var curvePointlist = JsonConvert.SerializeObject(dtoList);

            //Assert
            Assert.NotNull(curvePointResult);
            Assert.NotEmpty(curvePointResult);
            Assert.Equal(curvePointlist, curvePointResult);
        }

        [Fact]
        public void save()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            contextMock.Setup(m => m.Add(It.IsAny<CurvePoint>())).Callback<CurvePoint>(curvePoints.Add);
            var mapper = mapperCreation();
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);
            CurvePointService curvePointService = new CurvePointService(curvePointRepository, mapper);

            //Act
            int idToGet = 4;
            CurvePointDTO curvePointToAdd = new CurvePointDTO
            {
                CurvePointId = idToGet,
                CurveId = 5,
                AsOfDate = new DateTime(2022, 04, 04),
                CreationDate = new DateTime(2022, 04, 04),
                Term = 4.0,
                Value = 4.0,
            };

            CurvePointDTO curvePointResponse = curvePointService.Save(curvePointToAdd);
            CurvePointDTO curvePointResult = mapper.Map<CurvePointDTO>(contextMock.Object.CurvePoints.FirstOrDefault(x => x.CurvePointId == idToGet));

            IEnumerable<CurvePoint> curvePointlist = contextMock.Object.CurvePoints.Where(b => b.CurvePointId > 0);

            var curvePointToAddJson = JsonConvert.SerializeObject(curvePointToAdd);
            var curvePointResponseJson = JsonConvert.SerializeObject(curvePointResponse);
            var curvePointsResultJson = JsonConvert.SerializeObject(curvePointResult);

            //Assert
            Assert.NotNull(curvePointResult);
            Assert.NotEmpty(curvePointlist);
            Assert.Equal(4, curvePointlist.Count());
            Assert.Equal(curvePointToAddJson, curvePointsResultJson);
            Assert.Equal(curvePointsResultJson, curvePointResponseJson);
        }

        [Fact]
        public void update()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PoseidonDBContext>()
                .UseInMemoryDatabase("BidRepoUpdate" + Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;

            using (var context = new PoseidonDBContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                foreach (var b in SeedData())
                {
                    context.CurvePoints.Add(b);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                var mapper = mapperCreation();
                CurvePointRepository curvePointbidRepository = new CurvePointRepository(context);
                CurvePointService curvePointService = new CurvePointService(curvePointbidRepository, mapper);

                CurvePointDTO curvePointToUpdate = new CurvePointDTO
                {
                    CurvePointId = 1,
                    CurveId = 3,
                    AsOfDate = new DateTime(2023, 01, 01),
                    CreationDate = new DateTime(2023, 01, 01),
                    Term = 1.5,
                    Value = 1.5,
                };

                //Act
                curvePointService.Update(curvePointToUpdate);
                CurvePointDTO curvePointsResult = mapper.Map<CurvePointDTO>(context.CurvePoints.FirstOrDefault(x => x.CurvePointId == curvePointToUpdate.CurvePointId));
                var curvePointToUpdateJson = JsonConvert.SerializeObject(curvePointToUpdate);
                var curvePointResultJson = JsonConvert.SerializeObject(curvePointsResult);

                //Assert
                Assert.NotNull(curvePointsResult);
                Assert.Equal(curvePointToUpdateJson, curvePointResultJson);
            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            contextMock.Setup(m => m.Remove(It.IsAny<CurvePoint>())).Callback<CurvePoint>(b => curvePoints.Remove(b));
            var mapper = mapperCreation();
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);
            CurvePointService curvePointService = new CurvePointService(curvePointRepository, mapper);

            //Act
            int idToDelete = 1;
            curvePointService.Delete(idToDelete);

            CurvePoint curvePointResult = contextMock.Object.CurvePoints.FirstOrDefault(x => x.CurvePointId == idToDelete);
            IEnumerable<CurvePoint> curvePointlist = contextMock.Object.CurvePoints.Where(x => x.CurvePointId > 0);

            //Assert
            Assert.Null(curvePointResult);
            Assert.NotEmpty(curvePointlist);
            Assert.Equal(2, curvePointlist.Count());
        }
    }
}
