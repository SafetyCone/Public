using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code
{
    public class Node<T> : INode<T>
    {
        public Dictionary<string, INode<T>> Children { get; protected set; }
        public string Name { get; set; }
        public INode<T> Parent { get; set; }
        public T Value { get; set; }


        protected Node()
        {
            this.Children = new Dictionary<string, INode<T>>();
        }

        public Node(string name, INode<T> parent)
            : this()
        {
            this.Name = name;
            this.Parent = parent;
        }
    }
}
