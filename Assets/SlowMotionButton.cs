using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionButton : MonoBehaviour
{
    public Movement player;

    public void ActivateSlowMotion()
    {
        player.slowMoPressed = true;
        player.BulletTime();
    }
}
