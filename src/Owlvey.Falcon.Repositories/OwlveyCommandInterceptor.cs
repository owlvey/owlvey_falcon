using Microsoft.EntityFrameworkCore.Diagnostics;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Repositories
{
    public class OwlveyCommandInterceptor : DbCommandInterceptor
    {
        private void MeasureElapsed(DateTimeOffset startTime) {

            var anchor = DateTimeOffset.Now;
            if (startTime < anchor) {
                return;
            }
            var elapsed = anchor.Subtract(startTime);
            RepositoryMetrics.RelationAccessDuration.Observe(elapsed.TotalMilliseconds);
        }

        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {            
            MeasureElapsed(eventData.StartTime);
            return base.ReaderExecuted(command, eventData, result);
        }

        public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            MeasureElapsed(eventData.StartTime);
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }
        public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        {
            MeasureElapsed(eventData.StartTime);
            return base.NonQueryExecuted(command, eventData, result);
        }
        public override object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result)
        {
            MeasureElapsed(eventData.StartTime);
            return base.ScalarExecuted(command, eventData, result);
        }

        public override Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            MeasureElapsed(eventData.StartTime);
            return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            MeasureElapsed(eventData.StartTime);
            return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
        }
    }
}
