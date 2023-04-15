using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sky : MonoBehaviour
{
    public float skyboxRotation = 2f;
    void Update()
    {
        RenderSettings.skybox.SetFloat("Rotation", Time.time * skyboxRotation);
    }
}
