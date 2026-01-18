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

