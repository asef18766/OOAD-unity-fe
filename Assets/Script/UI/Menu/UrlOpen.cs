using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class UrlOpen : MonoBehaviour
    {
        public void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }

        public void LoadScence(string name)
        {
            SceneManager.LoadScene(name);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}