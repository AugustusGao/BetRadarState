using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    /// <summary>
    /// 队伍在不同赛季下的队员信息
    /// </summary>
    public class TeamPlayers : BaseCacheEntity
    {
        public string SeasonId;
        public string TeamId;
        public List<string> BestPlayerList = new List<string>();      //  最佳名单list名次PlayerId
        public List<string> CardPlayerList = new List<string>();      //  牌list名次PlayerId
        public List<string> AssistsPlayerList = new List<string>();   //  助攻list名次PlayerId
        public Dictionary<string, InjurePlayer> InjurePlayerDic = new Dictionary<string, InjurePlayer>();    //  kv = PlayerId--伤停状态描述
        public List<string> AllPlayerIdList = new List<string>();     //  球队名单
        public Dictionary<string, List<string>> CompareSetPlayerIdList(List<string> list)
        {
            var adds = list.Except(AllPlayerIdList).ToList();
            var dels = AllPlayerIdList.Except(list).ToList();

            AllPlayerIdList = list;
            return new Dictionary<string, List<string>> { { "add", adds }, { "del", dels } };
        }

        public void CompareSetInjurePlayerList(List<InjurePlayer> list)
        {
            var dic = new Dictionary<string, InjurePlayer>();
            foreach (var l in list)
            {
                if (dic.ContainsKey(l.PlayerId)) continue;
                dic.Add(l.PlayerId, l);
            }
            var adds = dic.Keys.Except(InjurePlayerDic.Keys).ToList();
            var dels = InjurePlayerDic.Keys.Except(dic.Keys).ToList();

            InjurePlayerDic = dic;
        }

        /// <summary>
        /// 对比队员各种排名
        /// </summary>
        /// <param name="type">"Best","Card","Assists"</param>
        /// <param name="list">最新排名数据</param>
        public void CompareBestCardAssistsPlayerList(string type, List<string> list)
        {
            if (list == null) return;
            List<string> oldList = type == "Best" ? BestPlayerList : type == "Card" ? CardPlayerList : AssistsPlayerList;

            bool isChanged;
            if (list.Count != oldList.Count)
            {
                oldList = list;
                isChanged = true;
            }
            else if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (oldList[i] != list[i])
                    {
                        isChanged = true;
                        oldList = list;
                        break;
                    }
                }
            }
        }
        public override string GetKey()
        {
            return TeamId + "_" + SeasonId;
        }
    }

    public class InjurePlayer
    {
        public string PlayerId;
        public string Missing;
        public string Status;

    }
}
