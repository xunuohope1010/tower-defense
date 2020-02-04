using System;
using System.Collections;
using System.Collections.Generic;
using Bean;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum Element { STORM, FIRE, ICE, POISON, NONE }  // types for the monsters, projectiles, and towers

public delegate void CurrencyChanged(); // delegate is a way of triggering an event and needs to have some sort of structure to suit its functionality

public class Game_Manager : Singleton<Game_Manager>
{
    public TowerBtn ClickedBtn { get; set; }

    private int currency, wave = 0, lives;
    public event CurrencyChanged Changed;

    public int Currency
    {
        get { return currency; }

        set
        {
            currency = value;
            currencyTxt.text = value.ToString() + "<color=lime>$</color>";

            OnCurrencyChanged();
        }
    }

    public int Lives
    {
        get { return lives; }

        set
        {
            lives = value;

            if (lives <= 0)
            {
                lives = 0;
                GameOver();
            }

            livesTxt.text = lives.ToString();
        }
    }

    [SerializeField] private Text currencyTxt, waveTxt, livesTxt, sellTxt, statsTxt, upgradeTxt;
    [SerializeField] private GameObject waveBtn, gameOverMenu, upgradePanel, statsPanel, inGameMenu, optionsMenu;
    [SerializeField] private float spawnWait = 2.5f;
    [SerializeField] private int numMonsters = 4;

    private Tower selectedTower;

    private List<Monster> activeMonsters = new List<Monster>();

    public ObjectPool Pool { get; set; }

    public bool WaveActive { get { return activeMonsters.Count > 0; } }
    private bool gameOver = false;

    private float monsterHealth = 15;

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Use this for initialization
    void Start ()
    {
        /*
         * init lives
         */
        Lives = 10;
        /*
         * init money
         */
        Currency = 50;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleEscape();
	}

    public void PickTower(TowerBtn towerBtn)    // triggered with Unity's button OnClick() event
    {
        if (Currency >= towerBtn.Price && !WaveActive) // only execute if there is enough currency
        {
            ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
            /*
             * send message: pick tower
             */
            OutputQueue.client.Send("pick tower");
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
            /*
             * buy tower, send data
             */
            OutputQueue.client.Send("buy tower, money left: "+Currency);
        }
    }

    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {
            Changed();
        }
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;

            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            Destroy(selectedTower.transform.parent.gameObject);

            DeselectTower();
        }
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        sellTxt.text = "+" + (selectedTower.Price / 2).ToString() + "$";

        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        upgradePanel.SetActive(false);

        selectedTower = null;
    }

    private void DropTower()
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedTower == null && !Hover.Instance.IsVisible)
            {
                ShowInGameMenu();
            }
            else if (Hover.Instance.IsVisible)
            {
                DropTower();
                ShowInGameMenu();
            }
            else if (selectedTower != null)
            {
                DeselectTower();
                ShowInGameMenu();
            }
        }
    }

    public void StartWave()
    {
        wave++;
        waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);    // scope of zero

        StartCoroutine(SpawnWave());

        waveBtn.SetActive(false);
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();

        // current wave functionality: number of monsters to spawn is equal to the wave number per wave
        for (int i = 0; i < wave; i++)
        {
            int monsterIndex = Random.Range(0, numMonsters);

            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "BlueMonster";
                    break;
                case 1:
                    type = "RedMonster";
                    break;
                case 2:
                    type = "GreenMonster";
                    break;
                case 3:
                    type = "PurpleMonster";
                    break;
            }
            /*
             * create monster
             */
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();

            monster.Spawn(monsterHealth);
            
            /*
            * send path of monster
            */
            OutputQueue.sendData();

            if (wave % 3 == 0)
            {
                monsterHealth += 5;
            }

            activeMonsters.Add(monster);

            yield return new WaitForSeconds(spawnWait);
        }
    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        } 
    }

    public void ShowInGameMenu()
    {
        if (optionsMenu.activeSelf) // if the options menu is active
        {
            ShowMain();
        }
        else  // if the options menu is not active
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);

            if (!inGameMenu.activeSelf) // Time.timeScale modifies the rate at which the Unity engine physics engine runs
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    public void Restart()
    {
        Time.timeScale = 1; // to ensure that everything runs at the correct time

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTip();
    }

    public void SetToolTipText(string txt)
    {
        statsTxt.text = txt;
    }

    public void UpdateUpgradeTip()
    {
        if (selectedTower != null)
        {
            sellTxt.text = "+" + (selectedTower.Price/2).ToString() + "$";
            SetToolTipText(selectedTower.GetStats());

            if (selectedTower.NextUpgrade != null)
            {
                upgradeTxt.text = selectedTower.NextUpgrade.Price.ToString() + "$";
            } else
            {
                upgradeTxt.text = "Max Out";
            }
        }
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    public void ShowOptions()
    {
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowMain()
    {
        inGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}