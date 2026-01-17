using System;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBuilder {
    public ActorHitBox hitBox;
    public float size;
    public Color color;
}

public class ActorModel : MonoBehaviour {
    
    private static readonly int HitState = Animator.StringToHash("Hit");
    
    [SerializeField] private Animator _animator;
    
    private ActorHitBox _hitBox;
    
    private readonly HashSet<IDisposable> _subscriptions = new();
    private readonly ModifierBuilder _builder = new ();
    
    public void Subscribe(Actor actor) => _subscriptions.Add(_hitBox.AddListener(actor));

    public void Dispose() {
        
        foreach (var subscription in _subscriptions) subscription.Dispose();
        _subscriptions.Clear();
        
        if (_hitBox == null) return;
        
        Destroy(_hitBox.gameObject);
        _hitBox = null;
    }

    public void SetColor(Color color) => _builder.color = color;

    public void SetSize(float size) => _builder.size = size;

    public void SetHitBox(ActorHitBox hitBoxPrefab) => _builder.hitBox = hitBoxPrefab;

    public void Build() {
        Dispose();
        
        var instance = Instantiate(_builder.hitBox, transform);
        
        _hitBox = instance.Apply(_builder);
    }

    public float GetRadius() => _builder.size * .5f;

    public void Hit() {
        _animator.SetTrigger(HitState);
    }
}