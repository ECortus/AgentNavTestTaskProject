using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI nameText, aliveUnitText, killedUnitText;

    private void Awake()
    {
        player.OnUnitUpdate += Refresh;
    }

    void Refresh()
    {
        nameText.text = player.SessionName;
        aliveUnitText.text = player.UnitAlive.ToString();
        killedUnitText.text = player.UnitKilled.ToString();
    }
}
