using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSelector : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    public int mat = 0;
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = this.GetComponent<Renderer>();
        if (mat == 0)
        {
            meshRenderer.material = mat1;
        }
        else
        {
            meshRenderer.material = mat2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
