using UnityEngine;

namespace UI.Menu
{
    public class UrlOpen : MonoBehaviour
    {
        public void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }
    }
}