using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Owlvey.Falcon.Components.Models
{
    public class BaseComponentResultRp
    {
        readonly List<BaseComponentMessageRp> _messages;
        readonly Dictionary<string, string> _results;

        public BaseComponentResultRp()
        {
            _messages = new List<BaseComponentMessageRp>();
            _results = new Dictionary<string, string>();
        }

        public void AddConflict(string message)
        {
            BaseComponentMessageRp domainMessage = new BaseComponentMessageRp("", message, ComponentMessageType.Conflict);
            _messages.Add(domainMessage);
        }

        public void AddNotFound(string message)
        {
            BaseComponentMessageRp domainMessage = new BaseComponentMessageRp("", message, ComponentMessageType.NotFound);
            if (!_messages.Any(c => c.ComponentMessageType == ComponentMessageType.NotFound))
                _messages.Add(domainMessage);
        }

        public void AddResult(string key, object value)
        {
            _results.Add(key, JsonConvert.SerializeObject(value));
        }

        public T GetResult<T>(string key)
        {
            if (!_results.Keys.Any(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
                throw new KeyNotFoundException($"The key ${key} was not found");

            return JsonConvert.DeserializeObject<T>(_results[key]);
        }

        public string GetConflicts()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var conflicts = _messages.Where(x => x.ComponentMessageType == ComponentMessageType.Conflict);
            foreach (var item in conflicts)
            {
                stringBuilder.AppendLine(item.Value);
            }
            return stringBuilder.ToString();
        }

        public bool HasConflicts()
        {
            return _messages.Any(x => x.ComponentMessageType == ComponentMessageType.Conflict);
        }

        public string GetNotFounds()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var notFounds = _messages.Where(x => x.ComponentMessageType == ComponentMessageType.NotFound);
            foreach (var item in notFounds)
            {
                stringBuilder.AppendLine(item.Value);
            }
            return stringBuilder.ToString();
        }

        public bool HasNotFounds()
        {
            return _messages.Any(x => x.ComponentMessageType == ComponentMessageType.NotFound);
        }
    }
}
