using UnityEngine;

[CreateAssetMenu(menuName = "Create Stats", fileName = "Stats", order = 0)]
public class Stats: ScriptableObject, IStat {
    
    [SerializeField] private int _health;
    [SerializeField] private int _attack;
    [SerializeField] private int _attackSpeed;
    [SerializeField] private int _speed;

    public int Health => _health;
    public int Attack => _attack;
    public int AttackSpeed => _attackSpeed;
    public int Speed => _speed;
    
    public IStat Transform(IStat stats) => this;
    public void ApplyModifiers(ActorModel model) => model.Build();

    public override string ToString() => $"[HP: {Health}, ATK: {Attack}, ATKSPD: {AttackSpeed}, SPD: {Speed}]";
}

public class Hero {

    public Hero(IStat stat, IStat[] modifiers) {
        var stats = stat;
        Debug.Log($"Base: {stats}");
        foreach (var modifier in modifiers) {
            stats = modifier.Transform(stats);
            Debug.Log($"Form: {stats}");
        }
        
    }
    
}
