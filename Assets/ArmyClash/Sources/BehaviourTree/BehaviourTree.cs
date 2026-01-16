using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviourTree {
    public enum Status { Running, Success, Failure }
    
    public interface INode {
        Status Evaluate(float dt);
        // factory methods
        static INode NewTree(IEnumerable<INode> nodes) => new BehaviourTree(nodes.ToArray());
        static INode NewTreeBySelector(IEnumerable<INode> nodes) => NewTree(new [] { Selector(nodes) });
        
        static INode Selector(IEnumerable<INode> nodes) => new Selector(nodes.ToArray());
        static INode Sequence(IEnumerable<INode> nodes) => new Sequence(nodes.ToArray());
        
        static INode Action(Action action) => new ActionNode(action);
        static INode Condition(Func<bool> predicate) => new ConditionNode(predicate);
        static INode Continuation(Func<float, bool> predicate) => new ContinuationNode(predicate);

    }

    public abstract class Composition {
        
        protected readonly List<INode> Children;
        protected int CurrentNode = 0;
        
        public Composition(INode[] nodes) => Children = new List<INode>(nodes);
        
        protected void NextNode() => CurrentNode = (CurrentNode + 1) % Children.Count;
        protected void ResetNode() => CurrentNode = 0;
    }

    public class Sequence : Composition, INode {
        
        public Sequence(params INode[] nodes) : base(nodes) { }
        
        public Status Evaluate(float dt) {
            
            while (CurrentNode < Children.Count) {
                var nodeStatus = Children[CurrentNode].Evaluate(dt);

                switch (nodeStatus) {
                    case Status.Running: return Status.Running; 
                    case Status.Success:
                        NextNode();
                        return CurrentNode == Children.Count? Status.Success: Status.Running;
                    default:
                        ResetNode();
                        return Status.Failure;
                }
            }
            
            ResetNode();
            return Status.Success;
        }
    }

    public class Selector : Composition, INode {
        public Selector(params INode[] nodes) : base(nodes) { }
        public Status Evaluate(float dt) {
            if (CurrentNode < Children.Count) {
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
        public ConditionNode(Func<bool> predicate) => _predicate = predicate;
        
        public Status Evaluate(float dt) {
            try {
                return _predicate() ? Status.Success : Status.Failure;
            }
            catch {
                return Status.Failure;
            }
        }
    }

    public class ContinuationNode : INode {
        private readonly Func<float, bool> _predicate;

        public ContinuationNode(Func<float, bool> predicate) => _predicate = predicate;

        public Status Evaluate(float dt) {
            try {
                if (_predicate == null) return Status.Failure;
                
                return _predicate(dt) ? Status.Success : Status.Running;
            }
            catch {
                return Status.Failure;
            }
        }
    }
    
    public class BehaviourTree: Composition, INode {
        
        public BehaviourTree(INode[] nodes) : base(nodes) { }
        
        public Status Evaluate(float dt) {
            while (CurrentNode < Children.Count) {
                var nodeStatus = Children[CurrentNode].Evaluate(dt);

                if (nodeStatus == Status.Failure) {
                    ResetNode();
                    return Status.Failure;
                }

                if (nodeStatus == Status.Success) {
                    NextNode();
                }
            }
            
            ResetNode();
            return Status.Running;
        }
    }
}