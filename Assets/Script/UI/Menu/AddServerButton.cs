using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu
{
    [RequireComponent(typeof(Button))]
    public class AddServerButton : MonoBehaviour
    {
        [SerializeField]private InputField inputField;
        [FormerlySerializedAs("gameChoice")] [SerializeField]private GameChoiceMenu gameChoiceMenu;
        [SerializeField]private GameObject warning;
        [SerializeField]private GameObject window;
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private IEnumerator DisplayWarn()
        {
            warning.SetActive(true);
            yield return new WaitForSeconds(3);
            warning.SetActive(false);
        }
        private void OnClick()
        {
            var str = inputField.text;
            if (IPAddress.TryParse(str, out _))
            {
                gameChoiceMenu.AddOption(str);
                inputField.text = "";
                window.SetActive(false);
            }
            else
            {
                StartCoroutine(DisplayWarn());
                throw new ArgumentException("invalid ip address");
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            warning.SetActive(false);
            inputField.text = "";
        }
    }
}