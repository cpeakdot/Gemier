using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

namespace Gemier.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set;}

        public Action OnPause;
        public Action OnQuit;
        [SerializeField] private int money;
        [SerializeField] private TextMeshProUGUI moneyText;
        private const string MONEY_PREF_TAG = "money";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            LoadMoney();
        }


#if UNITY_EDITOR

        private void OnEditorUpdate()
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                OnQuit?.Invoke();
                SaveMoney();
            }
        }

#endif

        private void OnApplicationQuit()
        {
            OnQuit?.Invoke();
            SaveMoney();
        }

        private void OnApplicationPause(bool pauseStatus) 
        {
            if(pauseStatus)
            {
                OnPause?.Invoke();
                SaveMoney();
            }    
        }

        public void AddMoney(int amount)
        {
            int tempMoney = money;
            float animationDuration = .5f;

            money += amount;

            DOTween.To(() => tempMoney, x => tempMoney = x, money, animationDuration)
            .OnUpdate(()=>{
                SetMoneyText(tempMoney);
            });
        }

        private void SaveMoney()
        {
            PlayerPrefs.SetInt(MONEY_PREF_TAG, money);
        }

        private void LoadMoney()
        {
            money = (PlayerPrefs.HasKey(MONEY_PREF_TAG) ? PlayerPrefs.GetInt(MONEY_PREF_TAG) : 0);
            SetMoneyText(money);
        }

        private void SetMoneyText(int amount)
        {
            moneyText.text = "<sprite=0>  " + amount;
        }

    }
}

