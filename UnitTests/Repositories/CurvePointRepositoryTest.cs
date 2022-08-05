using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class CurvePointRepositoryTest
    {
        private List<CurvePoint> SeedData()
        {
            var curvepoints = new List<CurvePoint>{
            new CurvePoint
            {
                CurvePointId = 1,
                CurveId = 2,
                AsOfDate = new DateTime(2022,01,01),
                CreationDate = new DateTime(2022,01,01),
                Term = 1.0,
                Value = 1.0,
            },
            new CurvePoint
            {
                CurvePointId = 2,
                CurveId = 3,
                AsOfDate = new DateTime(2022,02,02),
                CreationDate = new DateTime(2022,02,02),
                Term = 2.0,
                Value = 2.0,
            },
            new CurvePoint
            {
                CurvePointId = 3,
                CurveId = 4,
                AsOfDate = new DateTime(2022,03,03),
                CreationDate = new DateTime(2022,03,03),
                Term = 3.0,
                Value = 3.0,
            },
        };

            return curvepoints;
        }

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);

            //Act
            int id = 1;
            var curvePointResult = JsonConvert.SerializeObject(curvePointRepository.Get(id));
            var curvePoint = JsonConvert.SerializeObject(SeedData().FirstOrDefault(x => x.CurvePointId == id));

            //Assert
            Assert.NotNull(curvePointResult);
            Assert.Equal(curvePoint, curvePointResult);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);

            //Act
            var curvePointResult = JsonConvert.SerializeObject(curvePointRepository.GetAll());
            var curvePointlist = JsonConvert.SerializeObject(contextMock.Object.CurvePoints.Where(x => x.CurvePointId > 0));

            //Assert
            Assert.NotNull(curvePointResult);
            Assert.NotEmpty(curvePointResult);
            Assert.Equal(curvePointlist, curvePointResult);
        }

        [Fact]
        public void save()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            contextMock.Setup(m => m.Add(It.IsAny<CurvePoint>())).Callback<CurvePoint>(curvePoints.Add);
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);

            //Act
            int idToGet = 4;
            CurvePoint cpToAdd = new CurvePoint
            {
                CurvePointId = idToGet,
                CurveId = 5,
                AsOfDate = new DateTime(2022, 04, 04),
                CreationDate = new DateTime(2022, 04, 04),
                Term = 4.0,
                Value = 4.0,
            };
            curvePointRepository.Save(cpToAdd);

            CurvePoint cpResult = contextMock.Object.CurvePoints.FirstOrDefault(x => x.CurvePointId == idToGet);
            IEnumerable<CurvePoint> cplist = contextMock.Object.CurvePoints.Where(x => x.CurvePointId > 0);

            //Assert
            Assert.NotNull(cpResult);
            Assert.NotEmpty(cplist);
            Assert.Equal(4, cplist.Count());
            Assert.Same(cpToAdd, cpResult);
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

                foreach (var cp in SeedData())
                {
                    context.CurvePoints.Add(cp);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                CurvePointRepository curvePointRepository = new CurvePointRepository(context);

                CurvePoint cpToUpdate = new CurvePoint
                {
                    CurvePointId = 1,
                    CurveId = 3,
                    AsOfDate = new DateTime(2023, 01, 01),
                    CreationDate = new DateTime(2023, 01, 01),
                    Term = 1.5,
                    Value = 1.5,
                };

                //Act
                curvePointRepository.Update(cpToUpdate);
                CurvePoint curvePointResult = context.CurvePoints.FirstOrDefault(x => x.CurvePointId == cpToUpdate.CurvePointId);

                //Assert
                Assert.NotNull(curvePointResult);
                Assert.Same(cpToUpdate, curvePointResult);
            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<CurvePoint> curvePoints = SeedData();
            contextMock.Setup(x => x.CurvePoints).ReturnsDbSet(curvePoints);
            contextMock.Setup(m => m.Remove(It.IsAny<CurvePoint>())).Callback<CurvePoint>(c => curvePoints.Remove(c));
            CurvePointRepository curvePointRepository = new CurvePointRepository(contextMock.Object);

            //Act
            int idToDelete = 1;
            curvePointRepository.Delete(idToDelete);

            CurvePoint cpResult = contextMock.Object.CurvePoints.FirstOrDefault(x => x.CurvePointId == idToDelete);
            IEnumerable<CurvePoint> cplist = contextMock.Object.CurvePoints.Where(x => x.CurvePointId > 0);

            //Assert
            Assert.Null(cpResult);
            Assert.NotEmpty(cplist);
            Assert.Equal(2, cplist.Count());
        }
    }
}
