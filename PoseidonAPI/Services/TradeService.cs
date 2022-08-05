using AutoMapper;
using PoseidonAPI.Repositories;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;
using System.Collections.Generic;

namespace PoseidonAPI.Services
{
    public class TradeService : IService<TradeDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Trade> _tradesRepository;

        public TradeService(IRepository<Trade> tradesRepository, IMapper mapper)
        {
            _tradesRepository = tradesRepository;
            _mapper = mapper;
        }

        public TradeDTO Get(int id)
        {
            Trade trade = _tradesRepository.Get(id);
            TradeDTO dto = _mapper.Map<TradeDTO>(trade);
            return dto;
        }

        public IEnumerable<TradeDTO> GetAll()
        {
            List<TradeDTO> dtos = new List<TradeDTO>();
            IEnumerable<Trade> trades = _tradesRepository.GetAll();
            foreach (Trade t in trades)
            {
                TradeDTO dto = _mapper.Map<TradeDTO>(t);
                dtos.Add(dto);
            }

            return dtos;
        }

        public void Save(TradeDTO dto)
        {
            Trade trade = _mapper.Map<Trade>(dto);
            _tradesRepository.Save(trade);
        }

        public void Update(TradeDTO dto)
        {
            Trade trade = _mapper.Map<Trade>(dto);
            _tradesRepository.Update(trade);
        }

        public void Delete(int id)
        {
            _tradesRepository.Delete(id);
        }
    }
}
