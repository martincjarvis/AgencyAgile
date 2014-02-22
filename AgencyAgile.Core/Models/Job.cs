using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.Models
{
    public class Job
    {

        public Guid JobId { get; set; }
             
        public virtual Client Client { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
                
        public AuditedAction Created { get; set; }

        public AuditedAction LastUpdated { get; set; }
                

    }
}
