using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using PoseidonAPI.Contracts.Bid;

namespace UnitTests.Controllers
{
    public class BidControllerTest
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

        public List<BidDTO> seedData()
        {
            var items = new List<BidDTO>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<BidDTO>();
            var item2 = fixture.Create<BidDTO>();
            var item3 = fixture.Create<BidDTO>();
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
            var moqService = new Mock<IService<BidDTO>>();
            List<BidDTO> dtos = seedData();
            moqService.Setup(x => x.GetAll()).Returns(dtos);
            var controller = new BidsController(moqService.Object, mapper);

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
            var moqService = new Mock<IService<BidDTO>>();
            List<BidDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].BidId)).Returns(dtos[0]);
            var controller = new BidsController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Get(dtos[0].BidId);

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
            var moqService = new Mock<IService<BidDTO>>();
            List<BidDTO> dtos = seedData();
            moqService.Setup(x => x.Get(dtos[0].BidId)).Returns(dtos[0]);
            var controller = new BidsController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Get(0);

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, AutoData]
        public void Create(CreateBidRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            var dto = mapper.Map<BidDTO>(request);
            var dtoWithId = mapper.Map<BidDTO>(request);
            var moqService = new Mock<IService<BidDTO>>();
            List<BidDTO> dtos = seedData();
            moqService.Setup(x => x.Get(1)).Returns(dtoWithId);
            moqService.Setup(x => x.Save(It.IsAny<BidDTO>())).Returns(dtoWithId);
            var controller = new BidsController(moqService.Object, mapper);

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
        public void Update(UpsertBidRequest request)
        {
            //Arrange
            var mapper = mapperCreation();
            List<BidDTO> dtos = seedData();
            int id = dtos[0].BidId;
            var dto = mapper.Map<BidDTO>(request);
            dto.BidId = id;
            var toUpdate = dtos[0];
            var updated = mapper.Map<BidDTO>(request);
            updated.BidId = id;

            var moqService = new Mock<IService<BidDTO>>();
            moqService.Setup(x => x.Update(It.IsAny<BidDTO>())).Callback<BidDTO>(x => dtos.Remove(toUpdate));
            moqService.Setup(x => x.Update(It.IsAny<BidDTO>())).Callback<BidDTO>(x => dtos.Add(updated));
            var controller = new BidsController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Update(id,request);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            var actualJson = JsonConvert.SerializeObject(dtos[0]);
            var notExpected =JsonConvert.SerializeObject(updated);
            var expectedJson = JsonConvert.SerializeObject(toUpdate);
            Assert.Equal(expectedJson, actualJson);
            Assert.NotEqual(notExpected, actualJson);
        }

        [Theory, AutoData]
        public void Delete()
        {
            //Arrange
            var mapper = mapperCreation();
            var moqService = new Mock<IService<BidDTO>>();
            List<BidDTO> dtos = seedData();
            List<BidDTO> controlDtos = dtos;
            int id = dtos[0].BidId;
            moqService.Setup(x => x.Delete(It.IsAny<int>())).Callback<int>(i => dtos.Remove(dtos.FirstOrDefault(x => x.BidId == id)));
            var controller = new BidsController(moqService.Object, mapper);

            //Act
            var actionResult = controller.Delete(id);

            //Assert
            var result = actionResult as OkResult;
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            controlDtos.Remove(controlDtos.FirstOrDefault(x => x.BidId == id));
            var actualJson = JsonConvert.SerializeObject(dtos);
            var expectedJson = JsonConvert.SerializeObject(controlDtos);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
