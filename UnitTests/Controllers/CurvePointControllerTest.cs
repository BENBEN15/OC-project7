using PoseidonAPI.Contracts.CurvePoint;

namespace UnitTests.Controllers
{
    public class CurvePointControllerTest
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

        public List<CurvePointDTO> seedData()
        {
            var items = new List<CurvePointDTO>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<CurvePointDTO>();
            var item2 = fixture.Create<CurvePointDTO>();
            var item3 = fixture.Create<CurvePointDTO>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public ILogger<CurvePointsController> getLogger()
        {
            var logger = new Mock<ILogger<CurvePointsController>>();
            return logger.Object;
        }

        public DefaultHttpContext getHttpContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "userName"),

            }, "mock"));
            return new DefaultHttpContext() { User = user };
        }

        [Fact]
        public void Get_All()
        {
            //Arrange
            var moqService = new Mock<IService<CurvePointDTO>>();
            List<CurvePointDTO> dtos = seedData();
            moqService.Setup(x => x.GetAll()).Returns(dtos);
            var controller = new CurvePointsController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.GetAll();

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var actual = result.Value;
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(dtos);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void Get_valid_id()
        {
            //Arrange
            var moqService = new Mock<IService<CurvePointDTO>>();
            List<CurvePointDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].CurvePointId)).Returns(dtos[0]);
            var controller = new CurvePointsController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Get(dtos[0].CurvePointId);

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var actual = result.Value;
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(dtos[0]);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void Get_invalid_id()
        {
            //Arrange
            var moqService = new Mock<IService<CurvePointDTO>>();
            List<CurvePointDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].CurvePointId)).Returns(dtos[0]);
            var controller = new CurvePointsController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Get(0);

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, AutoData]
        public void Create(CreateCurvePointRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            var dto = mapper.Map<CurvePointDTO>(request);
            var dtoWithId = mapper.Map<CurvePointDTO>(request);
            var moqService = new Mock<IService<CurvePointDTO>>();
            List<CurvePointDTO> dtos = seedData();
            moqService.Setup(x => x.Get(1)).Returns(dtoWithId);
            moqService.Setup(x => x.Save(It.IsAny<CurvePointDTO>())).Returns(dtoWithId);
            var controller = new CurvePointsController(moqService.Object, mapper, getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Add(request);

            //Assert
            var result = actionResult as CreatedAtActionResult;
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);

            var actual = result.Value;
            var actualJson = JsonConvert.SerializeObject(actual);
            var expectedJson = JsonConvert.SerializeObject(dtoWithId);
            Assert.Equal(expectedJson, actualJson);
        }

        [Theory, AutoData]
        public void Update(UpsertCurvePointRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            List<CurvePointDTO> dtos = seedData();
            int id = dtos[0].CurvePointId;
            var dto = mapper.Map<CurvePointDTO>(request);
            dto.CurvePointId = id;
            var toUpdate = dtos[0];
            var updated = mapper.Map<CurvePointDTO>(request);
            updated.CurvePointId = id;

            var moqService = new Mock<IService<CurvePointDTO>>();
            moqService.Setup(x => x.Update(It.IsAny<CurvePointDTO>())).Callback<CurvePointDTO>(x => dtos.Remove(toUpdate));
            moqService.Setup(x => x.Update(It.IsAny<CurvePointDTO>())).Callback<CurvePointDTO>(x => dtos.Add(updated));
            var loggerMoq = new Mock<ILogger<CurvePointsController>>();
            var controller = new CurvePointsController(moqService.Object, mapper, getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Update(id, request);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            var actualJson = JsonConvert.SerializeObject(dtos[0]);
            var notExpected = JsonConvert.SerializeObject(updated);
            var expectedJson = JsonConvert.SerializeObject(toUpdate);
            Assert.Equal(expectedJson, actualJson);
            Assert.NotEqual(notExpected, actualJson);
        }

        [Theory, AutoData]
        public void Delete()
        {
            //Arrange
            var moqService = new Mock<IService<CurvePointDTO>>();
            List<CurvePointDTO> dtos = seedData();
            List<CurvePointDTO> controlDtos = dtos;
            int id = dtos[0].CurvePointId;
            moqService.Setup(x => x.Delete(It.IsAny<int>())).Callback<int>(i => dtos.Remove(dtos.FirstOrDefault(x => x.CurvePointId == id)));
            var controller = new CurvePointsController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Delete(id);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            controlDtos.Remove(controlDtos.FirstOrDefault(x => x.CurvePointId == id));
            var actualJson = JsonConvert.SerializeObject(dtos);
            var expectedJson = JsonConvert.SerializeObject(controlDtos);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
