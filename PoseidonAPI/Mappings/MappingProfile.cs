using AutoMapper;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;

using PoseidonAPI.Contracts.Bid;


namespace PoseidonAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Entity to DTO
            CreateMap<Bid, BidDTO>();
            CreateMap<CurvePoint, CurvePointDTO>();
            CreateMap<Rating, RatingDTO>();
            CreateMap<Rule, RuleDTO>();
            CreateMap<Trade, TradeDTO>();

            //DTO to Entity
            CreateMap<BidDTO, Bid>();
            CreateMap<CurvePointDTO, CurvePoint>();
            CreateMap<RatingDTO, Rating>();
            CreateMap<RuleDTO, Rule>();
            CreateMap<TradeDTO, Trade>();

            //contract to dto Bid
            CreateMap<CreateBidRequest, BidDTO>();
            CreateMap<UpsertBidRequest, BidDTO>();
            CreateMap<BidResponse, BidDTO>();

            //dot to contract Bid
            CreateMap<BidDTO, CreateBidRequest>();
            CreateMap<BidDTO, UpsertBidRequest>();
            CreateMap<BidDTO, BidResponse>();
        }
    }
}
