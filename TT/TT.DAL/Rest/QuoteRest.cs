using System.Collections.Generic;
using RestSharp.Deserializers;

namespace TT.DAL.Rest
{
    //{"name":"Forex Majors",
    //"values":
    //[{"S":"EURJPY","G":"Forex majors","D":3,"B":122.823,"A":122.84,"DL":122.582,"DH":123.561,"CHG":-43.4,"CHG%":-0.35,"AD":0},
    //{"S":"AUDUSD","G":"Forex majors","D":5,"B":0.7725,"A":0.77266,"DL":0.76801,"DH":0.77338,"CHG":34.9,"CHG%":0.45,"AD":0},
    //{"S":"EURUSD","G":"Forex majors","D":5,"B":1.12828,"A":1.12841,"DL":1.1245,"DH":1.13166,"CHG":18.1,"CHG%":0.16,"AD":0},
    //{"S":"USDCAD","G":"Forex majors","D":5,"B":1.28144,"A":1.28161,"DL":1.27962,"DH":1.29023,"CHG":-21.6,"CHG%":-0.17,"AD":0},
    //{"S":"EURGBP","G":"Forex majors","D":5,"B":0.7939,"A":0.79403,"DL":0.79347,"DH":0.79756,"CHG":-15.4,"CHG%":-0.19,"AD":0},
    //{"S":"GBPUSD","G":"Forex majors","D":5,"B":1.42035,"A":1.4205,"DL":1.4132,"DH":1.42413,"CHG":54.4,"CHG%":0.38,"AD":0},
    //{"S":"USDJPY","G":"Forex majors","D":3,"B":108.805,"A":108.819,"DL":108.593,"DH":109.726,"CHG":-55.2,"CHG%":-0.51,"AD":0},
    //{"S":"GBPJPY","G":"Forex majors","D":3,"B":154.467,"A":154.489,"DL":154.069,"DH":155.295,"CHG":-26.4,"CHG%":-0.17,"AD":0},
    //{"S":"NZDUSD","G":"Forex majors","D":5,"B":0.692,"A":0.69218,"DL":0.68364,"DH":0.69263,"CHG":81.3,"CHG%":1.17,"AD":0}]
    //}
    public class QuoteRest
    {
        [DeserializeAs(Name = "name")]
        public string Name { get; set; }
        [DeserializeAs(Name = "values")]
        public List<QuoteValueRest> Values { get; set; }
    }
}
