using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(menuName = "Create BrainBlackboard", fileName = "BrainBlackboard", order = 0), BurstCompile]
public class BrainBlackboard : ScriptableObject {
    [SerializeField] private bool _usePushBack;

    private readonly Dictionary<Type, List<Actor>> _actors = new(2);

    public void AddBrain(Actor actor) {
        _actors.TryAdd(actor.GetType(), new List<Actor>(200));
        _actors[actor.GetType()].Add(actor);
    }

    public void RemoveBrain(Actor actor) {
        if (_actors.ContainsKey(actor.GetType())) {
            _actors[actor.GetType()].Remove(actor);
        }
    }

    public bool TryUsePushBack(Actor actor) {
        if (!_usePushBack) return false;

        var chances = actor.HealthPercent > .5f ? 5 : 15;
        return UnityEngine.Random.Range(0, chances) >= 5;
    }

    public Actor GetNearestAliveTarget(Actor actor) =>
        GetNearestTarget(actor, out var nearestTarget) ? nearestTarget : null;

    private List<Actor> GetEnemyTeam(Actor actor) {
        foreach (var key in _actors.Keys) {
            if (key != actor.GetType()) return _actors[key];
        }

        return null;
    }

    public bool GetNearestTarget(Actor actor, out Actor target) {
        var key = _actors.Keys.FirstOrDefault(k => k != actor.GetType());
        target = null;

        if (key == null) return false;
        var nearest = float.MaxValue;

        foreach (var neighbor in _actors[key]) {
            if (neighbor.Dead()) continue;

            var distance = Vector3.Distance(actor.transform.position, neighbor.transform.position);
            if (distance < nearest || target != null && target.Health > neighbor.Health) {
                target = neighbor;
            }
        }

        return target != null;
    }

    public void GetSurroundedCount(Actor actor, int range, in HashSet<Actor> surroundingEnemy) {
        surroundingEnemy.Clear();

        foreach (var enemy in GetEnemyTeam(actor)) {
            if (Vector3.Distance(actor.transform.position, enemy.transform.position) <= range) {
                surroundingEnemy.Add(enemy);
            }
        }
    }
}

public static class JobsExtensions {
    public static T CompleteSchedule<T>(this T job, int count = 4) where T : struct, IJobParallelFor {
        job.Schedule(count, count / 4).Complete();
        return job;
    }
}

[BurstCompile]
public struct GetSurroundingCountJob : IJobParallelFor {
    
    [ReadOnly] public Vector3 Self;
    [ReadOnly] public float range;
    [ReadOnly] public NativeArray<Vector3> Targets;
    
    [WriteOnly] public NativeArray<int> Result;
    
    private int _count;
    
    public void Execute(int index) {
        var position = Targets[index];
        var distance = math.distance(Self, position);

        if (distance <= range) {
            Result[_count++] = index;
        }
    }

    public static GetSurroundingCountJob Create(Actor actor, float range, List<Actor> targets) {
        var positions = new NativeArray<Vector3>(targets.Count, Allocator.TempJob);

        for (var i = 0; i < targets.Count; i++) {
            positions[i] = targets[i].transform.position;
        }

        return new GetSurroundingCountJob {
            Self = actor.transform.position,
            range = range,
            Targets = positions,
        };
    }
}

[BurstCompile]
public struct GetNearestJob : IJobParallelFor {
    
    [ReadOnly] private Vector3 _self;
    [ReadOnly] private float _selfHealth;
    [ReadOnly] private NativeArray<Vector3> _position;
    [ReadOnly] private NativeArray<int> _healths;

    [WriteOnly] public int Target;

    private float _minDistance;
    
    public void Execute(int index) {
        _minDistance = _minDistance == 0? float.MaxValue : _minDistance;
        
        var position = _position[index];
        //var health = _healths[index];
        var distance = math.distance(position,  _self);

        if (distance < _minDistance) {
            _minDistance = distance;
            Target = index;
        }
        
    }

    public static GetNearestJob Create(Actor actor, List<Actor> targets) {
        var positions = new NativeArray<Vector3>(targets.Count, Allocator.TempJob);
        var healths = new NativeArray<int>(targets.Count, Allocator.TempJob);

        for (var i = 0; i < targets.Count; i++) {
            positions[i] = targets[i].transform.position;
            healths[i] = targets[i].Health;
        }
        
        return new GetNearestJob() {
            _self = actor.transform.position,
            _selfHealth = actor.HealthPercent,
            _position = positions,
            _healths = healths,
        };
    }
}