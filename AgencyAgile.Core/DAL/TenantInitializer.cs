using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAgile.DAL
{
    public class TenantInitializer 
        : System.Data.Entity.DropCreateDatabaseIfModelChanges<TenantDbContext>
    {
    }
}
