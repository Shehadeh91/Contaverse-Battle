using System;
using System.Collections.Generic;
using Contaquest.Metaverse.Robot;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Contaquest.Metaverse.Rooms
{
    public class RoomManager : GenericSingleton<RoomManager>
    {
        [TabGroup("References")] [SerializeField] private int defaultActivated = 0;
        [TabGroup("References")] [SerializeField] private bool instantiateRooms;
        [TabGroup("References")] [SerializeField] [ShowIf("instantiateRooms")] private RoomSO[] rooms;
        [TabGroup("References")] [SerializeField] [HideIf("instantiateRooms")] private List<RoomController> roomControllers = new List<RoomController>();
        [TabGroup("References")] public RobotController robotController;

        [TabGroup("Events")] [SerializeField] private UnityEvent onLoadAllRoomsLoaded = new UnityEvent();

        private void Start()
        {
            //Debug.Log("Initialized RoomManager");
            if(instantiateRooms)
                LoadAllRooms();

            InitializeAllRooms();

            roomControllers[defaultActivated].StartActivateRoom();

            onLoadAllRoomsLoaded?.Invoke();
        }

        public void LoadAllRooms()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                RoomController roomController = Instantiate(rooms[i].roomPrefab);
                roomControllers.Add(roomController);

                if (i == 0)
                    continue;

                roomController.transform.position = roomControllers[i-1].endpoint.transform.position - roomController.startPoint.localPosition;
            }
        }

        private void InitializeAllRooms()
        {
            for (int i = 0; i < roomControllers.Count; i++)
            {
                roomControllers[i].Initialize();
            }
        }
    }
}
