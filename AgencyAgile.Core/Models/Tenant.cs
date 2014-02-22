using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.Models
{
    public class Tenant
    {
        public Guid TenantId { get; set; }

        public string Slug { get; set; }

        public string Name { get; set; }

        public AuditedAction Created { get; set; }

    }
}
