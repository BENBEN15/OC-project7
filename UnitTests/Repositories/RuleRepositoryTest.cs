using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class RuleRepositoryTest
    {
        private List<Rule> SeedData()
        {
            var rules = new List<Rule>{
                new Rule
                {
                    RuleId = 1,
                    Description = "description1",
                    Json = "json1",
                    Name = "name1",
                    SqlPart = "sqlPart1",
                    SqlStr = "sqlStr1",
                    Template = "template1",
                },
                new Rule
                {
                    RuleId = 2,
                    Description = "description2",
                    Json = "json2",
                    Name = "name2",
                    SqlPart = "sqlPart2",
                    SqlStr = "sqlStr2",
                    Template = "template2",
                },
                new Rule
                {
                    RuleId = 3,
                    Description = "description3",
                    Json = "json3",
                    Name = "name3",
                    SqlPart = "sqlPart3",
                    SqlStr = "sqlStr3",
                    Template = "template3",
                },
            };
            return rules;
        }

        [Fact]
        public void get()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rule> rules = SeedData();
            contextMock.Setup(x => x.Rules).ReturnsDbSet(rules);
            RuleRepository ruleRepository = new RuleRepository(contextMock.Object);

            //Act
            int id = 1;
            var ruleResult = JsonConvert.SerializeObject(ruleRepository.Get(id));
            var rule = JsonConvert.SerializeObject(SeedData().FirstOrDefault(x => x.RuleId == id));

            //Assert
            Assert.NotNull(ruleResult);
            Assert.Equal(rule, ruleResult);
        }

        [Fact]
        public void getAll()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rule> rules = SeedData();
            contextMock.Setup(x => x.Rules).ReturnsDbSet(rules);
            RuleRepository ruleRepository = new RuleRepository(contextMock.Object);

            //Act
            var ruleResult = JsonConvert.SerializeObject(ruleRepository.GetAll());
            var rulelist = JsonConvert.SerializeObject(contextMock.Object.Rules.Where(b => b.RuleId > 0));

            //Assert
            Assert.NotNull(ruleResult);
            Assert.NotEmpty(ruleResult);
            Assert.Equal(rulelist, ruleResult);
        }

        [Fact]
        public void save()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rule> rules = SeedData();
            contextMock.Setup(x => x.Rules).ReturnsDbSet(rules);
            contextMock.Setup(m => m.Add(It.IsAny<Rule>())).Callback<Rule>(rules.Add);
            RuleRepository ruleRepository = new RuleRepository(contextMock.Object);

            //Act
            int idToGet = 4;
            Rule ruleToAdd = new Rule
            {
                RuleId = idToGet,
                Description = "description4",
                Json = "json4",
                Name = "name4",
                SqlPart = "sqlPart4",
                SqlStr = "sqlStr4",
                Template = "template4",
            };
            ruleRepository.Save(ruleToAdd);

            Rule ruleResult = contextMock.Object.Rules.FirstOrDefault(x => x.RuleId == idToGet);
            IEnumerable<Rule> rulelist = contextMock.Object.Rules.Where(b => b.RuleId > 0);

            //Assert
            Assert.NotNull(ruleResult);
            Assert.NotEmpty(rulelist);
            Assert.Equal(4, rulelist.Count());
            Assert.Same(ruleToAdd, ruleResult);
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
                    context.Rules.Add(r);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                RuleRepository rulesRepository = new RuleRepository(context);

                Rule ruleToUpdate = new Rule
                {
                    RuleId = 1,
                    Description = "description5",
                    Json = "json5",
                    Name = "name5",
                    SqlPart = "sqlPart5",
                    SqlStr = "sqlStr5",
                    Template = "template5",
                };

                //Act
                rulesRepository.Update(ruleToUpdate);
                Rule ruleResult = context.Rules.FirstOrDefault(x => x.RuleId == ruleToUpdate.RuleId);

                //Assert
                Assert.NotNull(ruleResult);
                Assert.Same(ruleToUpdate, ruleResult);

            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rule> rules = SeedData();
            contextMock.Setup(x => x.Rules).ReturnsDbSet(rules);
            contextMock.Setup(m => m.Remove(It.IsAny<Rule>())).Callback<Rule>(r => rules.Remove(r));
            RuleRepository ruleRepository = new RuleRepository(contextMock.Object);

            //Act
            int idToDelete = 1;
            ruleRepository.Delete(idToDelete);

            Rule ruleResult = contextMock.Object.Rules.FirstOrDefault(x => x.RuleId == idToDelete);
            IEnumerable<Rule> rulelist = contextMock.Object.Rules.Where(x => x.RuleId > 0);

            //Assert
            Assert.Null(ruleResult);
            Assert.NotEmpty(rulelist);
            Assert.Equal(2, rulelist.Count());
        }
    }
}
