using System;
using System.Collections.Generic;


namespace Eshunna.Lib
{
    [Serializable]
    public class StructureModel
    {
        public List<Location3Double> Vertices { get; private set; }
        public List<Facet> Facets { get; private set; }


        public StructureModel()
            : this(0, 0)
        {
        }

        public StructureModel(int numberOfVertices, int numberOfFacets)
        {
            this.Vertices = new List<Location3Double>(numberOfVertices);
            this.Facets = new List<Facet>(numberOfFacets);
        }

        public StructureModel(IEnumerable<Location3Double> vertices, IEnumerable<Facet> facets)
            : this()
        {
            this.Vertices.AddRange(vertices);
            this.Facets.AddRange(facets);
        }
    }
}
