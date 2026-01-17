using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSystem : MonoBehaviour, IEnumerable<IActor> {

    private readonly HashSet<IActor> _actors = new();
    private readonly Queue<IActor> _added = new();
    private readonly Queue<IActor> _removed = new();
    
    private bool _pause;
    
    public void Pause(bool pause) => _pause = pause;

    public void Add(IActor actor) => _added.Enqueue(actor);

    private void Update() {
        if(_pause) return;
        
        RemoveActors();
        AddActors();
        
        foreach (var actor in _actors) actor.Tick(Time.deltaTime);
    }

    private void AddActors() {
        while (_added.Count > 0) {
            var actor = _added.Dequeue();
            actor.Initialize();
            _actors.Add(actor);
        }
    }

    private void RemoveActors() {
        while (_removed.Count > 0) {
            var actor = _removed.Dequeue();
            actor.Dispose();
            _actors.Remove(actor);
        }
    }

    public void Remove(IActor actor) => _removed.Enqueue(actor);


    public IEnumerator<IActor> GetEnumerator() => _actors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}