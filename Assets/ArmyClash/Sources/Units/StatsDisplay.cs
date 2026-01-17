using System;
using UnityEngine;

[Serializable]
public struct StatsDisplay {
    
    public int health;
    public int attack;
    public int attackSpeed;
    public int speed;

    public static StatsDisplay Create(IStat stat) {
        return new StatsDisplay() {
            health = Mathf.Max(stat.Health, 0),
            attack = Mathf.Max(stat.Attack, 0),
            speed = Mathf.Max(stat.Speed, 0),
            attackSpeed = Mathf.Max(stat.AttackSpeed, 0)
        };
    }
    
}