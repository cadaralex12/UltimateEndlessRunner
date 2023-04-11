using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostButton : MonoBehaviour
{
    public Movement player;

    public void ActivateBoost()
    {
        player.boostPressed = true;
        player.Boost();
    }
}
