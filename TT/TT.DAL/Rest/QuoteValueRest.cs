using RestSharp.Deserializers;

namespace TT.DAL.Rest
{
    //{"S":"EURJPY","G":"Forex majors","D":3,"B":122.823,"A":122.84,"DL":122.582,"DH":123.561,"CHG":-43.4,"CHG%":-0.35,"AD":0}
    public class QuoteValueRest
    {
        [DeserializeAs(Name = "S")]
        public string SymbolName { get; set; }
        public string G { get; set; }
        public decimal D { get; set; }
        [DeserializeAs(Name = "B")]
        public decimal BidValue { get; set; }
        [DeserializeAs(Name = "A")]
        public decimal AskValue { get; set; }
        public decimal DL { get; set; }
        public decimal DH { get; set; }
        [DeserializeAs(Name = "CHG")]
        public decimal ChangePoints { get; set; }
        [DeserializeAs(Name = "CHG%")]
        public decimal GhangePercent { get; set; }
        public decimal AD { get; set; }
    }
}
