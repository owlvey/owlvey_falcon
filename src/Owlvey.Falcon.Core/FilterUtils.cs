using System;
namespace Owlvey.Falcon.Core
{
    public enum FilterOperator {
        eq,
        ne,
        lk
    }
    public static class FilterUtils
    {
        public static (string field, FilterOperator opera, string value) ParseQuery(string target) {
            var items = target.Split(" ");
            return (items[0], Enum.Parse<FilterOperator>(items[1]), items[2]);
        }
    }
}
