using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    public GameObject TowerPrefab { get { return towerPrefab; } }

    [SerializeField] private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

    [SerializeField] private int price;
    public int Price { get { return price; } }

    [SerializeField] private Text priceTxt;

    private void Start()
    {
        priceTxt.text = Price + "$";

        Game_Manager.Instance.Changed += new CurrencyChanged(PriceCheck);   // event CurrencyChanged() takes in a method for its parameter to call
    }

    private void PriceCheck()   // executed every single time the currency is changed as that is an event
    {
        if (price <= Game_Manager.Instance.Currency)    // if the player has enough money
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;
        }
        else // if the player does NOT have enough money
        {
            GetComponent<Image>().color = Color.gray;
            priceTxt.color = Color.gray;
        }
    }

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;

        switch(type)
        {
            case "Fire":
                FireTower fire = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Fire Tower</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff Duration: {2}sec \nTick time: {3} sec \nTick damage: {4}\nCan apply fire damage to the target", fire.Damage, fire.Proc, fire.DebuffDuration, fire.TickTime, fire.TickDamage);
                break;
            case "Ice":
                IceTower frost = towerPrefab.GetComponentInChildren<IceTower>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Frost Tower</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff Duration: {2}sec\nSlowing factor: {3}%\nHas a chance to slow down the target", frost.Damage, frost.Proc, frost.DebuffDuration, frost.SlowingFactor);
                break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Poison Tower</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff Duration: {2}sec \nTick time: {3} sec \nSplash damage: {4}\nCan apply dripping poison to the target", poison.Damage, poison.Proc, poison.DebuffDuration, poison.TickTime, poison.SplashDamage);
                break;
            case "Storm":
                StormTower storm = towerPrefab.GetComponentInChildren<StormTower>();
                tooltip = string.Format("<color=#add8e6ff><size=20><b>Storm Tower</b></size></color>\nDamage: {0} \nProc: {1}%\nDebuff Duration: {2}sec\nHas a chance to stun the target", storm.Damage, storm.Proc, storm.DebuffDuration);
                break;
        }

        Game_Manager.Instance.SetToolTipText(tooltip);
        Game_Manager.Instance.ShowStats();
    }
}
