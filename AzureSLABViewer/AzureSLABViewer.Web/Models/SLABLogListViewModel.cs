using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSLABViewer.Web.Models
{
    public class SLABLogListViewModel
    {
        public string SelectedSLABLogTable { get; set; }

        public IEnumerable<SlabLogsTable> SLABLogItems {get; set;}

        public IEnumerable<CloudTable> SLABLogTables { get; set; }

        public static SLABLogListViewModel Create(IEnumerable<SlabLogsTable> slabLogItems,
                                           IEnumerable<CloudTable> slabLogTables,
                                           string selectedSLABLogTable)
        {
            return new SLABLogListViewModel()
            {
                SLABLogItems = slabLogItems,
                SLABLogTables = slabLogTables,
                SelectedSLABLogTable = selectedSLABLogTable
            };
        }
    }
}