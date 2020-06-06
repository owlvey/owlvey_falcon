using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Series
{
    public class DatetimeSerieListModel 
    {
        public string Name { get; set; }
        public string Avatar { get; set; }

        public List<DatetimeSerieItemModel> Items { get; set; } = new List<DatetimeSerieItemModel>();

        public DatetimeSerieListModel() { }

        public DatetimeSerieListModel(string name, string avatar =null) {
            this.Avatar = avatar;
            this.Name = name;
        }

        public void AddItems(IEnumerable<(DateTime target, decimal value)> target) {
            foreach (var item in target)
            {
                this.Items.Add(new DatetimeSerieItemModel(item.target, item.value));
            }
        }
    }
}
