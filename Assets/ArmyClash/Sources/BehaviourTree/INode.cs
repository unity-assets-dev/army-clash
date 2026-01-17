using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviourTree {
    public interface INode {
        Status Evaluate(float dt);
        // factory methods
        static BehaviourAI NewTree(IEnumerable<INode> nodes) => new (nodes.ToArray());
        static BehaviourAI NewTreeByOr(IEnumerable<INode> nodes) => NewTree(new [] { Or(nodes) });
        static BehaviourAI NewTreeByAnd(IEnumerable<INode> nodes) => NewTree(new [] { And(nodes) });
        
        static INode Or(IEnumerable<INode> nodes) => new Selector(nodes.ToArray());
        static INode And(IEnumerable<INode> nodes) => new Sequence(nodes.ToArray());
        
        static INode Action(Action action) => new ActionNode(action);
        static INode Condition(Func<bool> predicate) => new ConditionNode(predicate);
        static INode Continuation(Func<float, bool> predicate) => new ContinuationNode(predicate);

    }
}