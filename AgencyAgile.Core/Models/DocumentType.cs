using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.Models
{
    public class DocumentType
    {

        public Guid DocumentTypeId { get; set; }

        public int SortOrder { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public int? LimitPerJob { get; set; }

        public bool HasFeatures { get; set; }

        public bool HasTasks { get; set; }

        public AuditedAction Created { get; set; }

        public AuditedAction LastUpdated { get; set; }

    }
}
