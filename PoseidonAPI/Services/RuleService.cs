using AutoMapper;
using PoseidonAPI.Repositories;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;
using System.Collections.Generic;

namespace PoseidonAPI.Services
{
    public class RuleService : IService<RuleDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Rule> _rulesRepository;

        public RuleService(IRepository<Rule> rulesRepository, IMapper mapper)
        {
            _rulesRepository = rulesRepository;
            _mapper = mapper;
        }

        public RuleDTO Get(int id)
        {
            Rule rule = _rulesRepository.Get(id);
            RuleDTO dto = _mapper.Map<RuleDTO>(rule);
            return dto;
        }

        public IEnumerable<RuleDTO> GetAll()
        {
            List<RuleDTO> dtos = new List<RuleDTO>();
            IEnumerable<Rule> rules = _rulesRepository.GetAll();
            foreach (Rule r in rules)
            {
                RuleDTO dto = _mapper.Map<RuleDTO>(r);
                dtos.Add(dto);
            }

            return dtos;
        }

        public RuleDTO Save(RuleDTO dto)
        {
            Rule rules = _mapper.Map<Rule>(dto);
            RuleDTO ruleDTO = _mapper.Map<RuleDTO>(_rulesRepository.Save(rules));
            return ruleDTO;
        }

        public void Update(RuleDTO dto)
        {
            Rule rules = _mapper.Map<Rule>(dto);
            _rulesRepository.Update(rules);
        }

        public void Delete(int id)
        {
            _rulesRepository.Delete(id);
        }
    }
}
