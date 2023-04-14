using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Versioning;
using DG.Tweening;
using UnityEngine;


public class StarUIMovement : MonoBehaviour
{
    public Movement player;
    public GameObject star;
    public GameObject start;
    public GameObject destinationIcon;

    public void SpawnMoveStar()
    {
        //starIcon = Resources.Load("StarIconPrefab");
        GameObject starInstance = Instantiate(star, start.transform.position, Quaternion.identity);
        starInstance.transform.SetParent(destinationIcon.transform.parent);
        starInstance.SetActive(true);

        starInstance.transform.localScale = destinationIcon.transform.localScale;

        // Destroy the ring UI element after a short delay*/
        starInstance.transform.DOLocalMove(new Vector3(-122,4,0), 0.3f);
        //starInstance.transform.DOLocalMove(destinationIcon.transform.position, 1f);
        Destroy(starInstance, 0.35f);
    }
}
