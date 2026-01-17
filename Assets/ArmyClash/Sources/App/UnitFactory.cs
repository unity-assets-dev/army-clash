using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitFactory : MonoBehaviour, IEnumerable<Actor> {
    
    [SerializeField] private Actor[] _teamPrefabs;

    [SerializeField] private Stats _stats;
    [SerializeField] private StatModifier[] _modifiers;
    
    private Dictionary<Type, IStat[]> _active;
    private readonly HashSet<Actor> _actors = new();

    private void Start() {
        _active = _modifiers
            .GroupBy(m => m.GetType(), m => m as IStat)
            .ToDictionary(x => x.Key, x => x.ToArray());
    }

    public T GetActor<T>(Vector3 position) where T : Actor {
        var prefab = _teamPrefabs.OfType<T>().FirstOrDefault();
        var instance = Instantiate(prefab, position, Quaternion.identity, transform);
        _actors.Add(instance);
        return instance;
    }

    public Actor[] RandomizeActors() {
        foreach (var actor in _actors) {
            ApplyModifiers(actor);
        }

        return _actors.ToArray();
    }

    private void ApplyModifiers(Actor actor, bool firstModifier = false) {
        var stats = _stats as IStat;

        foreach (var mod in _active.Keys) {
            var modifiers = _active[mod];
            var modifier = firstModifier? modifiers[0]: modifiers.Shuffle().FirstOrDefault();
            if (modifier != null)
                stats = modifier.Transform(stats);
        }

        actor.ApplyStats(stats);
    }

    public void Clear() {
        foreach (var actor in _actors) {
            actor.Dispose();
            _actors.Remove(actor);
            Destroy(actor.gameObject);
        }
    }

    public void DefaultStats() {
        foreach (var actor in _actors) {
            ApplyModifiers(actor, firstModifier: true);
        }
    }

    public IEnumerator<Actor> GetEnumerator() => _actors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
