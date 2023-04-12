using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopManager : MonoBehaviour
{
    public int coins=0;
    public TMP_Text coinUI;
    public ShopItemSO[] ShopItemsSO;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGO;
    public Button[] myPurchaseBtns;

    void Start()
    {
        coins = PlayerPrefs.GetInt("totalCoins");
        for(int i=0; i<ShopItemsSO.Length; i++){
            shopPanelsGO[i].SetActive(true);
        }
        coinUI.text = "Stars: " + coins.ToString();
        LoadPanels();
        CheckPurchaseable();
    }
    public void LoadPanels(){
        for(int i=0; i < ShopItemsSO.Length; i++){
            shopPanels[i].titleText.text = ShopItemsSO[i].title;
            shopPanels[i].descriptionText.text = ShopItemsSO[i].description;
            shopPanels[i].costText.text = "Stars: " + ShopItemsSO[i].basecost.ToString();
        }
    }
    public void CheckPurchaseable()
    {
    for(int i=0;i<ShopItemsSO.Length;i++){
        if(coins>=ShopItemsSO[i].basecost){
            myPurchaseBtns[i].interactable = true;
        }
        else{
            myPurchaseBtns[i].interactable = false;
        }
    }
}

    public void PurchaseItem(int btnNo)
    {
        if (coins >= ShopItemsSO[btnNo].basecost)
        {
            coins = coins - ShopItemsSO[btnNo].basecost;
            PlayerPrefs.SetInt("totalCoins", coins);
            coinUI.text = "Stars: " + coins.ToString();
            CheckPurchaseable();
        }
    }
}

