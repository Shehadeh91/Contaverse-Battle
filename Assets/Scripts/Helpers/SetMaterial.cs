﻿using UnityEngine;
public class SetMaterial : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material material = null;
    [SerializeField] private int index = 0;

    public void SetRendererMaterial()
    {
        Material[] mats = meshRenderer.materials;
        mats[index] = material;
        meshRenderer.materials = mats;
    }
}
