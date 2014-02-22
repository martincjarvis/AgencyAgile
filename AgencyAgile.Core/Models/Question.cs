using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgencyAgile.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; }

        public virtual Client Client { get; set; }

        public virtual Job Job { get; set; }

        public virtual Document Document { get; set; }

        public virtual CopyBlock Text { get; set; }

        public AuditedAction Created { get; set; }

        public AuditedAction LastUpdated { get; set; }

        public virtual ICollection<Response> Responses { get; set; }

        public virtual Response Answer { get; set; }

    }
}