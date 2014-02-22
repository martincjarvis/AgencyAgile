using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgencyAgile.Models
{
    public class Fragment
    {
        public Guid FragmentId { get; set; }

        public string Title { get; set; }

        public string Markup { get; set; }
        
        public AuditedAction Created { get; set; }
                
    }
}