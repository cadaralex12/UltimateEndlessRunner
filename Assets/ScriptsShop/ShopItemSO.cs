using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="ShopMenu", menuName="ScriptableObjectsShop/New Shop Item", order=1)]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public string description;
    public int basecost;
   
}
