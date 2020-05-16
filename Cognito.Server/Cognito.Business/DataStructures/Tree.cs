using System.Collections.Generic;

namespace Cognito.Business.DataStructures
{
    public class Tree<T>
    {
        TreeNode<T> root = null;
        IList<TreeNode<T>> nodes = new List<TreeNode<T>>();

        public Tree(T value)
        {
            root = new TreeNode<T>(value, null);
            nodes.Add(root);
        }

        public int Count
        {
            get { return nodes.Count; }
        }

        public TreeNode<T> Root
        {
            get { return root; }
        }

        public bool AddNode(TreeNode<T> node)
        {
            if (node == null || node.Parent == null || !nodes.Contains(node.Parent))
            {
                return false;
            }
            else if (node.Parent.Children.Contains(node))
            {
                return false;
            }
            else
            {
                nodes.Add(node);
                return node.Parent.AddChild(node);
            }
        }

        public void Clear()
        {
            foreach (TreeNode<T> node in nodes)
            {
                node.Parent = null;
                node.RemoveAllChildren();
            }

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                nodes.RemoveAt(i);
            }
            root = null;
        }

        public bool RemoveNode(TreeNode<T> node)
        {
            if (node == null)
            {
                return false;
            }
            else if (node == root)
            {
                Clear();
                return true;
            }
            else
            {
                bool success = node.Parent.RemoveChild(node);
                if (!success)
                {
                    return false;
                }

                success = nodes.Remove(node);
                if (!success)
                {
                    return false;
                }

                return true;
            }
        }

        public TreeNode<T> Find(T value)
        {
            foreach (TreeNode<T> node in nodes)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }
    }
}