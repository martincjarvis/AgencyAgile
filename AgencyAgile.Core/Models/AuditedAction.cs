using System;
using System.ComponentModel.DataAnnotations;
namespace AgencyAgile.Models
{
    public class AuditedAction
    {
        public DateTime At { get; set; }

        public string ByUserName { get; set; }

        public string ById { get; set; }

        

        public static AuditedAction Create(ApplicationUser user)
        {
            return new AuditedAction
            {
                At = DateTime.UtcNow
                ,
                ById = user.Id
                ,
                ByUserName = user.UserName
            };
        }

        public static AuditedAction Create(string userId, string userName)
        {
            return new AuditedAction
            {
                At = DateTime.UtcNow
                ,
                ById = userId
                ,
                ByUserName = userName
            };
        }

        public AuditedAction Clone()
        {
            return new AuditedAction
            {
                At = At
                ,
                ByUserName = ByUserName
                ,
                ById = ById
            };
        }
    }
}
