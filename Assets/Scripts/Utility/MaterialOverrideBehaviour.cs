using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOverrideBehaviour : MonoBehaviour
{
    [SerializeField]
    private Renderer[] renderers;
    [SerializeField]
    private bool autoAssignRenderers = true;

    private void Start()
    {
        if(!MaterialOverrideController.Instance.shouldOverrideMaterials)
            return;
        
        if(autoAssignRenderers)
        {
            renderers = GetComponentsInChildren(typeof(Renderer), true) as Renderer[];
        }

        foreach (Renderer renderer in renderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material material = MaterialOverrideController.Instance.GetMaterialFromBaseMaterial(renderer.materials[i].GetInstanceID());
                
                if(material != null)
                {
                    renderer.materials[i] = material;
                }
            }
        }
        Destroy(this);
    }
}