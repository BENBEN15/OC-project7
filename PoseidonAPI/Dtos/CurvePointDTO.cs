using System;

namespace PoseidonAPI.Dtos
{
    public class CurvePointDTO
    {
        public int CurvePointId { get; set; }
        public int CurveId { get; set; }
        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? Value { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}