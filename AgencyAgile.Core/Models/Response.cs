using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgencyAgile.Models
{
    public class Response
    {
      
        public Guid ResponseId { get; set; }

        public virtual Question Question { get; set; }
      
        public virtual CopyBlock Text { get; set; }

        public AuditedAction Created { get; set; }

        public AuditedAction LastUpdated { get; set; }

        public bool IsAnswer { get; set; }

    }
}