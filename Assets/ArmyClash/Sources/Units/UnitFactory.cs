using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitFactory : MonoBehaviour {
    [SerializeField] private int _teamSize = 20;
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

    public T GetActor<T>() where T : Actor {
        var prefab = _teamPrefabs.OfType<T>().FirstOrDefault();
        var instance = Instantiate(prefab, transform);
        _actors.Add(instance);
        return instance;
    }

    public void DisposeActor(Actor actor) {
        _actors.Remove(actor);
    }

    public Actor[] UpdateActors() {
        foreach (var actor in _actors) {
            ApplyModifiers(actor);
        }

        return _actors.ToArray();
    }

    private void ApplyModifiers(Actor actor) {
        var stats = _stats as IStat;

        foreach (var mod in _active.Keys) {
            var modifier = _active[mod].Shuffle().FirstOrDefault();
            if (modifier != null)
                stats = modifier.Transform(stats);
        }

        actor.ApplyStats(stats);
    }

}
