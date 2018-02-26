using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class TeamEntity : BaseCacheEntity
    {
        public string TeamId;
        public string TeamName;
        public string Venue;            //  主场
        public string Manager;          //  教练
        public string Mark;             //  队标
        public override string GetKey()
        {
            return TeamId;
        }

        public void CompareSetTeamEntity(TeamEntity teamEntity)
        {
            if (Equals(teamEntity)) return;
            TeamName = teamEntity.TeamName;
            Venue = teamEntity.Venue;
            Manager = teamEntity.Manager;
            Mark = teamEntity.Mark;
        }

        public override bool Equals(object entity)
        {
            TeamEntity teamEntity = entity as TeamEntity;
            return
                TeamId == teamEntity.TeamId &&
                TeamName == teamEntity.TeamName &&
                Venue == teamEntity.Venue &&
                Manager == teamEntity.Manager &&
                Mark == teamEntity.Mark;
        }
    }
}
