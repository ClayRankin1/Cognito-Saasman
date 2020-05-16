using System.Collections.Generic;

namespace Cognito.Business.DataStructures
{
    public class TreeNode<T>
    {
        public T Value { get; }
        public TreeNode<T> Parent { get; set; }
        public IList<TreeNode<T>> Children { get; set; }

        public TreeNode(T value, TreeNode<T> parent)
        {
            this.Value = value;
            this.Parent = parent;
            this.Children = new List<TreeNode<T>>();
        }

        public bool AddChild(TreeNode<T> child)
        {
            if (Children.Contains(child))
            {
                return false;
            }
            else if (child == this)
            {
                return false;
            }
            else
            {
                Children.Add(child);
                child.Parent = this;
                return true;
            }
        }

        public bool RemoveChild(TreeNode<T> child)
        {
            if (Children.Contains(child))
            {
                child.Parent = null;
                return Children.Remove(child);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveAllChildren()
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Parent = null;
                Children.RemoveAt(i);
            }
            return true;
        }
    }
}