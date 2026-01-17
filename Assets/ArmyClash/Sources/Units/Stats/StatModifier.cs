using UnityEngine;

public abstract class StatModifier : ScriptableObject, IStat {
    
    [SerializeField] private int _healthBonus;
    [SerializeField] private int _attackBonus;
    [SerializeField] private int _attackSpeedBonus;
    [SerializeField] private int _speedBonus;
    
    private IStat _stat;

    public int Health => _stat.Health + _healthBonus;
    public int Attack => _stat.Attack + _attackBonus;
    public int AttackSpeed => _stat.AttackSpeed + _attackSpeedBonus;
    public int Speed => _stat.Speed + _speedBonus;

    protected void SetParent(IStat stat) => _stat = stat;
    
    public abstract IStat Transform(IStat stats);

    public void ApplyModifiers(ActorModel model) {
        OnApplyModifier(model);
        _stat.ApplyModifiers(model);
    }

    protected abstract void OnApplyModifier(ActorModel model);

    public override string ToString() => $"[HP: {Health}, ATK: {Attack}, ATKSPD: {AttackSpeed}, SPD: {Speed}]";
}