using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class RobotStatsCollection
    {
        public RobotStats robotStats;
        //public List<RobotStats> robotStatsList;

        public RobotStatsCollection(RobotPart[] robotParts)
        {
            robotStats = new RobotStats();
            //Debug.Log("creating stats collection");
            for (int i = 0; i < robotParts.Length; i++)
            {
                if (robotParts[i] is null)
                    continue;
                AddRobotStats(robotParts[i].stats);
            }
        }

        public void RemoveRobotStats(RobotStats oldRobotStats)
        {
            foreach (var stat in oldRobotStats.stats)
            {
                int statHashCode = stat.statName.GetHashCode();
                if (robotStats.statsDictionary.ContainsKey(statHashCode))
                    robotStats.statsDictionary[statHashCode].Value -= stat.Value;
            }
        }

        public void AddRobotStats(RobotStats newRobotStats)
        {
            //Debug.Log("Adding stats" + newRobotStats.stats.Count);
            foreach (var stat in newRobotStats.stats)
            {
                int statHashCode = stat.statName.GetHashCode();
                if (robotStats.statsDictionary.ContainsKey(statHashCode))
                {
                    robotStats.statsDictionary[statHashCode].Value += stat.Value;
                    //Debug.Log("Adding value to old stat " + stat.statName);
                }
                else
                {
                    robotStats.statsDictionary.Add(statHashCode, new Stat(stat.statName, stat.Value));
                    //Debug.Log("Adding new stat " + stat.statName);
                }
            }
        }

        public float GetStatValue(string statName)
        {
            return robotStats.GetStat(statName).Value;
        }
    }
}
