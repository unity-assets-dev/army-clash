using UnityEngine;

[CreateAssetMenu(menuName = "Create FormModifier", fileName = "FormModifier", order = 0)]
public class FormModifier : StatModifier, IStat {
    [SerializeField] public ActorHitBox _hitBoxPrefab;
    public override IStat Transform(IStat stats) {
        SetParent(stats);
        return this;
    }

    protected override void OnApplyModifier(ActorModel model) {
        model.SetHitBox(_hitBoxPrefab);
    }
}