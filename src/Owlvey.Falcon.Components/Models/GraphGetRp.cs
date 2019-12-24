using System;
using System.Collections.Generic;
using Owlvey.Falcon.Core;

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
        public decimal Value { get; set; }
        public decimal Slo { get; set; }
        public decimal Budget { get; set; }
        public string Group { get; set; }
        public decimal Importance { get; set; }
        public Dictionary<string, object> Tags { get; set; } = new Dictionary<string, object>();


        public GraphNode() { }

        public GraphNode(string group, string key, int id, string avatar, string name,
           decimal value, decimal slo)
        {
            this.Id = string.Format("{0}_{1}", key, id);
            this.Avatar = avatar;
            this.Name = name;
            this.Value = value;
            this.Group = group;
            this.Slo = slo;
            this.Importance = AvailabilityUtils.MeasureImpact(slo);
            this.Budget = value - (decimal)slo;                        
        }

    }

    public class GraphEdge {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Value { get; set; }
        public IDictionary<string, object> Tags { get; set; } = new Dictionary<string, object>();

        public GraphEdge() { }
        public GraphEdge(string from, string to, decimal value, IDictionary<string, object> tags) {
            this.From = from;
            this.To = to;
            this.Value = value;
            this.Tags = tags;
        }
    }
}
