using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoseidonAPI.Contracts.Trade;

namespace UnitTests.Controllers
{
    public class TradeControllerTest
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

        public List<TradeDTO> seedData()
        {
            var items = new List<TradeDTO>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<TradeDTO>();
            var item2 = fixture.Create<TradeDTO>();
            var item3 = fixture.Create<TradeDTO>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        [Fact]
        public void Get_All()
        {
            //Arrange
            var mapper = mapperCreation();
            var moqService = new Mock<IService<TradeDTO>>();
            List<TradeDTO> dtos = seedData();
            moqService.Setup(x => x.GetAll()).Returns(dtos);
            var controller = new TradeController(moqService.Object, mapper);

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
            var mapper = mapperCreation();
            var moqService = new Mock<IService<TradeDTO>>();
            List<TradeDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].TradeId)).Returns(dtos[0]);
            var controller = new TradeController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Get(dtos[0].TradeId);

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
            var mapper = mapperCreation();
            var moqService = new Mock<IService<TradeDTO>>();
            List<TradeDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].TradeId)).Returns(dtos[0]);
            var controller = new TradeController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Get(0);

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, AutoData]
        public void Create(CreateTradeRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            var dto = mapper.Map<TradeDTO>(request);
            var dtoWithId = mapper.Map<TradeDTO>(request);
            var moqService = new Mock<IService<TradeDTO>>();
            List<TradeDTO> dtos = seedData();
            moqService.Setup(x => x.Get(1)).Returns(dtoWithId);
            moqService.Setup(x => x.Save(It.IsAny<TradeDTO>())).Returns(dtoWithId);
            var controller = new TradeController(moqService.Object, mapper);

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
        public void Update(UpsertTradeRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            List<TradeDTO> dtos = seedData();
            int id = dtos[0].TradeId;
            var dto = mapper.Map<TradeDTO>(request);
            dto.TradeId = id;
            var toUpdate = dtos[0];
            var updated = mapper.Map<TradeDTO>(request);
            updated.TradeId = id;

            var moqService = new Mock<IService<TradeDTO>>();
            moqService.Setup(x => x.Update(It.IsAny<TradeDTO>())).Callback<TradeDTO>(x => dtos.Remove(toUpdate));
            moqService.Setup(x => x.Update(It.IsAny<TradeDTO>())).Callback<TradeDTO>(x => dtos.Add(updated));
            var controller = new TradeController(moqService.Object, mapper);

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
            var mapper = mapperCreation();
            var moqService = new Mock<IService<TradeDTO>>();
            List<TradeDTO> dtos = seedData();
            List<TradeDTO> controlDtos = dtos;
            int id = dtos[0].TradeId;
            moqService.Setup(x => x.Delete(It.IsAny<int>())).Callback<int>(i => dtos.Remove(dtos.FirstOrDefault(x => x.TradeId == id)));
            var controller = new TradeController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Delete(id);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            controlDtos.Remove(controlDtos.FirstOrDefault(x => x.TradeId == id));
            var actualJson = JsonConvert.SerializeObject(dtos);
            var expectedJson = JsonConvert.SerializeObject(controlDtos);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
