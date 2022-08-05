using AutoMapper;
using PoseidonAPI.Repositories;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;
using System.Collections.Generic;

namespace PoseidonAPI.Services
{
    public class CurvePointService : IService<CurvePointDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CurvePoint> _cpRepository;

        public CurvePointService(IRepository<CurvePoint> cpRepository, IMapper mapper)
        {
            _cpRepository = cpRepository;
            _mapper = mapper;
        }

        public CurvePointDTO Get(int id)
        {
            CurvePoint cp = _cpRepository.Get(id);
            CurvePointDTO dto = _mapper.Map<CurvePointDTO>(cp);
            return dto;
        }

        public IEnumerable<CurvePointDTO> GetAll()
        {
            List<CurvePointDTO> dtos = new List<CurvePointDTO>();
            IEnumerable<CurvePoint> cps = _cpRepository.GetAll();
            foreach (CurvePoint cp in cps)
            {
                CurvePointDTO dto = _mapper.Map<CurvePointDTO>(cp);
                dtos.Add(dto);
            }

            return dtos;
        }

        public void Save(CurvePointDTO dto)
        {
            CurvePoint cp = _mapper.Map<CurvePoint>(dto);
            _cpRepository.Save(cp);
        }

        public void Update(CurvePointDTO dto)
        {
            CurvePoint cp = _mapper.Map<CurvePoint>(dto);
            _cpRepository.Update(cp);
        }

        public void Delete(int id)
        {
            _cpRepository.Delete(id);
        }
    }
}
