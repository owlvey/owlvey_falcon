using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class BaseComponentMessageRp
    {
        public Guid ComponentMessageId { get; private set; }
        public ComponentMessageType ComponentMessageType { get; set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

        public BaseComponentMessageRp(string key, string value, ComponentMessageType type)
        {
            ComponentMessageId = Guid.NewGuid();
            ComponentMessageType = type;
            Version = 1;
            Key = key;
            Value = value;
        }
    }

    public enum ComponentMessageType {
        NotFound,
        Conflict
    }
}
