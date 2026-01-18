using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ActorBrain", fileName = "ActorBrain", order = 0)]
public class ActorBrain : ScriptableObject {
    
    [SerializeField] private BrainBlackboard _blackboard;
    [SerializeField] private float _timeBeforeStart = 2;
    
    private BehaviourAI _root;
    private float _startupTimer;
    private Actor _spottedTarget;
    private Actor _actor;
    private Vector3 _retreatPosition;

    public ActorBrain CreateInstance(Actor actor) {
        var instance = Instantiate(this);
        instance.Setup(actor);
        return instance;
    }

    private readonly HashSet<Actor> _surroundingEnemies = new (100);
    private void Setup(Actor actor) {
        _actor = actor;
        
        _root = INode.NewTree(new [] {
            // Timer before the units can run and check health condition;
            INode.And(new[] { 
                INode.Continuation(StartupTimer),
                INode.Condition(actor.Alive),
                INode.Action(() => {
                    if (actor.GetHit()) _spottedTarget = null;
                })
            }),
            
            INode.Or(new [] {
                // if actor is surrounded by enemies then will try escape from surround;
                INode.And(new [] {
                    INode.Condition(() => {
                        if (!_blackboard.TryUserPushBack(actor)) return false;
                        
                        _blackboard.GetSurroundedCount(actor, 3, _surroundingEnemies);

                        return _surroundingEnemies.Count > 3;
                    }),
                    INode.Action(() => actor.PushBack(_surroundingEnemies)), 
                }),
                // if no a target or the target is lost, then find other nearest a target;
                INode.And(new [] { 
                    INode.Condition(() => _spottedTarget.Dead() || Random.Range(0, 50) < 10),
                    INode.Condition(() => {
                        _spottedTarget = _blackboard.GetNearestAliveTarget(actor);

                        return _spottedTarget.Alive();
                    }), 
                }),
                
                // if a target exists and alive then move towards the target and try attack;
                INode.And(new[] { 
                    INode.Condition(() => _spottedTarget.Alive()),
                    INode.Continuation((dt) => actor.MoveTo(_spottedTarget.transform.position)), 
                    INode.Action(() => {
                        if (_spottedTarget.Alive() && actor.OnAttackDistance(_spottedTarget)) {
                            actor.Attack(_spottedTarget);
                            return;
                        }
                        
                        _spottedTarget = null;
                    })
                }),
            }),
            
        });
        
        _blackboard.AddBrain(actor);
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