using System;
using System.Collections.Generic;

namespace Owlvey.Falcon.Models
{
    public class GraphGetRp
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }

        public List<GraphNode> Nodes { get; set; } = new List<GraphNode>();
        public List<GraphEdge> Edges { get; set; } = new List<GraphEdge>();         
    }

    public class GraphNode {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Avatar { get; set; }
        public decimal Availability { get; set; }
        public decimal Slo { get; set; }
        public decimal Budget { get; set; }
        public string Group { get; set; }
        public decimal Importance { get; set; }
    }

    public class GraphEdge {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Budget { get; set; }
        public decimal Availability { get; set; }
    }
}
