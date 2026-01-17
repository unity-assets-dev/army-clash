using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create BrainBlackboard", fileName = "BrainBlackboard", order = 0)]
public class BrainBlackboard : ScriptableObject {
    
    private readonly Dictionary<Type, List<Actor>> _actors = new();
    
    public void AddBrain(Actor actor) {
        _actors.TryAdd(actor.GetType(), new List<Actor>());
        _actors[actor.GetType()].Add(actor);
    }

    public void RemoveBrain(Actor actor) {
        if (_actors.ContainsKey(actor.GetType())) {
            _actors[actor.GetType()].Remove(actor);
        }
    }

    public Actor GetNearestAliveTarget(Actor actor) {
        return GetNearestTarget(actor, out var nearestTarget) ? nearestTarget : null;
    }

    public bool GetNearestTarget(Actor actor, out Actor target) {
        var key = _actors.Keys.FirstOrDefault(k => k != actor.GetType());
        target = null;
        
        if(key == null) return false;
        var nearest = float.MaxValue;
        
        foreach (var neighbor in _actors[key]) {
            if(neighbor.Dead()) continue;
            
            var distance = Vector3.Distance(actor.transform.position, neighbor.transform.position);
            if (distance < nearest) {
                target = neighbor;
                nearest = distance;
            }
        }

        return target != null;
    }
}