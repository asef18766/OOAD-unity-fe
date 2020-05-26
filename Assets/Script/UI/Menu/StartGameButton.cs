using System;
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
            switch (GameChoice.Gamemode)
            {
                case GameMode.Offline:
                    throw new NotImplementedException("not implemented yet...QQ");
                case GameMode.Online:
                    SceneManager.LoadScene(onlineScene);
                    break;
                case GameMode.Server:
                    print("you dirty hacker :P");
                    break;
                default:
                    throw new ArgumentException($"receive unexpected option {GameChoice.Gamemode}");
            }
        }
    }
}