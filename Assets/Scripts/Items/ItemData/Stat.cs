using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class Stat
    {
        public string statName = "";

        [SerializeField]
        [HorizontalGroup("Values")]
        [LabelWidth(65f)]
        [FormerlySerializedAs("baseValue")]
        public float Value;

        [JsonConstructor]
        public Stat(string name, float value)
        {
            statName = name;
            Value = value;
        }
        public Stat(string name, int value)
        {
            statName = name;
            Value = (float)value;
        }
    }
}