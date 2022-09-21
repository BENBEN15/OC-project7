using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnitTests.Repositories
{
    public class BidRepositoryTest
    {

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Bid> bids = SeedData();
            contextMock.Setup(x => x.Bids).ReturnsDbSet(bids);
            BidRepository bidRepository = new BidRepository(contextMock.Object);

            //Act
            int id = 1;
            var bidResult = JsonConvert.SerializeObject(bidRepository.Get(id));
            var bid = JsonConvert.SerializeObject(SeedData().FirstOrDefault(x => x.BidId == id));

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
            BidRepository bidRepository = new BidRepository(contextMock.Object);

            //Act
            var bidsResult = JsonConvert.SerializeObject(bidRepository.GetAll());
            var bidlist = JsonConvert.SerializeObject(contextMock.Object.Bids.Where(b => b.BidId > 0));

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
            BidRepository bidRepository = new BidRepository(contextMock.Object);

            //Act
            int idToGet = 4;
            Bid bidToAdd = new Bid
            {
                BidId = idToGet,
                Account = "account4",
                Type = "type4",
                BidQuantity = 1.0,
                AskQuantity = 1.0,
                BidValue = 1.0,
                Ask = 1.0,
                Benchmark = "benchmark4",
                BidDate = new DateTime(2022,01,01),
                Commentary = "commentary4",
                Security = "securoty4",
                Status = "status4",
                Trader = "trader4",
                Book = "book4",
                CreationName = "creationName4",
                CreationDate = new DateTime(2022,01,01),
                RevisionName = "revisionName4",
                RevisionDate = new DateTime(2022,01,01),
                DealName = "dealName4",
                DealType = "dealType4",
                SourceListId = "SourceListId4",
                Side = "side4",
            };
            bidRepository.Save(bidToAdd);

            Bid bidsResult = contextMock.Object.Bids.FirstOrDefault(x => x.BidId == idToGet);
            IEnumerable<Bid> bidlist = contextMock.Object.Bids.Where(b => b.BidId > 0);

            //Assert
            Assert.NotNull(bidsResult);
            Assert.NotEmpty(bidlist);
            Assert.Equal(4, bidlist.Count());
            Assert.Same(bidToAdd, bidsResult);
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
                BidRepository bidRepository = new BidRepository(context);

                Bid bidToUpdate = new Bid
                {
                    BidId = 1,
                    Account = "NEWaccount1",
                    Type = "NEWtype1",
                    BidQuantity = 2.0,
                    AskQuantity = 2.0,
                    BidValue = 2.0,
                    Ask = 2.0,
                    Benchmark = "NEWbenchmark1",
                    BidDate = new DateTime(2023,01,01),
                    Commentary = "NEWcommentary1",
                    Security = "NEWsecuroty1",
                    Status = "NEWstatus1",
                    Trader = "NEWtrader1",
                    Book = "NEWbook1",
                    CreationName = "NEWcreationName1",
                    CreationDate = new DateTime(2023,01,01),
                    RevisionName = "NEWrevisionName1",
                    RevisionDate = new DateTime(2023,01,01),
                    DealName = "NEWdealName1",
                    DealType = "NEWdealType1",
                    SourceListId = "NEWSourceListId1",
                    Side = "NEWside1",
                };

                //Act
                bidRepository.Update(bidToUpdate);
                Bid bidsResult = context.Bids.FirstOrDefault(x => x.BidId == bidToUpdate.BidId);

                //Assert
                Assert.NotNull(bidsResult);
                Assert.Same(bidToUpdate, bidsResult);

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
            BidRepository bidRepository = new BidRepository(contextMock.Object);

            //Act
            int idToDelete = 1;
            bidRepository.Delete(idToDelete);

            Bid bidsResult = contextMock.Object.Bids.FirstOrDefault(x => x.BidId == idToDelete);
            IEnumerable<Bid> bidlist = contextMock.Object.Bids.Where(x => x.BidId > 0);

            //Assert
            Assert.Null(bidsResult);
            Assert.NotEmpty(bidlist);
            Assert.Equal(2, bidlist.Count());
        }
    }
}
