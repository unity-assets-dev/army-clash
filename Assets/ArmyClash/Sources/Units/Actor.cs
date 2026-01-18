using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Actor : MonoBehaviour, IActor, IInteractionListener {
    
    private const int ATTACK_DISTANCE = 2;
    
    [SerializeField] private ActorBrain _brainFactory;
    [SerializeField] private ActorModel _model;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private MeshRenderer _marker;
    
    [Space, Header("Stats")]
    [SerializeField] private StatsDisplay _statDisplay;
    
    private Weaponry _weaponry;
    private IStat _stats;
    private ActorBrain _brain;
    
    private int _health;
    private bool _damaged;
    private float _push;

    public event Action<Actor> OnDeath;
    public int Health => _health;
    public float HealthPercent => _health / (float)_stats.Health;

    private void OnValidate() => _agent = GetComponent<NavMeshAgent>();

    public void Initialize() {
        _brain = _brainFactory.CreateInstance(this);
        _model.Subscribe(this);
    }
    
    public void Tick(float dt) {
        _ = _push > 0 && (_push -= dt) > 0;
        _brain.UpdateBrain(dt);
    }

    public void Dispose() {
        _brain.Dispose();
        _model.Dispose();
    }

    public void Hit(int damage) => SetHealth(Mathf.Max(_health - damage, 0));

    public void ApplyStats(IStat stats) {
        stats.ApplyModifiers(_model);
        
        _statDisplay = StatsDisplay.Create(stats);
        _stats = stats;
        _weaponry = new Weaponry(stats);
        
        SetHealth(_stats.Health);
        
        _agent.speed = Mathf.Max(_stats.Speed, 0);
        _agent.stoppingDistance = 1;
        _agent.radius = _model.GetRadius();
        _agent.avoidancePriority = UnityEngine.Random.Range(1, 500);
        
        SetMarkerColor(this is Blue? 
            Color.blue.WithAlpha(.25f): 
            Color.red.WithAlpha(.25f));
    }
    
    private void SetMarkerColor(Color color) {
        var block = new MaterialPropertyBlock();
        block.SetColor("_Color", color);
        _marker.SetPropertyBlock(block);
    }

    private void SetHealth(int health) {
        _health = health;
        
        if (health <= 0) {
            OnDeath?.Invoke(this);
            return;
        }

        _damaged = true;
        _model.Hit();
        // TODO: health bar update;
    }

    public bool MoveTo(Vector3 target) {
        if(_push > 0) return true;
        
        var targetPosition = target;
        var distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= ATTACK_DISTANCE || _agent.isStopped) {
            _agent.ResetPath();
            return true;
        }
        //NavMesh.SamplePosition(targetPosition, out var hit, float.MaxValue, NavMesh.AllAreas);
        _agent.SetDestination(target);
        
        return false;
    }
    
    public bool Attack(Actor target) => _weaponry.TryAttack(target);

    public bool GetHit() {
        var previous = _damaged;
        _damaged = false;
        return previous;
    }

    public void PushBack(HashSet<Actor> targets, float force = 5) {
        var position = transform.position;
        foreach (var target in targets) {
            var direction = target.transform.position - position;
            target.TakePush(direction.normalized * force);
        }
    }

    private void TakePush(Vector3 direction) {
        if(this.Alive() && _agent.isOnNavMesh) {
            _agent.SetDestination(transform.position + direction);
            _push = 1f;
        }
    }
    
    private void OnDrawGizmos() {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one + Vector3.up);
    }

    public bool OnAttackDistance(Actor spottedTarget) {
        var distance = Vector3.Distance(spottedTarget.transform.position, transform.position);
        return distance <= ATTACK_DISTANCE;
    }
}
