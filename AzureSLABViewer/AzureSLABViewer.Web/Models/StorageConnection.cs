using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureSLABViewer.Web.Models
{
    public class StorageConnection
    {
        [Key]
        public int StorageConnectionID { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(500)]
        public string ConnectionString { get; set; }
    }
}