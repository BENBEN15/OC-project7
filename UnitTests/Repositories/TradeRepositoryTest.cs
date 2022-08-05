using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class TradeRepositoryTest
    {
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
            TradeRepository tradeRepository = new TradeRepository(contextMock.Object);

            //Act
            int id = 1;
            var tradeResult = JsonConvert.SerializeObject(tradeRepository.Get(id));
            var trade = JsonConvert.SerializeObject(SeedData().FirstOrDefault(x => x.TradeId == id));

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
            TradeRepository tradeRepository = new TradeRepository(contextMock.Object);

            //Act
            var tradeResult = JsonConvert.SerializeObject(tradeRepository.GetAll());
            var tradelist = JsonConvert.SerializeObject(contextMock.Object.Trades.Where(b => b.TradeId > 0));

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
            TradeRepository tradeRepository = new TradeRepository(contextMock.Object);

            //Act
            int idToGet = 4;
            Trade tradeToAdd = new Trade
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
            tradeRepository.Save(tradeToAdd);

            Trade tradeResult = contextMock.Object.Trades.FirstOrDefault(x => x.TradeId == idToGet);
            IEnumerable<Trade> tradelist = contextMock.Object.Trades.Where(b => b.TradeId > 0);

            //Assert
            Assert.NotNull(tradeResult);
            Assert.NotEmpty(tradelist);
            Assert.Equal(4, tradelist.Count());
            Assert.Same(tradeToAdd, tradeResult);
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

                foreach (var t in SeedData())
                {
                    context.Trades.Add(t);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                TradeRepository tradeRepository = new TradeRepository(context);

                Trade tradeToUpdate = new Trade
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
                tradeRepository.Update(tradeToUpdate);
                Trade tradeResult = context.Trades.FirstOrDefault(x => x.TradeId == tradeToUpdate.TradeId);

                //Assert
                Assert.NotNull(tradeResult);
                Assert.Same(tradeToUpdate, tradeResult);
            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Trade> trades = SeedData();
            contextMock.Setup(x => x.Trades).ReturnsDbSet(trades);
            contextMock.Setup(m => m.Remove(It.IsAny<Trade>())).Callback<Trade>(r => trades.Remove(r));
            TradeRepository tradeRepository = new TradeRepository(contextMock.Object);

            //Act
            int idToDelete = 1;
            tradeRepository.Delete(idToDelete);

            Trade tradeResult = contextMock.Object.Trades.FirstOrDefault(x => x.TradeId == idToDelete);
            IEnumerable<Trade> tradelist = contextMock.Object.Trades.Where(x => x.TradeId > 0);

            //Assert
            Assert.Null(tradeResult);
            Assert.NotEmpty(tradelist);
            Assert.Equal(2, tradelist.Count());
        }
    }
}
