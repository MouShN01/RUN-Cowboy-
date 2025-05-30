using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private PlayerStats _playerStats;
    private StatPresenter _UI;

    [Inject]
    public void Construct(PlayerStats playerStats, StatPresenter UI)
    {
        _playerStats = playerStats;
        _UI = UI;
    } 

    
}
