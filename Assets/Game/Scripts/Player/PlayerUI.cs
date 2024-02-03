using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, aliveUnitText, killedUnitText;

    public void Refresh(Player player)
    {
        nameText.text = player.SessionName;
        aliveUnitText.text = player.UnitAlive.ToString();
        killedUnitText.text = player.UnitKilled.ToString();
    }
}
