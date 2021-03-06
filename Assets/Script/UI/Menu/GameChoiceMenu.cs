﻿using System.Collections.Generic;
using Network;
using SocketIO;
using Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    
    [RequireComponent(typeof(Dropdown))]
    public class GameChoiceMenu : MonoBehaviour
    {
        [SerializeField]private GameObject addServerMenu=null;
        [SerializeField]private GameObject errorWindow=null;
        [SerializeField]private GameObject waitWindow=null;
        [SerializeField]private string defaultIp;
        private Dropdown _dropdown;
        private int _choice = -1;
        private Dictionary<string, string> _serverNameDictionary = null;
        public void AddOption(string ip)
        {
            _dropdown.AddOptions(new List<string>(){ip});
        }
        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.onValueChanged.AddListener(_onChanged);
            _serverNameDictionary = new Dictionary<string, string>();
        }

        private void _setUpNetworking()
        {
            var manager = NetworkManager.GetInstance();
            var ioComponent = manager.GetComponent();
            ioComponent.On("open" , TestOpen);
            ioComponent.On("error", TestError);
            ioComponent.Connect();
        }
        
        private void _onChanged(int index)
        {
            NetworkManager manager;
            SocketIOComponent ioComponent;
            
            switch (_dropdown.options[index].text)
            {
                case "Offline":
                    GameChoice.GameMode = GameMode.Offline;
                    break;
                case "Default Server":
                    GameChoice.GameMode = GameMode.Online;
                    manager = NetworkManager.GetInstance();
                    manager.Clean();
                    ioComponent = manager.GetComponent();
                    var serverAdder = $"ws://{defaultIp}/socket.io/?EIO=3&transport=websocket";
                    ioComponent.Url = serverAdder;
                    _setUpNetworking();
                    waitWindow.SetActive(true);
                    break;
                case "Add Server":
                    addServerMenu.SetActive(true);
                    _dropdown.value = 0;
                    break;
                default:
                    GameChoice.GameMode = GameMode.Online;
                    var dest = _dropdown.options[index].text;
                    manager = NetworkManager.GetInstance();
                    manager.Clean();
                    ioComponent = manager.GetComponent();
                    
                    ioComponent.Url = $"ws://{dest}/socket.io/?EIO=3&transport=websocket";
                    
                    if (_serverNameDictionary.ContainsKey(dest))
                    {
                        ioComponent.Url = _serverNameDictionary[dest];
                    }
                    _setUpNetworking();
                    waitWindow.SetActive(true);
                    _choice = index;
                    break;
            }
        }

        private void GetServerName(SocketIOEvent e)
        {
            print("Got server's name");
            
            var text = e.data["name"].str;
            UnityMainThread.Worker.AddJob(() =>
            {
                waitWindow.SetActive(false);
                if (!_serverNameDictionary.ContainsKey(text))
                {
                    _serverNameDictionary.Add(text , NetworkManager.GetInstance().GetComponent().Url);
                }
                if(_choice == -1) return;
                    _dropdown.options[_choice].text = text;
                _dropdown.RefreshShownValue();
            });
            GameChoice.ServerName = text;
        }
        private void TestOpen(SocketIOEvent e)
        {
            print("Connected to Server");
            var ioComponent = NetworkManager.GetInstance().GetComponent();
            ioComponent.On("getServerName" , GetServerName);
            ioComponent.Emit("getServerName");
        }

        private void TestError(SocketIOEvent e)
        {
            print("receive error");
            print($"error data {e.data}");
            UnityMainThread.Worker.AddJob(() =>
            {
                waitWindow.SetActive(false);
                errorWindow.SetActive(true);
                if(_choice != -1)
                    _dropdown.options.Remove(_dropdown.options[_choice]);
                _dropdown.value = 0;
                _dropdown.RefreshShownValue();
                _choice = -1;
            
                NetworkManager.GetInstance().Clean();
            });
        }
        

        private void OnDestroy()
        {
            if (!NetworkManager.HasInstance()) return;
            var ioComponent = NetworkManager.GetInstance().GetComponent();
            ioComponent.Off("getServerName" , GetServerName);
            ioComponent.Off("open" , TestOpen);
            ioComponent.Off("error", TestError);
        }
    }
}