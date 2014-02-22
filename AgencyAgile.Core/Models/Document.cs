using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.Models
{
    public class Document
    {

        public Guid DocumentId { get; set; }

        public string Slug { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public virtual Client Client { get; set; }

        public virtual Job Job { get; set; }

        public string Title { get; set; }

        public virtual ICollection<CopyBlock> Sections { get; set; }

        public virtual ICollection<Feature> Features { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public AuditedAction Created { get; set; }

        public AuditedAction LastUpdated { get; set; }

    }
}
