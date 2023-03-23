using System.Collections.Generic;
using Contaquest.Metaverse.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Contaquest.UI.Customization;
namespace Contaquest.Metaverse.Robot.Customization
{
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        private Material material;
        [SerializeField] private int materialIndex = 0;

        private static readonly int dirtinessPropertyId = Shader.PropertyToID("Dirtiness");
        private static readonly int primaryColorPropertyId = Shader.PropertyToID("PrimaryColor");
        private static readonly int secondaryColorPropertyId = Shader.PropertyToID("SecondaryColor");
        private static readonly int tertiaryColorPropertyId = Shader.PropertyToID("TertiaryColor");

        public void Initialize(RobotPart robotPart)
        {
            // Debug.Log("Initializing Materials");
            if (meshRenderer == null)
                return;
            if(material == null)
            {
                GetMaterial();
            }
            material.SetColor(primaryColorPropertyId, robotPart.primaryColor);
            material.SetColor(secondaryColorPropertyId, robotPart.secondaryColor);
            material.SetColor(tertiaryColorPropertyId, robotPart.tertiaryColor);
        }

        public void SetDirtiness(float value)
        {
            // Debug.Log($"Setting Dirtiness to {value}");

            if (meshRenderer == null)
                return;
            if(material == null)
                GetMaterial();

            material.SetFloat(dirtinessPropertyId, value);
        }
        public void GetMaterial()
        {
            if(materialIndex < meshRenderer.materials.Length)
                material = meshRenderer.materials[materialIndex];
            else
            {
                Debug.LogError("Material index is longer than array");
                return;
            }
        }
    }
}