using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {

    Material mat;
    public  float strength = 1.0f;

    private void Start() {
        mat = GetComponent<Renderer>().material;
        
        mat.SetFloat("_DissolveAmount", 1f);
    }

    private void Update() {
        if (strength > 0)
        {
            strength -= 0.3f * Time.deltaTime;
            mat.SetFloat("_DissolveAmount", strength);
        }
        
    }
}