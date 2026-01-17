using System.Collections.Generic;

namespace BehaviourTree {
    public abstract class Composition {
        
        protected readonly List<INode> Children;
        protected int CurrentNode = 0;

        protected Composition(INode[] nodes) => Children = new List<INode>(nodes);
        
        protected void NextNode() => CurrentNode++;

        protected void ResetNode() => CurrentNode = 0;

        protected void Clear() {
            foreach (var child in Children) {
                if(child is Composition composition)
                    composition.Clear();
            }
            
            Children.Clear();
        }
    }
}