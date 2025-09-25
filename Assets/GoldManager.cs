using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    public int goldCount = 0;
    public Text goldText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    public void AddGold(int amount)
    {
        goldCount += amount;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = "GOLD:  " + goldCount.ToString(); 

    }



}
