using AutoMapper;
using PoseidonAPI.Repositories;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;
using System.Collections.Generic;

namespace PoseidonAPI.Services
{
    public class RatingService : IService<RatingDTO>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Rating> _ratingRepository;

        public RatingService(IRepository<Rating> ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public RatingDTO Get(int id)
        {
            Rating rating = _ratingRepository.Get(id);
            RatingDTO dto = _mapper.Map<RatingDTO>(rating);
            return dto;
        }

        public IEnumerable<RatingDTO> GetAll()
        {
            List<RatingDTO> dtos = new List<RatingDTO>();
            IEnumerable<Rating> ratings = _ratingRepository.GetAll();
            foreach (Rating r in ratings)
            {
                RatingDTO dto = _mapper.Map<RatingDTO>(r);
                dtos.Add(dto);
            }

            return dtos;
        }

        public RatingDTO Save(RatingDTO dto)
        {
            Rating rating = _mapper.Map<Rating>(dto);
            RatingDTO ratingDto = _mapper.Map<RatingDTO>(_ratingRepository.Save(rating));
            return ratingDto;
        }

        public void Update(RatingDTO dto)
        {
            Rating rating = _mapper.Map<Rating>(dto);
            _ratingRepository.Update(rating);
        }

        public void Delete(int id)
        {
            _ratingRepository.Delete(id);
        }
    }
}
