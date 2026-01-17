using System;
using System.Collections;
using UnityEngine;

public class SceneMediator : MonoBehaviour {
    
    [SerializeField] private int _teamSize = 20;
    
    [SerializeField] private UnitFactory _unitFactory;
    [SerializeField] private ActorSystem _actorSystem;
    [SerializeField] private GridMap _grid;
    
    private Action<int, int> _onCountRemainUnits;
    
    private int _red;
    private int _blue;
    
    public void PlaceUnits() {
        //var units = new HashSet<Actor>();
        _red = _blue = 0;
        AddTeam<Red>(0, _teamSize);
        AddTeam<Blue>(_grid.CellsCount - _teamSize, _grid.CellsCount);
        
        _unitFactory.DefaultStats();
        
        return;
        
        void AddTeam<T>(int from, int to) where T : Actor {
            for (var i = from; i < to; i++) {
                var unit = _unitFactory.GetActor<T>(_grid.GetPosition(i));
                _ = unit is Red? _red++ : _blue++;
            }
        }
    }

    public void RandomizeTeams() {
        _unitFactory.RandomizeActors();
    }

    public void StartSimulation() {
        foreach (var actor in _unitFactory) {
            actor.OnDeath += OnActorDied;
            _actorSystem.Add(actor);
        }
        
        _actorSystem.Pause(false);
    }

    private void OnActorDied(Actor actor) {
        _ = actor is Red? _red--: _blue--;
        _onCountRemainUnits?.Invoke(_red, _blue);
        
        actor.OnDeath -= OnActorDied;
        _actorSystem.Remove(actor);
        _unitFactory.RemoveUnit(actor);
    }

    public void OnCountRemainUnits(Action<int, int> onCountRemainUnits) {
        _onCountRemainUnits = onCountRemainUnits;
        _onCountRemainUnits?.Invoke(_red, _blue);
    }

    public void DisposeScene() {
        foreach (var actor in _unitFactory) {
            _actorSystem.Remove(actor);
        }
        
        _actorSystem.Pause(true);
        
        Running().PlayCoroutine();

        return;
        IEnumerator Running() {
            yield return new WaitForEndOfFrame();
            
            _actorSystem.Clear();
            _unitFactory.Clear();
        }
    }
}