using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CombinedData;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class SeasonTableInfo : BaseCacheEntity
    {
        public string TableId;
        public string TableName;
        public List<string> TeamIdList = new List<string>();
        public List<SeasonTableRank> TableRankList = new List<SeasonTableRank>();

        public void CompareSet(string tableId, string tableName)
        {
            if (tableId == TableId && tableName == TableName) return;
            TableId = tableId;
            TableName = tableName;
        }

        public void CompareTableRank(List<SeasonTableRank> list)
        {
            bool isChanged;
            if (TableRankList.Count != list.Count)
            {
                TableRankList = list;
                isChanged = true;
            }
            else if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (TableRankList[i].TeamId == list[i].TeamId && TableRankList[i].TeamPoints == list[i].TeamPoints && TableRankList[i].Description == list[i].Description) continue;
                    TableRankList = list;
                    isChanged = true;
                    break;
                }
            }
        }

        public void CompareTeamIdList(List<string> list)
        {
            TeamIdList = list;
        }
        public override string GetKey()
        {
            return TableId;
        }
    }
}
