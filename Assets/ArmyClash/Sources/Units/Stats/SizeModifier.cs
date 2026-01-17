using UnityEngine;

[CreateAssetMenu(menuName = "Create SizeModifier", fileName = "SizeModifier", order = 0)]
public class SizeModifier: StatModifier, IStat {
    
    [SerializeField] private float _size;
    
    public override IStat Transform(IStat stats) {
        SetParent(stats);
        return this;
    }

    protected override void OnApplyModifier(ActorModel model) => model.SetSize(_size);
}