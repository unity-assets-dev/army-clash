using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct StatsDisplay {
    
    public int health;
    public int attack;
    public int attackSpeed;
    public int speed;

    public static StatsDisplay Create(IStat stat) {
        return new StatsDisplay() {
            health = Mathf.Max(stat.Health, 0),
            attack = Mathf.Max(stat.Attack, 0),
            speed = 5, //Mathf.Max(stat.Speed, 1),
            attackSpeed = Mathf.Max(stat.AttackSpeed, 0)
        };
    }
    
}

[RequireComponent(typeof(NavMeshAgent))]
public class Actor : MonoBehaviour, IActor, IInteractionListener {
    
    [SerializeField] private ActorBrain _brainFactory;
    [SerializeField] private ActorModel _model;
    [SerializeField] private NavMeshAgent _agent;
    
    [Space, Header("Stats")]
    [SerializeField] private StatsDisplay _stats;
    
    private ActorBrain _brain;

    private void OnValidate() {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize() {
        _brain = _brainFactory.CreateInstance(this);
        _model.Subscribe(this);
    }
    
    public void Tick(float dt) {
        _brain.UpdateBrain(dt);
    }
    
    public void Dispose() {
        _brain.Dispose();
        _model.Dispose();
    }

    public void Hit(int damage) {
        
    }

    public void ApplyStats(IStat stats) {
        stats.ApplyModifiers(_model);
        
        _stats = StatsDisplay.Create(stats);
        
        _agent.speed = _stats.speed;
        _agent.stoppingDistance = 1;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one + Vector3.up);
    }

    public bool MoveTo(Actor target, float stoppingDistance, float dt) {
        
        var distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < stoppingDistance) {
            _agent.ResetPath();
            return true;
        }
        
        _agent.SetDestination(target.transform.position);
        
        return false;
    }
}
