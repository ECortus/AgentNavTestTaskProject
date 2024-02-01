using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    
    private string PlayerName => PlayerController.Instance.SessionName;
    
    [Inject] private void Awake()
    {
        GameManager.OnGameStart += Refresh;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= Refresh;
    }

    private void Refresh()
    {
        nameText.text = $"Name: {PlayerName}";
    }
}
