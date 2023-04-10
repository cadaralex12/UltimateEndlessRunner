using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public int respawn;
    public int babyMode = 1;
    public float timer = 0;
    public float seconds = 0;

    private bool edgeSet = false;
    private Vector3 edgeVertex = Vector3.zero;
    private Vector2 edgeUV = Vector2.zero;
    private Plane edgePlane = new Plane();

    public int CutCascades = 1;
    public float ExplodeForce = 0;

    public Movement player;

    void reloadScene()
    {
        SceneManager.LoadScene(respawn);
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.CompareTag("Death"))
            {
                Invoke("reloadScene", 0);
            }
            player = other.gameObject.GetComponent<Movement>();
            if (player.lives > 1)
            {

                player.lives--;
                player.y = player.jumpPower * 2/3;
                player.m_Animator.Play("Floating");
                player.hurtCounter = 1f;
                this.gameObject.SetActive(false);
            }
            else
            {
                Invoke("reloadScene", 0);//this will happen after a delay of 1.5 seconds
            }
        }

    }
}
