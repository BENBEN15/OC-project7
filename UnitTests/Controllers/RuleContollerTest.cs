using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PoseidonAPI.Contracts.Rule;

namespace UnitTests.Controllers
{
    public class RuleContollerTest
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

        public List<RuleDTO> seedData()
        {
            var items = new List<RuleDTO>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<RuleDTO>();
            var item2 = fixture.Create<RuleDTO>();
            var item3 = fixture.Create<RuleDTO>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }
        public ILogger<RuleController> getLogger()
        {
            var logger = new Mock<ILogger<RuleController>>();
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
            var moqService = new Mock<IService<RuleDTO>>();
            List<RuleDTO> dtos = seedData();
            moqService.Setup(x => x.GetAll()).Returns(dtos);
            var controller = new RuleController(moqService.Object, mapperCreation(), getLogger());
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
            var moqService = new Mock<IService<RuleDTO>>();
            List<RuleDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].RuleId)).Returns(dtos[0]);
            var controller = new RuleController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Get(dtos[0].RuleId);

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
            var moqService = new Mock<IService<RuleDTO>>();
            List<RuleDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].RuleId)).Returns(dtos[0]);
            var controller = new RuleController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Get(0);

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, AutoData]
        public void Create(CreateRuleRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            var dto = mapper.Map<RuleDTO>(request);
            var dtoWithId = mapper.Map<RuleDTO>(request);
            var moqService = new Mock<IService<RuleDTO>>();
            List<RuleDTO> dtos = seedData();
            moqService.Setup(x => x.Get(1)).Returns(dtoWithId);
            moqService.Setup(x => x.Save(It.IsAny<RuleDTO>())).Returns(dtoWithId);
            var controller = new RuleController(moqService.Object, mapper, getLogger());
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
        public void Update(UpsertRuleRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            List<RuleDTO> dtos = seedData();
            int id = dtos[0].RuleId;
            var dto = mapper.Map<RuleDTO>(request);
            dto.RuleId = id;
            var toUpdate = dtos[0];
            var updated = mapper.Map<RuleDTO>(request);
            updated.RuleId = id;

            var moqService = new Mock<IService<RuleDTO>>();
            moqService.Setup(x => x.Update(It.IsAny<RuleDTO>())).Callback<RuleDTO>(x => dtos.Remove(toUpdate));
            moqService.Setup(x => x.Update(It.IsAny<RuleDTO>())).Callback<RuleDTO>(x => dtos.Add(updated));
            var controller = new RuleController(moqService.Object, mapper, getLogger());
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
            var moqService = new Mock<IService<RuleDTO>>();
            List<RuleDTO> dtos = seedData();
            List<RuleDTO> controlDtos = dtos;
            int id = dtos[0].RuleId;
            moqService.Setup(x => x.Delete(It.IsAny<int>())).Callback<int>(i => dtos.Remove(dtos.FirstOrDefault(x => x.RuleId == id)));
            var controller = new RuleController(moqService.Object, mapperCreation(), getLogger());
            controller.ControllerContext.HttpContext = getHttpContext();

            //Act
            var actionResult = controller.Delete(id);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            controlDtos.Remove(controlDtos.FirstOrDefault(x => x.RuleId == id));
            var actualJson = JsonConvert.SerializeObject(dtos);
            var expectedJson = JsonConvert.SerializeObject(controlDtos);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
