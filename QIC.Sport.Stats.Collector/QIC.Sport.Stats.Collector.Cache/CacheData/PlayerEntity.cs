using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache.CacheData
{
    public class PlayerEntity : BaseCacheEntity
    {
        public string PlayerId;
        public string FullName;
        public string Country;
        public string SecondaryCountry;
        public string Birth;
        public string Age;
        public string Height;
        public string Weight;
        public string Position;
        public string ShirtNumber;
        public string TeamName;
        public string PreferredFoot;
        public List<TransferHistory> TransferHistorys = new List<TransferHistory>();

        public void CompareSetInfo(PlayerEntity pe)
        {
            if (this.Equals(pe)) return;

            FullName = pe.FullName;
            Country = pe.Country;
            SecondaryCountry = pe.SecondaryCountry;
            Birth = pe.Birth;
            Age = pe.Age;
            Height = pe.Height;
            Weight = pe.Weight;
            Position = pe.Position;
            ShirtNumber = pe.ShirtNumber;
            TeamName = pe.TeamName;
            PreferredFoot = pe.PreferredFoot;
        }

        public void CompareSetTransferHistory(TransferHistory th)
        {
            var oldTh = TransferHistorys.FirstOrDefault(o => o.DuringTime == th.DuringTime); //  时间是唯一的
            if (oldTh == null) TransferHistorys.Add(th);
            else if (th.TeamId != oldTh.TeamId || th.Description != oldTh.Description)
            {
                oldTh.TeamId = th.TeamId;
                oldTh.Description = th.Description;
            }
        }

        public override bool Equals(object entity)
        {
            var pe = entity as PlayerEntity;

            return FullName == pe.FullName &&
                   Country == pe.Country &&
                   SecondaryCountry == pe.SecondaryCountry &&
                   Birth == pe.Birth &&
                   Age == pe.Age &&
                   Height == pe.Height &&
                   Weight == pe.Weight &&
                   Position == pe.Position &&
                   ShirtNumber == pe.ShirtNumber &&
                   TeamName == pe.TeamName &&
                   PreferredFoot == pe.PreferredFoot;
        }
    }

    public class TransferHistory
    {
        public string TeamId;
        public string DuringTime;
        public string Description;

    }
}
