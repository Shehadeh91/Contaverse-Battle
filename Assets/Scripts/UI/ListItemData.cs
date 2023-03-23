using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Contaquest.UI
{
    public class ListItemData : ScriptableObject
    {
        public string title;
        public Sprite sprite;

        [Header("Events")]
        [HideInInlineEditors] public Action onSelected;
        [HideInInlineEditors] public Action onDeselected;
    }

}
