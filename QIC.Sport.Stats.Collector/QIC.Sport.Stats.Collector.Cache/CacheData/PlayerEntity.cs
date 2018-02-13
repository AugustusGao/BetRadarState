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
        public List<TransferHistory> TransferHistorys;

        public void CompareInfo(PlayerEntity pe)
        {
            if (this.Equal(pe)) return;

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

        public void CompareTransferHistory(TransferHistory th)
        {

        }

        public override bool Equal(BaseCacheEntity entity)
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
        public int TeamId;
        public string DuringTime;
        public string Description;

    }
}
