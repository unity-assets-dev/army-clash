using BehaviourTree;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ActorBrain", fileName = "ActorBrain", order = 0)]
public class ActorBrain : ScriptableObject {
    
    [SerializeField] private BrainBlackboard _blackboard;
    [SerializeField] private float _timeBeforeStart = 2;
    
    private BehaviourAI _root;
    private float _startupTimer;
    private Actor _actor;

    public ActorBrain CreateInstance(Actor actor) {
        var instance = Instantiate(this);
        instance.Setup(actor);
        return instance;
    }

    private void Setup(Actor actor) {
        _actor = actor;
        
        _root = INode.NewTree(new [] {
            // check states: Init, Validate health, Action in combat
            StartupState(actor),
            ValidateHealth(actor),
            SearchTarget(actor),
            AttackTarget(actor),
        });
        
        _blackboard.AddBrain(actor);
    }

    private INode AttackTarget(Actor actor) {
        return INode.And(new[] {
            INode.Condition(() => _target.Alive()),
            INode.Continuation((dt) => {
                if (_target.Dead()) {
                    _target = null;
                    return true;
                }
                return  actor.MoveTo(_target, 2, dt);
            }), 
        });
    }

    private INode StartupState(Actor actor) {
        return INode.Continuation(StartupTimer);
    }

    private INode ValidateHealth(Actor actor) {
        return INode.Or(new[] {
            INode.Condition(actor.Alive),
        });
    }

    private Actor _target;

    private INode SearchTarget(Actor actor) {
        return INode.And(new INode[] {
            
            // No target, then
            INode.Condition(() => _target.Dead()),
            // Find nearest target
            INode.Condition(() => {
                _target = _blackboard.GetNearestAliveTarget(actor);

                return _target.Alive();
            }), 
        });
    }

    private bool StartupTimer(float dt) {
        if (_startupTimer >= _timeBeforeStart) return true;
        
        var result = (_startupTimer += dt) >= _timeBeforeStart;
        
        return result;
    }

    public void UpdateBrain(float dt) => _root.Evaluate(dt);

    public void Dispose() {
        _blackboard.RemoveBrain(_actor);
        _root.Dispose();
    }
}