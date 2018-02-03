using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.BetRadar.Handle;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class BRBaseParam : BaseParam
    {
        protected string BaseUrl = "http://www.stats.betradar.com/s4/";
        protected string CustomUrl;
        public string SportId = "1";
        public string ContinentId;
        public string OrganizerId;
        public string SeasonId;
        public string TeamId;
        public string PlayerId;

        protected string EncryptToSHA1(string str)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = Encoding.UTF8.GetBytes(str);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1 Encrypt Error" + ex.Message);
            }
        }
        public virtual string GetUrl() { return null; }

        public T CopyBaseParam<T>() where T : BRBaseParam, new()
        {
            var t = new T()
            {
                SportId = "1",
                ContinentId = ContinentId,
                OrganizerId = OrganizerId,
                SeasonId = SeasonId,
                TeamId = TeamId,
                PlayerId = PlayerId,
            };
            return t;
        }
    }
}
