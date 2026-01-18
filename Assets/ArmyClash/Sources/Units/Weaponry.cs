using UnityEngine;

public class Weaponry {
    
    private readonly IStat _stat;
    private float _reload;

    private bool CanAttack => _reload == 0 || _reload <= Time.time;
    
    public Weaponry(IStat stat) => _stat = stat;

    private void UpdateReloadCooldown() {
        _reload = Time.time + _stat.AttackSpeed * .1f;
    }

    public bool TryAttack(Actor target) {
        if(!CanAttack) return false;
        
        target.Hit(Mathf.Max(1, _stat.Attack));
        UpdateReloadCooldown();

        return true;
    }

}