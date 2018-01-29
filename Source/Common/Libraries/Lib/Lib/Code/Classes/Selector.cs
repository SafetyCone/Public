using System.Collections.Generic;


namespace Public.Common.Lib
{
    public class Selector : ISelector
    {
        private HashSet<int> Indices { get; set; }
        public bool this[int index]
        {
            get
            {
                bool output = this.Indices.Contains(index);
                return output;
            }
        }

        public Selector(IEnumerable<int> indices)
        {
            this.Indices = new HashSet<int>(indices);
        }
    }
}
