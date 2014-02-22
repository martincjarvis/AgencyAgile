using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgencyAgile.Models
{
    public class Feature
    {
        
        public Guid FeatureId { get; set; }
                        
        public virtual CopyBlock Introduction { get; set; }

        public virtual Feature Parent { get; set; }

        public virtual ICollection<Feature> SubFeatures { get; set; }

        public AuditedAction Created { get; set; }

        public AuditedAction LastUpdated { get; set; }
    }
}