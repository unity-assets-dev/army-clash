using BehaviourTree;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ActorBrain", fileName = "ActorBrain", order = 0)]
public class ActorBrain : ScriptableObject {
    [SerializeField] private float _timeBeforeStart = 2;
    
    private INode _root;
    private float _startupTimer;
    
    public ActorBrain CreateInstance(Actor actor) {
        var instance = Instantiate(this);
        
        return instance;
    }

    private void Setup(Actor actor) {
        _root = INode.NewTreeBySelector(new [] {
            // check states: Init, Search, Running, Dead
            StartupState(actor),
            SearchTarget(actor),
        });
    }

    private INode StartupState(Actor actor) {
        return INode.Continuation(StartupTimer);
    }

    private INode SearchTarget(Actor actor) {
        return INode.Selector(new[] {
            INode.Condition(() => {
                return true;
            }),
        });
    }

    private bool StartupTimer(float dt) {
        var result = (_startupTimer += dt) >= _timeBeforeStart;
        
        if(result) _startupTimer = 0;
        
        return result;
    }

    public void UpdateBrain(float dt) {
        
    }

    public void Dispose() {
        
    }
}