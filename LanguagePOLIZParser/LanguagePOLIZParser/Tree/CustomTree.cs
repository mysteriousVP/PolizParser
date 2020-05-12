using System.Collections.Generic;

namespace PolizParser
{
    public class CustomTree
    {
        private List<CustomTree> children = null;

        public string Name { get; }

        public uint Id { get; }

        public CustomTree(string name, uint id = 0)
        {
            Name = name;
            Id = id;
        }

        public void AddChild(CustomTree node)
        {
            if (children == null)
            {
                children = new List<CustomTree>();
            }

            children.Add(node);
        }

        public List<CustomTree> GetChildren()
        {
            return children;
        }
    }
}