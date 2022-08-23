using Newtonsoft.Json;

namespace UnitTests.Services
{
    public class RatingServiceTest
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
            var mapper = mapperCreation();
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);
            RatingService ratingService = new RatingService(ratingRepository, mapper);

            //Act
            int id = 1;
            var ratingResult = JsonConvert.SerializeObject(ratingService.Get(id));
            var ratingDto = mapper.Map<RatingDTO>(SeedData().FirstOrDefault(x => x.RatingId == id));
            var rating = JsonConvert.SerializeObject(ratingDto);

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
            var mapper = mapperCreation();
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);
            RatingService ratingService = new RatingService(ratingRepository, mapper);

            //Act
            var ratingResult = JsonConvert.SerializeObject(ratingService.GetAll());
            var dtoList = new List<RatingDTO>();
            var ratingsDB = contextMock.Object.Ratings.Where(b => b.RatingId > 0);
            foreach (Rating rating in ratingsDB)
            {
                var dto = mapper.Map<RatingDTO>(rating);
                dtoList.Add(dto);
            }
            var ratinglist = JsonConvert.SerializeObject(dtoList);

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
            var mapper = mapperCreation();
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);
            RatingService ratingService = new RatingService(ratingRepository, mapper);
            
            //Act
            int idToGet = 4;
            RatingDTO ratingToAdd = new RatingDTO
            {
                RatingId = idToGet,
                FitchRating = "fitchRating4",
                MoodysRating = "MoodyRating4",
                OrderNumber = 4,
                SandPrating = "sandPrating4",
            };

            RatingDTO ratingResponse = ratingService.Save(ratingToAdd);
            RatingDTO ratingResult = mapper.Map<RatingDTO>(contextMock.Object.Ratings.FirstOrDefault(x => x.RatingId == idToGet));

            IEnumerable<Rating> ratinglist = contextMock.Object.Ratings.Where(b => b.RatingId > 0);

            var ratingToAddJson = JsonConvert.SerializeObject(ratingToAdd);
            var ratingResponseJson = JsonConvert.SerializeObject(ratingResponse);
            var ratingsResultJson = JsonConvert.SerializeObject(ratingResult);

            //Assert
            Assert.NotNull(ratingResult);
            Assert.NotEmpty(ratinglist);
            Assert.Equal(4, ratinglist.Count());
            Assert.Equal(ratingToAddJson, ratingsResultJson);
            Assert.Equal(ratingsResultJson, ratingResponseJson);
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
                    context.Ratings.Add(b);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                var mapper = mapperCreation();
                RatingRepository ratingRepository = new RatingRepository(context);
                RatingService ratingService = new RatingService(ratingRepository, mapper);

                RatingDTO ratingToUpdate = new RatingDTO
                {
                    RatingId = 1,
                    FitchRating = "fitchRating5",
                    MoodysRating = "MoodyRatin51",
                    OrderNumber = 5,
                    SandPrating = "sandPrating5",
                };

                //Act
                ratingService.Update(ratingToUpdate);
                RatingDTO ratingsResult = mapper.Map<RatingDTO>(context.Ratings.FirstOrDefault(x => x.RatingId == ratingToUpdate.RatingId));
                var curvePointToUpdateJson = JsonConvert.SerializeObject(ratingToUpdate);
                var curvePointResultJson = JsonConvert.SerializeObject(ratingsResult);

                //Assert
                Assert.NotNull(ratingsResult);
                Assert.Equal(curvePointToUpdateJson, curvePointResultJson);
            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rating> ratings = SeedData();
            contextMock.Setup(x => x.Ratings).ReturnsDbSet(ratings);
            contextMock.Setup(m => m.Remove(It.IsAny<Rating>())).Callback<Rating>(b => ratings.Remove(b));
            var mapper = mapperCreation();
            RatingRepository ratingRepository = new RatingRepository(contextMock.Object);
            RatingService ratingService = new RatingService(ratingRepository, mapper);

            //Act
            int idToDelete = 1;
            ratingService.Delete(idToDelete);

            Rating ratingResult = contextMock.Object.Ratings.FirstOrDefault(x => x.RatingId == idToDelete);
            IEnumerable<Rating> ratinglist = contextMock.Object.Ratings.Where(x => x.RatingId > 0);

            //Assert
            Assert.Null(ratingResult);
            Assert.NotEmpty(ratinglist);
            Assert.Equal(2, ratinglist.Count());
        }
    }
}
