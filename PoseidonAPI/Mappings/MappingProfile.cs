using AutoMapper;
using PoseidonAPI.Dtos;
using PoseidonAPI.Model;

using PoseidonAPI.Contracts.Bid;
using PoseidonAPI.Contracts.CurvePoint;
using PoseidonAPI.Contracts.Rating;
using PoseidonAPI.Contracts.Rule;
using PoseidonAPI.Contracts.Trade;


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

            //dto to contract Bid
            CreateMap<BidDTO, CreateBidRequest>();
            CreateMap<BidDTO, UpsertBidRequest>();
            CreateMap<BidDTO, BidResponse>();

            //contract to dto CurvePoint
            CreateMap<CreateCurvePointRequest, CurvePointDTO>();
            CreateMap<UpsertCurvePointRequest, CurvePointDTO>();
            CreateMap<CurvePointResponse, CurvePointDTO>();

            //dto to contract CurvePoint
            CreateMap<CurvePointDTO, CreateCurvePointRequest>();
            CreateMap<CurvePointDTO, UpsertCurvePointRequest>();
            CreateMap<CurvePointDTO, CurvePointResponse>();

            //contract to dto Rating
            CreateMap<CreateRatingRequest, RatingDTO>();
            CreateMap<UpsertRatingRequest, RatingDTO>();
            CreateMap<RatingResponse, RatingDTO>();

            //dto to contract Rating
            CreateMap<RatingDTO, CreateRatingRequest>();
            CreateMap<RatingDTO, UpsertRatingRequest>();
            CreateMap<RatingDTO, RatingResponse>();

            //contract to dto Rule
            CreateMap<CreateRuleRequest, RuleDTO>();
            CreateMap<UpsertRuleRequest, RuleDTO>();
            CreateMap<RuleResponse, RuleDTO>();

            //dto to contract Rule
            CreateMap<RuleDTO, CreateRuleRequest>();
            CreateMap<RuleDTO, UpsertRuleRequest>();
            CreateMap<RuleDTO, RuleResponse>();

            //contract to dto Trade
            CreateMap<CreateTradeRequest, TradeDTO>();
            CreateMap<UpsertTradeRequest, TradeDTO>();
            CreateMap<TradeResponse, TradeDTO>();

            //dto to contract Trade
            CreateMap<TradeDTO, CreateTradeRequest>();
            CreateMap<TradeDTO, UpsertTradeRequest>();
            CreateMap<TradeDTO, TradeResponse>();
        }
    }
}
