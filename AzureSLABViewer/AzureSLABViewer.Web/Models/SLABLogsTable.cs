using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSLABViewer.Web.Models
{
    public class SLABLogsTable : TableEntity
    {
        public SLABLogsTable()
            : base()
        {
        }

        public SLABLogsTable(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public DateTime EventDate { get; set; }

        public int EventId { get; set; }

        public string InstanceName { get; set; }

        public int Keywords { get; set; }

        public int Level { get; set; }

        public string Message { get; set; }

        public int Opcode { get; set; }

        public string Payload { get; set; }

        public Guid ProviderId { get; set; }

        public string ProviderName { get; set; }

        public int Task { get; set; }

        public int Version { get; set; }
    }
}