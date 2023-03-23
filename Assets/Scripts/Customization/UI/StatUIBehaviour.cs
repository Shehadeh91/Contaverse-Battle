using UnityEngine;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using TMPro;
using Contaquest.Metaverse.Data;

namespace Contaquest.UI.Customization
{
    public class StatUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statName;
        [SerializeField] private TextMeshProUGUI statValue;
        [SerializeField] private int roundDecimals = 1;

        public void DisplayStat(Stat stat)
        {
            statName.text = stat.statName;
            statValue.text = System.Math.Round((decimal)stat.Value, roundDecimals).ToString();
        }
    }
}
