using UnityEngine;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;

namespace Contaquest.UI.Customization
{
    public class RobotStatsUIController : MonoBehaviour
    {
        [ShowInInspector] private RobotStats robotStats;
        [SerializeField] private StatUIBehaviour statUIPrefab;
        [SerializeField] private Transform statsParentTransform;
        private List<StatUIBehaviour> statUIBehaviours = new List<StatUIBehaviour>();

        public void DisplayRobotStats(RobotStats newRobotStats)
        {
            robotStats = newRobotStats;
            // newRobotStats.PrintDictionary();
            while(newRobotStats.statsDictionary.Count != statUIBehaviours.Count)
            {
                if(newRobotStats.statsDictionary.Count > statUIBehaviours.Count)
                {
                    StatUIBehaviour statUIBehaviour = Instantiate(statUIPrefab, statsParentTransform);
                    statUIBehaviours.Add(statUIBehaviour);
                    //Debug.Log("Created new stat");
                }
                else
                {
                    Destroy(statUIBehaviours[statUIBehaviours.Count - 1]);
                    //Debug.Log("deleted stat");
                    statUIBehaviours.RemoveAt(statUIBehaviours.Count - 1);
                }
            }

            int index = 0;
            //Debug.Log("Starting to display stats" + gameObject.name, gameObject);
            foreach (var stat in newRobotStats.statsDictionary)
            {
                //Debug.Log("Displaying Stat " + stat.Value.statName);
                statUIBehaviours[index].DisplayStat(stat.Value);
                index++;
            }
        }
    }
}