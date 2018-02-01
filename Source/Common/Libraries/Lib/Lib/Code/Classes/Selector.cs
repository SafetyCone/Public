using System.Collections.Generic;


namespace Public.Common.Lib
{
    public class DummySelector : ISelector
    {
        public static DummySelector Instance { get; } = new DummySelector();


        public bool this[int index] => true;
    }


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


    public class EveryNthSelector : ISelector
    {
        public int Nth { get; }
        public bool this[int index]
        {
            get
            {
                bool output = 0 == index % this.Nth;
                return output;
            }
        }


        public EveryNthSelector(int nth)
        {
            this.Nth = nth;
        }
    }

    public class EveryNthOffsetSelector : ISelector
    {
        public int Nth { get; }
        public int Offset { get; }
        public bool this[int index]
        {
            get
            {
                bool output = this.Offset == index % this.Nth;
                return output;
            }
        }


        public EveryNthOffsetSelector(int nth, int offset)
        {
            this.Nth = nth;
            this.Offset = offset;
        }
    }
}
