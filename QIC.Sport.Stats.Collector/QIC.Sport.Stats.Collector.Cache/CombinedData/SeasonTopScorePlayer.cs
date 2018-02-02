using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CombinedData
{
    public class SeasonTopScorePlayer
    {
        public string SeasonId;
        public string PlayerId;
        public string Penalties;
        //  其他信息可由SeasonId联合PlayerId查询并生成
    }
}
