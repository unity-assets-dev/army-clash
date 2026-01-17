using UnityEngine;

[CreateAssetMenu(menuName = "Create ColorModifier", fileName = "ColorModifier", order = 0)]
public class ColorModifier : StatModifier, IStat {
    [SerializeField] public Color _color;
    public override IStat Transform(IStat stats) {
        SetParent(stats);
        return this;
    }

    protected override void OnApplyModifier(ActorModel model) => model.SetColor(_color);
}