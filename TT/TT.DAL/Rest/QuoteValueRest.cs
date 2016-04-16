using RestSharp.Deserializers;

namespace TT.DAL.Rest
{
    //{"S":"EURJPY","G":"Forex majors","D":3,"B":122.823,"A":122.84,"DL":122.582,"DH":123.561,"CHG":-43.4,"CHG%":-0.35,"AD":0}
    public class QuoteValueRest
    {
        [DeserializeAs(Name = "S")]
        public string Symbol { get; set; }
        public string G { get; set; }
        public decimal D { get; set; }
        [DeserializeAs(Name = "B")]
        public decimal Bid { get; set; }
        [DeserializeAs(Name = "A")]
        public decimal Ask { get; set; }
        public decimal DL { get; set; }
        public decimal DH { get; set; }
        [DeserializeAs(Name = "CHG")]
        public decimal ChangePoints { get; set; }
        [DeserializeAs(Name = "CHG%")]
        public decimal ChangePercent { get; set; }
        public decimal AD { get; set; }
    }
}
