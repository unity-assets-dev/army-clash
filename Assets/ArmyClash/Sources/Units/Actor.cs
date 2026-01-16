using UnityEngine;

public class Actor : MonoBehaviour, IActor {
    [SerializeField] private ActorBrain _brainFactory;
    
    private ActorBrain _brain;

    public void Initialize() {
        _brain = _brainFactory.CreateInstance(this);
    }
    
    public void Tick(float dt) {
        _brain.UpdateBrain(dt);
    }
    
    public void Dispose() {
        _brain.Dispose();
    }
}
