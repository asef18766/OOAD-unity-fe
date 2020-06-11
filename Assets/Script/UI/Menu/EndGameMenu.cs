using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private Text winner;
        [SerializeField] private Image winnerImage;
        [SerializeField] private Sprite p1, p2;
        private void Start()
        {
            winner.text = GameChoice.Winner;
            winnerImage.sprite = (GameChoice.Winner == "p1") ? p1 : p2;
        }
    }
}