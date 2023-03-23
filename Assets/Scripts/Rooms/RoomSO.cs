using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Contaquest.UI;
using UnityEngine.Events;

namespace Contaquest.Metaverse.Rooms
{
    [CreateAssetMenu(menuName = "Data/New Room")]
    public class RoomSO : ListItemData
    {
        public RoomController roomPrefab;

        //public UnityEvent onRoomActivated;

        public void LoadScene()
        {

        }
    }
}
