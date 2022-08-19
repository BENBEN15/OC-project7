using Newtonsoft.Json;

namespace UnitTests.Services
{
    public class BidServiceTest
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

        private List<Bid> SeedData()
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
        }

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Bid> bids = SeedData();
            contextMock.Setup(x => x.Bids).ReturnsDbSet(bids);
            var mapper = mapperCreation();
            BidRepository bidRepository = new BidRepository(contextMock.Object);
            BidService bidService = new BidService(bidRepository, mapper);

            //Act
            int id = 1;
            var bidResult = JsonConvert.SerializeObject(bidService.Get(id));
            var bidDto = mapper.Map<BidDTO>(SeedData().FirstOrDefault(x => x.BidId == id));
            var bid = JsonConvert.SerializeObject(bidDto);

            //Assert
            Assert.NotNull(bidResult);
            Assert.Equal(bid, bidResult);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Bid> bids = SeedData();
            contextMock.Setup(x => x.Bids).ReturnsDbSet(bids);
            var mapper = mapperCreation();
            BidRepository bidRepository = new BidRepository(contextMock.Object);
            BidService bidService = new BidService(bidRepository, mapper);

            //Act
            var bidsResult = JsonConvert.SerializeObject(bidService.GetAll());
            var dtoList = new List<BidDTO>();
            var bidsDB = contextMock.Object.Bids.Where(b => b.BidId > 0);
            foreach (Bid bid in bidsDB) { 
                var dto  = mapper.Map<BidDTO>(bid);
                dtoList.Add(dto);
            }
            var bidlist = JsonConvert.SerializeObject(dtoList);

            //Assert
            Assert.NotNull(bidsResult);
            Assert.NotEmpty(bidsResult);
            Assert.Equal(bidlist, bidsResult);
        }

        [Fact]
        public void save()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Bid> bids = SeedData();
            contextMock.Setup(x => x.Bids).ReturnsDbSet(bids);
            contextMock.Setup(m => m.Add(It.IsAny<Bid>())).Callback<Bid>(bids.Add);
            var mapper = mapperCreation();
            BidRepository bidRepository = new BidRepository(contextMock.Object);
            BidService bidService = new BidService(bidRepository, mapper);

            //Act
            int idToGet = 4;
            BidDTO bidToAdd = new BidDTO
            {
                BidId = idToGet,
                Account = "account4",
                Type = "type4",
                BidQuantity = 1.0,
                AskQuantity = 1.0,
                BidValue = 1.0,
                Ask = 1.0,
                Benchmark = "benchmark4",
                BidDate = new DateTime(2022, 01, 01),
                Commentary = "commentary4",
                Security = "securoty4",
                Status = "status4",
                Trader = "trader4",
                Book = "book4",
                CreationName = "creationName4",
                CreationDate = new DateTime(2022, 01, 01),
                RevisionName = "revisionName4",
                RevisionDate = new DateTime(2022, 01, 01),
                DealName = "dealName4",
                DealType = "dealType4",
                SourceListId = "SourceListId4",
                Side = "side4",
            };

            BidDTO bidResponse = bidService.Save(bidToAdd);
            BidDTO bidsResult = mapper.Map<BidDTO>(contextMock.Object.Bids.FirstOrDefault(x => x.BidId == idToGet));

            IEnumerable<Bid> bidlist = contextMock.Object.Bids.Where(b => b.BidId > 0);

            var bidToAddJson = JsonConvert.SerializeObject(bidToAdd);
            var bidResponseJson = JsonConvert.SerializeObject(bidResponse);
            var bidsResultJson = JsonConvert.SerializeObject(bidsResult);

            //Assert
            Assert.NotNull(bidsResult);
            Assert.NotEmpty(bidlist);
            Assert.Equal(4, bidlist.Count());
            Assert.Equal(bidToAddJson, bidsResultJson);
            Assert.Equal(bidsResultJson, bidResponseJson);
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
                    context.Bids.Add(b);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                var mapper = mapperCreation();
                BidRepository bidRepository = new BidRepository(context);
                BidService bidService = new BidService(bidRepository, mapper);

                BidDTO bidToUpdate = new BidDTO
                {
                    BidId = 1,
                    Account = "NEWaccount1",
                    Type = "NEWtype1",
                    BidQuantity = 2.0,
                    AskQuantity = 2.0,
                    BidValue = 2.0,
                    Ask = 2.0,
                    Benchmark = "NEWbenchmark1",
                    BidDate = new DateTime(2023, 01, 01),
                    Commentary = "NEWcommentary1",
                    Security = "NEWsecuroty1",
                    Status = "NEWstatus1",
                    Trader = "NEWtrader1",
                    Book = "NEWbook1",
                    CreationName = "NEWcreationName1",
                    CreationDate = new DateTime(2023, 01, 01),
                    RevisionName = "NEWrevisionName1",
                    RevisionDate = new DateTime(2023, 01, 01),
                    DealName = "NEWdealName1",
                    DealType = "NEWdealType1",
                    SourceListId = "NEWSourceListId1",
                    Side = "NEWside1",
                };

                //Act
                bidService.Update(bidToUpdate);
                BidDTO bidsResult = mapper.Map<BidDTO>(context.Bids.FirstOrDefault(x => x.BidId == bidToUpdate.BidId));
                var bidToUpdateJson = JsonConvert.SerializeObject(bidToUpdate);
                var bidResultJson = JsonConvert.SerializeObject(bidsResult);

                //Assert
                Assert.NotNull(bidsResult);
                Assert.Equal(bidToUpdateJson, bidResultJson);
            }
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
