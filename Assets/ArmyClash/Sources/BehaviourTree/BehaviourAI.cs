using System;
using UnityEngine;

namespace BehaviourTree {
    public enum Status { Running, Success, Failure }

    // Logic AND; All nodes must return Success;
    public class Sequence : Composition, INode {
        
        public Sequence(params INode[] nodes) : base(nodes) { }
        
        public Status Evaluate(float dt) {
            
            while (CurrentNode < Children.Count) {
                var nodeStatus = Children[CurrentNode].Evaluate(dt);

                switch (nodeStatus) {
                    case Status.Running: return Status.Running; 
                    case Status.Success:
                        CurrentNode++;
                        return CurrentNode == Children.Count? 
                            Status.Success: 
                            Status.Running;
                    default:
                        ResetNode();
                        return Status.Failure;
                }
            }
            
            ResetNode();
            return Status.Success;
        }
    }

    // Logic OR; One of Node must return Success;
    public class Selector : Composition, INode {
        public Selector(params INode[] nodes) : base(nodes) { }
        
        public Status Evaluate(float dt) {
            while (CurrentNode < Children.Count) {
                switch (Children[CurrentNode].Evaluate(dt)) {
                    case Status.Running: return Status.Running;
                    case Status.Success: 
                        ResetNode();
                        return Status.Success;
                    default:
                        NextNode();
                        return Status.Running;
                }
            }
            
            ResetNode();
            return Status.Failure;
        }
    }
    
    public class ActionNode : INode {
        private readonly Action _action;

        public ActionNode(Action action) => _action = action;

        public Status Evaluate(float dt) {
            try {
                _action?.Invoke();
                return Status.Success;
            }
            catch {
                return Status.Failure;
            }
        }
    }

    public class ConditionNode : INode {
        
        private readonly Func<bool> _predicate;
        private readonly string _name;
        public ConditionNode(Func<bool> predicate) => _predicate = predicate;
        public ConditionNode(string name, Func<bool> predicate): this(predicate) => _name = name;

        public Status Evaluate(float dt) {
            try {
                var result = _predicate() ? Status.Success : Status.Failure;
                if(!string.IsNullOrEmpty(_name)) Debug.Log($"[{_name}: {result}]");
                return result;
            }
            catch {
                return Status.Failure;
            }
        }
    }

    public class ContinuationNode : INode {
        private readonly Func<float, bool> _predicate;
        private readonly string _name;

        public ContinuationNode(Func<float, bool> predicate) => _predicate = predicate;

        public ContinuationNode(string name, Func<float, bool> predicate): this(predicate) {
            _name = name;
        }

        public Status Evaluate(float dt) {
            try {
                if (_predicate == null) return Status.Failure;
                var result = _predicate(dt) ? Status.Success : Status.Running;
                
                if(!string.IsNullOrEmpty(_name)) Debug.Log($"[{_name}: {result}]");
                
                return result;
            }
            catch {
                return Status.Failure;
            }
        }
    }
    
    public class BehaviourAI: Composition, INode {
        public BehaviourAI(INode[] nodes) : base(nodes) { }
        
        public Status Evaluate(float dt) {
            
            while (CurrentNode < Children.Count) {
                var nodeStatus = Children[CurrentNode].Evaluate(dt);

                if(nodeStatus == Status.Running) return Status.Running;
                
                if (nodeStatus == Status.Failure) {
                    ResetNode();
                    return Status.Failure;
                }
                
                NextNode();
            }
            
            ResetNode();
            return Status.Running;
        }

        public void Dispose() {
            ResetNode();
            Clear();
        }
    }
}