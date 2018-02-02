using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;
using System.Security.Cryptography;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class OrganizerParam : BRBaseParam
    {
        public string IndexUrl;
        public override string GetKey()
        {
            return ((RBHandleType)HandleType).ToString();
        }

        public override string GetUrl()
        {
            var sha1 = EncryptToSHA1(IndexUrl);
            return BaseUrl + IndexUrl + "&callback=" + sha1;
        }
    }
}
