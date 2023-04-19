using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFollow: MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    private float y;
    public float speedFollow = 5f;
    public int postProcessingGlobal;
    public PostProcessVolume postProcessVolume;
    //private float fiveSideOffset = -20f;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;

        postProcessingGlobal = PlayerPrefs.GetInt("PostProcessingSetting");
        postProcessVolume = GetComponent<PostProcessVolume>();
        
        if (postProcessingGlobal == 1)
        {
            postProcessVolume.weight = 1;
        }
        else
        {
            postProcessVolume.weight = 0;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 followPos = target.position + offset;
        //followPos.z += fiveSideOffset;
        RaycastHit hit;
        if (Physics.Raycast(target.position, Vector3.down, out hit, 2.5f))
        {
            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime * speedFollow);
        }
        else
        {
            y = Mathf.Lerp(y, target.position.y, Time.deltaTime * speedFollow);
        }
        followPos.y = offset.y + y;
        transform.position = followPos;

    }
}
