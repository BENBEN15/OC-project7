using Newtonsoft.Json;

namespace UnitTests.Services
{
    public class RuleServiceTest
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
            var mapper = mapperCreation();
            RuleRepository rulesRepository = new RuleRepository(contextMock.Object);
            RuleService ruleService = new RuleService(rulesRepository, mapper);

            //Act
            int id = 1;
            var ruleResult = JsonConvert.SerializeObject(ruleService.Get(id));
            var ruleDto = mapper.Map<RuleDTO>(SeedData().FirstOrDefault(x => x.RuleId == id));
            var rule = JsonConvert.SerializeObject(ruleDto);

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
            var mapper = mapperCreation();
            RuleRepository rulesRepository = new RuleRepository(contextMock.Object);
            RuleService ruleService = new RuleService(rulesRepository, mapper);

            //Act
            var ruleResult = JsonConvert.SerializeObject(ruleService.GetAll());
            var dtoList = new List<RuleDTO>();
            var rulesDB = contextMock.Object.Rules.Where(b => b.RuleId > 0);
            foreach (Rule rule in rulesDB)
            {
                var dto = mapper.Map<RuleDTO>(rule);
                dtoList.Add(dto);
            }
            var rulelist = JsonConvert.SerializeObject(dtoList);

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
            var mapper = mapperCreation();
            RuleRepository rulesRepository = new RuleRepository(contextMock.Object);
            RuleService ruleService = new RuleService(rulesRepository, mapper);

            //Act
            int idToGet = 4;
            RuleDTO ruleToAdd = new RuleDTO
            {
                RuleId = idToGet,
                Description = "description4",
                Json = "json4",
                Name = "name4",
                SqlPart = "sqlPart4",
                SqlStr = "sqlStr4",
                Template = "template4",
            };

            RuleDTO ruleResponse = ruleService.Save(ruleToAdd);
            RuleDTO ruleResult = mapper.Map<RuleDTO>(contextMock.Object.Rules.FirstOrDefault(x => x.RuleId == idToGet));

            IEnumerable<Rule> rulelist = contextMock.Object.Rules.Where(b => b.RuleId > 0);

            var ruleToAddJson = JsonConvert.SerializeObject(ruleToAdd);
            var ruleResponseJson = JsonConvert.SerializeObject(ruleResponse);
            var rulesResultJson = JsonConvert.SerializeObject(ruleResult);

            //Assert
            Assert.NotNull(ruleResult);
            Assert.NotEmpty(rulelist);
            Assert.Equal(4, rulelist.Count());
            Assert.Equal(ruleToAddJson, rulesResultJson);
            Assert.Equal(rulesResultJson, ruleResponseJson);
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
                    context.Rules.Add(b);
                }
                context.SaveChanges();
            }

            using (var context = new PoseidonDBContext(options))
            {
                var mapper = mapperCreation();
                RuleRepository ruleRepository = new RuleRepository(context);
                RuleService ruleService = new RuleService(ruleRepository, mapper);

                RuleDTO ruleToUpdate = new RuleDTO
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
                ruleService.Update(ruleToUpdate);
                RuleDTO rulesResult = mapper.Map<RuleDTO>(context.Rules.FirstOrDefault(x => x.RuleId == ruleToUpdate.RuleId));
                var rulesToUpdateJson = JsonConvert.SerializeObject(ruleToUpdate);
                var ruleResultJson = JsonConvert.SerializeObject(rulesResult);

                //Assert
                Assert.NotNull(rulesResult);
                Assert.Equal(rulesToUpdateJson, ruleResultJson);
            }
        }

        [Fact]
        public void delete()
        {
            //Arrange
            var contextMock = new Mock<PoseidonDBContext>();
            List<Rule> rules = SeedData();
            contextMock.Setup(x => x.Rules).ReturnsDbSet(rules);
            contextMock.Setup(m => m.Remove(It.IsAny<Rule>())).Callback<Rule>(b => rules.Remove(b));
            var mapper = mapperCreation();
            RuleRepository ruleRepository = new RuleRepository(contextMock.Object);
            RuleService ruleService = new RuleService(ruleRepository, mapper);

            //Act
            int idToDelete = 1;
            ruleService.Delete(idToDelete);

            Rule ruleResult = contextMock.Object.Rules.FirstOrDefault(x => x.RuleId == idToDelete);
            IEnumerable<Rule> rulelist = contextMock.Object.Rules.Where(x => x.RuleId > 0);

            //Assert
            Assert.Null(ruleResult);
            Assert.NotEmpty(rulelist);
            Assert.Equal(2, rulelist.Count());
        }
    }
}
