using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CurrentLayerCamShaderHandler : MonoBehaviour
{
    public Shader currentLayerStencil;

    private void OnEnable()
    {
        if (currentLayerStencil != null)
            GetComponent<Camera>().SetReplacementShader(currentLayerStencil, "");
    }

    private void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader(); // Probably redundant
    }
    void Start()
    {
        
    }
}
