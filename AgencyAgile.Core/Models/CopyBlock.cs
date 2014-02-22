using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgencyAgile.Models
{
    public class CopyBlock
    {
        
        public Guid CopyBlockId { get; set; }
                        
        public virtual Fragment Latest { get; set; }

        public virtual ICollection<Fragment> History { get; set; }

        public virtual ICollection<CopyBlock> SubSections { get; set; }

    }
}