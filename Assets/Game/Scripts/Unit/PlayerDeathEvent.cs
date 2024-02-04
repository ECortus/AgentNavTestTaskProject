using UnityEngine;

public class PlayerDeathEvent : MonoBehaviour
{
    [SerializeField] private Player player;

    void Start()
    {
        player.OnDeathEvent += GameManager.Instance.OnGameStop;
    }
}