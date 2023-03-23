using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOverrideController : GenericSingleton<MaterialOverrideController>
{
    public bool shouldOverrideMaterials = false;
    [SerializeField]
    private Material[] baseMaterials;
    [SerializeField]
    private Material[] overrideMaterials;

    Dictionary<int, Material> materialDictionary = new Dictionary<int, Material>();

    public override void Awake()
    {
        base.Awake();
        for (int i = 0; i < baseMaterials.Length; i++)
        {
            if(baseMaterials[i] != null && overrideMaterials[i] != null)
            {
                materialDictionary.Add(baseMaterials[i].GetInstanceID(), overrideMaterials[i]);
            }
        }
    }
    
    public Material GetMaterialFromBaseMaterial(int instanceID)
    {
        if(!shouldOverrideMaterials)
            return null;

        if(materialDictionary.TryGetValue(instanceID, out Material returnMaterial))
            return returnMaterial;
        
        return null;
    }
}