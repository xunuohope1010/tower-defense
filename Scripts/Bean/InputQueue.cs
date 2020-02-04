using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bean
{
    public class InputQueue
    {
        public static Button poisonBtn;
        public static void parse(String inputString)
        {
            Debug.Log("get: "+inputString);
            // poisonBtn = GameObject.Find("Canvas/TowerPanel/PoisonBtn").GetComponent<Button>();
            // poisonBtn.onClick.Invoke();

        }
    }
}