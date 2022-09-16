using Newtonsoft.Json;

namespace UnitTests.Services
{
    public class BidServiceTest
    {
        /*private List<Bid> SeedData()
        {
            var bids = new List<Bid>{
            new Bid
            {
                BidId = 1,
                Account = "account1",
                Type = "type1",
                BidQuantity = 1.0,
                AskQuantity = 1.0,
                BidValue = 1.0,
                Ask = 1.0,
                Benchmark = "benchmark1",
                BidDate = new DateTime(2022,01,01),
                Commentary = "commentary1",
                Security = "securoty1",
                Status = "status1",
                Trader = "trader1",
                Book = "book1",
                CreationName = "creationName1",
                CreationDate = new DateTime(2022,01,01),
                RevisionName = "revisionName1",
                RevisionDate = new DateTime(2022,01,01),
                DealName = "dealName1",
                DealType = "dealType1",
                SourceListId = "SourceListId1",
                Side = "side1",
            },
            new Bid
            {
                BidId = 2,
                Account = "account2",
                Type = "type2",
                BidQuantity = 1.0,
                AskQuantity = 1.0,
                BidValue = 1.0,
                Ask = 1.0,
                Benchmark = "benchmark2",
                BidDate = new DateTime(2022,01,01),
                Commentary = "commentary2",
                Security = "securoty2",
                Status = "status2",
                Trader = "trader2",
                Book = "book2",
                CreationName = "creationName2",
                CreationDate = new DateTime(2022,01,01),
                RevisionName = "revisionName2",
                RevisionDate = new DateTime(2022,01,01),
                DealName = "dealName2",
                DealType = "dealType2",
                SourceListId = "SourceListId2",
                Side = "side2",
            },
            new Bid
            {
                BidId = 3,
                Account = "account3",
                Type = "type3",
                BidQuantity = 1.0,
                AskQuantity = 1.0,
                BidValue = 1.0,
                Ask = 1.0,
                Benchmark = "benchmark3",
                BidDate = new DateTime(2022,01,01),
                Commentary = "commentary3",
                Security = "securoty3",
                Status = "status3",
                Trader = "trader3",
                Book = "book3",
                CreationName = "creationName3",
                CreationDate = new DateTime(2022,01,01),
                RevisionName = "revisionName3",
                RevisionDate = new DateTime(2022,01,01),
                DealName = "dealName3",
                DealType = "dealType3",
                SourceListId = "SourceListId3",
                Side = "side3",
            },
        };

            return bids;
        }*/

        private IMapper mapperCreation()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return mapper;
        }

        public List<Bid> SeedData()
        {
            var items = new List<Bid>();
            Fixture fixture = new Fixture();
            var item1 = fixture.Create<Bid>();
            var item2 = fixture.Create<Bid>();
            var item3 = fixture.Create<Bid>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }

        public Mock<IRepository<Bid>> MockRepo()
        {
            return new Mock<IRepository<Bid>>();
        }

        public BidService initService(IRepository<Bid> repo, IMapper mapper)
        {
            return new BidService(repo, mapper);
        }


        [Fact]
        public void get()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Bid> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Get(data[0].BidId)).Returns(data[0]);
            var service = initService(mockRepo.Object, mapper);

            //Act
            var result = service.Get(data[0].BidId);

            //Assert
            var actualJson = JsonConvert.SerializeObject(result);
            var expected = mapper.Map<BidDTO>(data[0]);
            var expectedJson = JsonConvert.SerializeObject(expected);

            Assert.NotNull(result);
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var mapper = mapperCreation();
            List<Bid> data = SeedData();
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
        public void save(BidDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<Bid> data = SeedData();
            var repoReturn = mapper.Map<Bid>(dto);
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Save(It.IsAny<Bid>())).Callback<Bid>(x => data.Add(repoReturn)).Returns(repoReturn);
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
        public void update(BidDTO dto)
        {
            //Arrange
            var mapper = mapperCreation();
            List<Bid> data = SeedData();
            var mockRepo = MockRepo();
            mockRepo.Setup(x => x.Update(It.IsAny<Bid>()));
            var service = initService(mockRepo.Object, mapper);

            //Act
            service.Update(dto);
            mockRepo.Verify(mock => mock.Update(It.IsAny<Bid>()), Times.Once());
            //mockRepo.

            //Assert
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Bid> bids = SeedData();
            contextMock.Setup(x => x.Bids).ReturnsDbSet(bids);
            contextMock.Setup(m => m.Remove(It.IsAny<Bid>())).Callback<Bid>(b => bids.Remove(b));
            var mapper = mapperCreation();
            BidRepository bidRepository = new BidRepository(contextMock.Object);
            BidService bidService = new BidService(bidRepository, mapper);

            //Act
            int idToDelete = 1;
            bidService.Delete(idToDelete);

            Bid bidsResult = contextMock.Object.Bids.FirstOrDefault(x => x.BidId == idToDelete);
            IEnumerable<Bid> bidlist = contextMock.Object.Bids.Where(x => x.BidId > 0);

            //Assert
            Assert.Null(bidsResult);
            Assert.NotEmpty(bidlist);
            Assert.Equal(2, bidlist.Count());
        }
    }
}
