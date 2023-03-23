using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Contaquest.Metaverse.Combat2
{
    public class ArenaDataManager : GenericSingleton<ArenaDataManager>
    {
        [SerializeField] private List<ArenaData> arenaDatas = new List<ArenaData>();

        public ArenaData GetArenaDataByName(string arenaName)
        {
            return arenaDatas.FirstOrDefault((arenaData) => arenaData.arenaName == arenaName);
        }
    }
}