using AutoMapper;
using PoseidonAPI.Repositories;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;
using System.Collections.Generic;

namespace PoseidonAPI.Services
{
    public class BidService : IService<BidDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Bid> _bidsRepository;

        public BidService(IRepository<Bid> bidsRepository, IMapper mapper)
        {
            _bidsRepository = bidsRepository;
            _mapper = mapper;
        }

        public BidDTO Get(int id)
        {
            Bid bid = _bidsRepository.Get(id);
            BidDTO dto = _mapper.Map<BidDTO>(bid);
            return dto;
        }

        public IEnumerable<BidDTO> GetAll()
        {
            List<BidDTO> dtos = new List<BidDTO>();
            IEnumerable<Bid> bids = _bidsRepository.GetAll();
            foreach (Bid b in bids)
            {
                BidDTO dto = _mapper.Map<BidDTO>(b);
                dtos.Add(dto);
            }

            return dtos;
        }

        public BidDTO Save(BidDTO dto)
        {
            Bid bid = _mapper.Map<Bid>(dto);
            BidDTO bidDTO = _mapper.Map<BidDTO>(_bidsRepository.Save(bid));
            return bidDTO;
        }

        public void Update(BidDTO dto)
        {
            Bid bid = _mapper.Map<Bid>(dto);
            _bidsRepository.Update(bid);
        }

        public void Delete(int id)
        {
            _bidsRepository.Delete(id);
        }
    }
}
