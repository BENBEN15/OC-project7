using AutoMapper;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;


namespace PoseidonAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bid, BidDTO>();
            CreateMap<CurvePoint, CurvePointDTO>();
            CreateMap<Rating, RatingDTO>();
            CreateMap<Rule, RuleDTO>();
            CreateMap<Trade, TradeDTO>();

            CreateMap<BidDTO, Bid>();
            CreateMap<CurvePointDTO, CurvePoint>();
            CreateMap<RatingDTO, Rating>();
            CreateMap<RuleDTO, Rule>();
            CreateMap<TradeDTO, Trade>();
        }
    }
}
