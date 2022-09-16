using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PoseidonAPI.Contracts.Rating;

namespace UnitTests.Controllers
{
    public class RatingControllerTest
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

        public List<RatingDTO> seedData()
        {
            var items = new List<RatingDTO>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<RatingDTO>();
            var item2 = fixture.Create<RatingDTO>();
            var item3 = fixture.Create<RatingDTO>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public ILogger<RatingController> getLogger()
        {
            var logger = new Mock<ILogger<RatingController>>();
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
            var moqService = new Mock<IService<RatingDTO>>();
            List<RatingDTO> dtos = seedData();
            moqService.Setup(x => x.GetAll()).Returns(dtos);
            var controller = new RatingController(moqService.Object, mapperCreation(), getLogger());
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
            var moqService = new Mock<IService<RatingDTO>>();
            List<RatingDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].RatingId)).Returns(dtos[0]);
            var controller = new RatingController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Get(dtos[0].RatingId);

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
            var moqService = new Mock<IService<RatingDTO>>();
            List<RatingDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].RatingId)).Returns(dtos[0]);
            var controller = new RatingController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Get(0);

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, AutoData]
        public void Create(CreateRatingRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            var dto = mapper.Map<RatingDTO>(request);
            var dtoWithId = mapper.Map<RatingDTO>(request);
            var moqService = new Mock<IService<RatingDTO>>();
            List<RatingDTO> dtos = seedData();
            moqService.Setup(x => x.Get(1)).Returns(dtoWithId);
            moqService.Setup(x => x.Save(It.IsAny<RatingDTO>())).Returns(dtoWithId);
            var controller = new RatingController(moqService.Object, mapper, getLogger());
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
        public void Update(UpsertRatingRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            List<RatingDTO> dtos = seedData();
            int id = dtos[0].RatingId;
            var dto = mapper.Map<RatingDTO>(request);
            dto.RatingId = id;
            var toUpdate = dtos[0];
            var updated = mapper.Map<RatingDTO>(request);
            updated.RatingId = id;

            var moqService = new Mock<IService<RatingDTO>>();
            moqService.Setup(x => x.Update(It.IsAny<RatingDTO>())).Callback<RatingDTO>(x => dtos.Remove(toUpdate));
            moqService.Setup(x => x.Update(It.IsAny<RatingDTO>())).Callback<RatingDTO>(x => dtos.Add(updated));
            var controller = new RatingController(moqService.Object, mapper, getLogger());
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
            var moqService = new Mock<IService<RatingDTO>>();
            List<RatingDTO> dtos = seedData();
            List<RatingDTO> controlDtos = dtos;
            int id = dtos[0].RatingId;
            moqService.Setup(x => x.Delete(It.IsAny<int>())).Callback<int>(i => dtos.Remove(dtos.FirstOrDefault(x => x.RatingId == id)));
            var controller = new RatingController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Delete(id);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            controlDtos.Remove(controlDtos.FirstOrDefault(x => x.RatingId == id));
            var actualJson = JsonConvert.SerializeObject(dtos);
            var expectedJson = JsonConvert.SerializeObject(controlDtos);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
