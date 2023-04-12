using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldButton : MonoBehaviour
{
    public Movement player;
    
    public void ActivateShield()
    {
        player.shieldPressed = true;
        player.Shield();
    }
}
