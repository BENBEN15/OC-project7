using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class RatingRepositoryTest
    {
        private List<Rating> SeedData()
        {
            var ratings = new List<Rating>{
                new Rating
                {
                    RatingId = 1,
                    FitchRating = "fitchRating1",
                    MoodysRating = "MoodyRating1",
                    OrderNumber = 1,
                    SandPrating = "sandPrating1",
                },
                new Rating
                {
                    RatingId = 2,
                    FitchRating = "fitchRating2",
                    MoodysRating = "MoodyRating2",
                    OrderNumber = 2,
                    SandPrating = "sandPrating2",
                },
                new Rating
                {
                    RatingId = 3,
                    FitchRating = "fitchRating3",
                    MoodysRating = "MoodyRating3",
                    OrderNumber = 3,
                    SandPrating = "sandPrating3",
                },
            };
            return ratings;
        }

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rating> ratings = SeedData();
            contextMock.Setup(x => x.Ratings).ReturnsDbSet(ratings);
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);

            //Act
            int id = 1;
            var ratingResult = JsonConvert.SerializeObject(ratingRepository.Get(id));
            var rating = JsonConvert.SerializeObject(SeedData().FirstOrDefault(x => x.RatingId == id));

            //Assert
            Assert.NotNull(ratingResult);
            Assert.Equal(rating, ratingResult);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rating> ratings = SeedData();
            contextMock.Setup(x => x.Ratings).ReturnsDbSet(ratings);
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);

            //Act
            var ratingResult = JsonConvert.SerializeObject(ratingRepository.GetAll());
            var ratinglist = JsonConvert.SerializeObject(contextMock.Object.Ratings.Where(b => b.RatingId > 0));

            //Assert
            Assert.NotNull(ratingResult);
            Assert.NotEmpty(ratingResult);
            Assert.Equal(ratinglist, ratingResult);
        }

        [Fact]
        public void save()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rating> ratings = SeedData();
            contextMock.Setup(x => x.Ratings).ReturnsDbSet(ratings);
            contextMock.Setup(m => m.Add(It.IsAny<Rating>())).Callback<Rating>(ratings.Add);
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);

            //Act
            int idToGet = 4;
            Rating ratingToAdd = new Rating
            {
                RatingId = idToGet,
                FitchRating = "fitchRating4",
                MoodysRating = "MoodyRating4",
                OrderNumber = 4,
                SandPrating = "sandPrating4",
            };
            ratingRepository.Save(ratingToAdd);

            Rating ratingResult = contextMock.Object.Ratings.FirstOrDefault(x => x.RatingId == idToGet);
            IEnumerable<Rating> ratinglist = contextMock.Object.Ratings.Where(b => b.RatingId > 0);

            //Assert
            Assert.NotNull(ratingResult);
            Assert.NotEmpty(ratinglist);
            Assert.Equal(4, ratinglist.Count());
            Assert.Same(ratingToAdd, ratingResult);
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

                foreach (var r in SeedData())
                {
                    context.Ratings.Add(r);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                RatingRepository ratingRepository = new RatingRepository(context);

                Rating ratingToUpdate = new Rating
                {
                    RatingId = 1,
                    FitchRating = "fitchRating5",
                    MoodysRating = "MoodyRatin51",
                    OrderNumber = 5,
                    SandPrating = "sandPrating5",
                };

                //Act
                ratingRepository.Update(ratingToUpdate);
                Rating ratingResult = context.Ratings.FirstOrDefault(x => x.RatingId == ratingToUpdate.RatingId);

                //Assert
                Assert.NotNull(ratingResult);
                Assert.Same(ratingToUpdate, ratingResult);

            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rating> ratings = SeedData();
            contextMock.Setup(x => x.Ratings).ReturnsDbSet(ratings);
            contextMock.Setup(m => m.Remove(It.IsAny<Rating>())).Callback<Rating>(r => ratings.Remove(r));
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);

            //Act
            int idToDelete = 1;
            ratingRepository.Delete(idToDelete);

            Rating ratingResult = contextMock.Object.Ratings.FirstOrDefault(x => x.RatingId == idToDelete);
            IEnumerable<Rating> ratinglist = contextMock.Object.Ratings.Where(x => x.RatingId > 0);

            //Assert
            Assert.Null(ratingResult);
            Assert.NotEmpty(ratinglist);
            Assert.Equal(2, ratinglist.Count());
        }
    }
}
