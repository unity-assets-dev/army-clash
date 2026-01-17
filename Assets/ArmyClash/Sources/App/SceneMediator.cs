using System;
using UnityEngine;

public class SceneMediator : MonoBehaviour {
    
    [SerializeField] private int _teamSize = 20;
    
    [SerializeField] private UnitFactory _unitFactory;
    [SerializeField] private ActorSystem _actorSystem;
    [SerializeField] private GridMap _grid;
    
    private Action<int> _onCountRemainUnits;

    public void PlaceUnits() {
        //var units = new HashSet<Actor>();
        
        AddTeam<Red>(0, _teamSize);
        AddTeam<Blue>(_grid.CellsCount - _teamSize, _grid.CellsCount);
        
        _unitFactory.DefaultStats();
        
        return;
        
        void AddTeam<T>(int from, int to) where T : Actor {
            for (var i = from; i < to; i++) {
                _unitFactory.GetActor<T>(_grid.GetPosition(i));
                //units.Add(unit);
            }
        }
    }

    public void RandomizeTeams() {
        _unitFactory.RandomizeActors();
    }

    public void StartSimulation() {
        foreach (var actor in _unitFactory) {
            _actorSystem.Add(actor);
        }

        _actorSystem.Pause(false);
    }

    public void OnCountRemainUnits(Action<int> onCountRemainUnits) {
        _onCountRemainUnits = onCountRemainUnits;
    }

    public void DisposeScene() {
        _actorSystem.Pause(true);
        _actorSystem.Clear();
        _unitFactory.Clear();
    }
}