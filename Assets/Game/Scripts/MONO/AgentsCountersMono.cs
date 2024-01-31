using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AgentsCountersMono
{
    public static int EnemyAlive { get; private set; }
    public static void AddEnemyAlive()
    {
        EnemyAlive++;
        
        AgentsInfoMonoUI.Instance.Refresh();
    }
    
    public static int EnemyKilled { get; private set; }
    public static void AddEnemyKilled()
    {
        EnemyAlive--;
        EnemyKilled++;
        
        AgentsInfoMonoUI.Instance.Refresh();
    }
    
    public static int AllyAlive { get; private set; }
    public static void AddAllyAlive()
    {
        AllyAlive++;
        
        AgentsInfoMonoUI.Instance.Refresh();
    }
    
    public static int AllyKilled { get; private set; }
    public static void AddAllyKilled()
    {
        AllyAlive--;
        AllyKilled++;
        
        AgentsInfoMonoUI.Instance.Refresh();
    }
    
    public static void Init()
    {
        EnemyAlive = 0;
        EnemyKilled = 0;
        AllyAlive = 0;
        AllyKilled = 0;
        
        AgentsInfoMonoUI.Instance.Refresh();
    }
}
