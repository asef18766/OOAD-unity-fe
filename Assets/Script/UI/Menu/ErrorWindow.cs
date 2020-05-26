using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class ErrorWindow : MonoBehaviour
    {
        [SerializeField] private Text errorMessage;

        public void SetMessage(string str)
        {
            errorMessage.text = str;
        }
    }
}