using System.Collections.Generic;
using Contaquest.Metaverse.Data;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.Text;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class RobotStats
    {
        [ListDrawerSettings(ListElementLabelName = "statName", Expanded = true)]
        public List<Stat> stats = new List<Stat>();

        [JsonIgnore] public Dictionary<int, Stat> statsDictionary = new Dictionary<int, Stat>();

        [Button]
        public void PrintDictionary()
        {
            foreach (var stat in statsDictionary)
            {
                Debug.Log($"Stat Name: {stat.Value.statName} Value: {stat.Value.Value}");
            }
            if (statsDictionary.Count == 0)
                Debug.Log("Empty Dictionary");
        }

        //this will initialize the robotstats after it has been deseralized
        public void Initialize()
        {
            //if (stats.Count == statsDictionary.Count)
                //return;
            statsDictionary = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
            {
                statsDictionary.Add(stat.statName.GetHashCode(), stat);
            }
        }
        public void AddStat(Stat stat)
        {
            stats.Add(stat);
            statsDictionary.Add(stat.statName.GetHashCode(), stat);
        }

        public Stat GetStat(string statName)
        {
            statsDictionary.TryGetValue(statName.GetHashCode(), out Stat stat);
            return stat;
        }
    }
}
