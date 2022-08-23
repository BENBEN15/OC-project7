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

        private List<Trade> SeedData()
        {
            var trades = new List<Trade>{
                new Trade
                {
                    TradeId = 1,
                    Account = "account1",
                    Benchmark = "benchMark1",
                    Book = "book1",
                    BuyPrice = 1.0,
                    BuyQuantity = 1.0,
                    CreationDate = new DateTime(2022,01,01),
                    CreationName = "creationName1",
                    DealName = "dealName1",
                    DealType = "dealType1",
                    RevisionDate = new DateTime(2022,01,01),
                    RevisionName = "revisionName1",
                    Security =  "security1",
                    SellPrice = 1.0,
                    SellQuantity = 1.0,
                    Side = "side1",
                    SourceListId = "1",
                    Status = "status1",
                    TradeDate = new DateTime(2022,01,01),
                    Trader = "trader1",
                    Type = "type1",
                },
                new Trade
                {
                    TradeId = 2,
                    Account = "account2",
                    Benchmark = "benchMark2",
                    Book = "book2",
                    BuyPrice = 2.0,
                    BuyQuantity = 2.0,
                    CreationDate = new DateTime(2022,02,02),
                    CreationName = "creationName2",
                    DealName = "dealName2",
                    DealType = "dealType2",
                    RevisionDate = new DateTime(2022,02,02),
                    RevisionName = "revisionName2",
                    Security =  "security2",
                    SellPrice = 2.0,
                    SellQuantity = 2.0,
                    Side = "side2",
                    SourceListId = "2",
                    Status = "status2",
                    TradeDate = new DateTime(2022,02,02),
                    Trader = "trader2",
                    Type = "type2",
                },
                new Trade
                {
                    TradeId = 3,
                    Account = "account3",
                    Benchmark = "benchMark3",
                    Book = "book3",
                    BuyPrice = 3.0,
                    BuyQuantity = 3.0,
                    CreationDate = new DateTime(2022,03,03),
                    CreationName = "creationName3",
                    DealName = "dealName3",
                    DealType = "dealType3",
                    RevisionDate = new DateTime(2022,03,03),
                    RevisionName = "revisionName3",
                    Security =  "security3",
                    SellPrice = 3.0,
                    SellQuantity = 3.0,
                    Side = "side3",
                    SourceListId = "3",
                    Status = "status3",
                    TradeDate = new DateTime(2022,03,03),
                    Trader = "trader3",
                    Type = "type3",
                },
            };
            return trades;
        }

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Trade> trades = SeedData();
            contextMock.Setup(x => x.Trades).ReturnsDbSet(trades);
            var mapper = mapperCreation();
            TradeRepository tradesRepository = new TradeRepository(contextMock.Object);
            TradeService tradesService = new TradeService(tradesRepository, mapper);

            //Act
            int id = 1;
            var tradeResult = JsonConvert.SerializeObject(tradesService.Get(id));
            var tradeDto = mapper.Map<TradeDTO>(SeedData().FirstOrDefault(x => x.TradeId == id));
            var trade = JsonConvert.SerializeObject(tradeDto);

            //Assert
            Assert.NotNull(tradeResult);
            Assert.Equal(trade, tradeResult);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Trade> trades = SeedData();
            contextMock.Setup(x => x.Trades).ReturnsDbSet(trades);
            var mapper = mapperCreation();
            TradeRepository tradesRepository = new TradeRepository(contextMock.Object);
            TradeService tradesService = new TradeService(tradesRepository, mapper);

            //Act
            var tradeResult = JsonConvert.SerializeObject(tradesService.GetAll());
            var dtoList = new List<TradeDTO>();
            var tradesDB = contextMock.Object.Trades.Where(b => b.TradeId > 0);
            foreach (Trade trade in tradesDB)
            {
                var dto = mapper.Map<TradeDTO>(trade);
                dtoList.Add(dto);
            }
            var tradelist = JsonConvert.SerializeObject(dtoList);

            //Assert
            Assert.NotNull(tradeResult);
            Assert.NotEmpty(tradeResult);
            Assert.Equal(tradelist, tradeResult);
        }

        [Fact]
        public void save()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Trade> trades = SeedData();
            contextMock.Setup(x => x.Trades).ReturnsDbSet(trades);
            contextMock.Setup(m => m.Add(It.IsAny<Trade>())).Callback<Trade>(trades.Add);
            var mapper = mapperCreation();
            TradeRepository tradesRepository = new TradeRepository(contextMock.Object);
            TradeService tradesService = new TradeService(tradesRepository, mapper);

            //Act
            int idToGet = 4;
            TradeDTO tradeToAdd = new TradeDTO
            {
                TradeId = idToGet,
                Account = "account4",
                Benchmark = "benchMark4",
                Book = "book4",
                BuyPrice = 4.0,
                BuyQuantity = 4.0,
                CreationDate = new DateTime(2023, 03, 03),
                CreationName = "creationName4",
                DealName = "dealName4",
                DealType = "dealType4",
                RevisionDate = new DateTime(2023, 03, 03),
                RevisionName = "revisionName4",
                Security = "security4",
                SellPrice = 4.0,
                SellQuantity = 4.0,
                Side = "side4",
                SourceListId = "4",
                Status = "status4",
                TradeDate = new DateTime(2023, 03, 03),
                Trader = "trader4",
                Type = "type4",
            };

            TradeDTO tradeResponse = tradesService.Save(tradeToAdd);
            TradeDTO tradeResult = mapper.Map<TradeDTO>(contextMock.Object.Trades.FirstOrDefault(x => x.TradeId == idToGet));

            IEnumerable<Trade> tradelist = contextMock.Object.Trades.Where(b => b.TradeId > 0);

            var tradeToAddJson = JsonConvert.SerializeObject(tradeToAdd);
            var tradeResponseJson = JsonConvert.SerializeObject(tradeResponse);
            var tradesResultJson = JsonConvert.SerializeObject(tradeResult);

            //Assert
            Assert.NotNull(tradeResult);
            Assert.NotEmpty(tradelist);
            Assert.Equal(4, tradelist.Count());
            Assert.Equal(tradeToAddJson, tradesResultJson);
            Assert.Equal(tradesResultJson, tradeResponseJson);
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
                    context.Trades.Add(b);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                var mapper = mapperCreation();
                TradeRepository tradeRepository = new TradeRepository(context);
                TradeService tradeService = new TradeService(tradeRepository, mapper);

                TradeDTO tradeToUpdate = new TradeDTO
                {
                    TradeId = 1,
                    Account = "account5",
                    Benchmark = "benchMark5",
                    Book = "book5",
                    BuyPrice = 5.0,
                    BuyQuantity = 5.0,
                    CreationDate = new DateTime(2022, 05, 05),
                    CreationName = "creationName5",
                    DealName = "dealName5",
                    DealType = "dealType5",
                    RevisionDate = new DateTime(2022, 05, 05),
                    RevisionName = "revisionName5",
                    Security = "security5",
                    SellPrice = 5.0,
                    SellQuantity = 5.0,
                    Side = "side5",
                    SourceListId = "5",
                    Status = "status5",
                    TradeDate = new DateTime(2022, 05, 05),
                    Trader = "trader5",
                    Type = "type5",
                };

                //Act
                tradeService.Update(tradeToUpdate);
                TradeDTO tradesResult = mapper.Map<TradeDTO>(context.Trades.FirstOrDefault(x => x.TradeId == tradeToUpdate.TradeId));
                var tradesToUpdateJson = JsonConvert.SerializeObject(tradeToUpdate);
                var tradeResultJson = JsonConvert.SerializeObject(tradesResult);

                //Assert
                Assert.NotNull(tradesResult);
                Assert.Equal(tradesToUpdateJson, tradeResultJson);
            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Trade> trades = SeedData();
            contextMock.Setup(x => x.Trades).ReturnsDbSet(trades);
            contextMock.Setup(m => m.Remove(It.IsAny<Trade>())).Callback<Trade>(b => trades.Remove(b));
            var mapper = mapperCreation();
            TradeRepository tradesRepository = new TradeRepository(contextMock.Object);
            TradeService tradesService = new TradeService(tradesRepository, mapper);

            //Act
            int idToDelete = 1;
            tradesService.Delete(idToDelete);

            Trade tradeResult = contextMock.Object.Trades.FirstOrDefault(x => x.TradeId == idToDelete);
            IEnumerable<Trade> tradelist = contextMock.Object.Trades.Where(x => x.TradeId > 0);

            //Assert
            Assert.Null(tradeResult);
            Assert.NotEmpty(tradelist);
            Assert.Equal(2, tradelist.Count());
        }
    }
}
