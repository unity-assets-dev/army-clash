using System;
using System.Collections.Generic;
using UnityEngine;

public interface IActorInteraction {
    void Hit(int damage);
}

public interface IInteractionListener {
    void Hit(int damage);
}

public class ActorHitBox : MonoBehaviour, IActorInteraction {
    private static readonly int MainColor = Shader.PropertyToID("_Color");
    
    [SerializeField] private MeshRenderer _renderer;
    
    private readonly HashSet<IInteractionListener> _listeners = new ();

    private void OnValidate() => _renderer = GetComponent<MeshRenderer>();

    public IDisposable AddListener(IInteractionListener listener) => SubscriptionAction.Subscribe(_listeners, listener);

    public void Hit(int damage) {
        foreach (var listener in _listeners) listener.Hit(damage);
    }

    private class SubscriptionAction: IDisposable {
        private readonly Action _onDispose;

        private SubscriptionAction(HashSet<IInteractionListener> container, IInteractionListener listener) {
            _onDispose = () => {
                container.Remove(listener);
            };
            container.Add(listener);
        }

        public void Dispose() => _onDispose?.Invoke();
        
        public static IDisposable Subscribe(HashSet<IInteractionListener> container, IInteractionListener listener) => new SubscriptionAction(container, listener);
    }

    public ActorHitBox Apply(ModifierBuilder builder) {
        transform.localScale = Vector3.one *  builder.size;
        transform.localPosition = Vector3.up * (builder.size * .5f);
        _renderer.material.SetColor(MainColor, builder.color);
        return this;
    }
    
}
