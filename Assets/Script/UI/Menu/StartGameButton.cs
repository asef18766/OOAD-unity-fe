﻿using System;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField] private string onlineScene;
        [SerializeField] private string offlineScene;
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            switch (GameChoice.GameMode)
            {
                case GameMode.Offline:
                    if (NetworkManager.HasInstance())
                        Destroy(NetworkManager.GetInstance().GetComponent().gameObject);
                    if(Application.platform == RuntimePlatform.Android)
                        SceneManager.LoadScene("AndroidGameScene");
                    else
                        SceneManager.LoadScene(offlineScene);
                    
                    break;
                case GameMode.Online:
                    SceneManager.LoadScene(onlineScene);
                    break;
                case GameMode.Server:
                    print("you dirty hacker :P");
                    break;
                default:
                    throw new ArgumentException($"receive unexpected option {GameChoice.GameMode}");
            }
        }
    }
}