using UnityEngine;

public class AppEntry : MonoBehaviour {

    [SerializeField] private UnitFactory _factory;
    [SerializeField] private ActorSystem _actors;
    
    private Actor[] _units;

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.R)) {
            
            _units = _factory.UpdateActors();
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            foreach (var actor in _actors) _actors.Remove(actor);
            
            foreach (var actor in _units) _actors.Add(actor);
        }
    }
}
