using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Core.Models.Series
{
    public class DatetimeSerieModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }

        public DatetimeSerieListModel Availability { get; set; } = new DatetimeSerieListModel("Availability");

        public DatetimeSerieListModel Latency { get; set; } = new DatetimeSerieListModel("Latency");

        public DatetimeSerieListModel Experience { get; set; } = new DatetimeSerieListModel("Experience");

        public List<DatetimeSerieListModel> AvailabilityDetail { get; set; } = new List<DatetimeSerieListModel>();
        public List<DatetimeSerieListModel> ExperienceDetail { get; set; } = new List<DatetimeSerieListModel>();
        public List<DatetimeSerieListModel> LatencyDetail { get; set; } = new List<DatetimeSerieListModel>();

    }
}
