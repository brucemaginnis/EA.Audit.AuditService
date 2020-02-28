using System;
using System.Linq;
using EA.Audit.AuditService.Models;
using Microsoft.EntityFrameworkCore;

namespace EA.Audit.AuditService.Data
{
        public static class DbInitializer
    {
        public static void Initialize(AuditContext context)
        {
            context.Database.EnsureCreated();

            // Look for any audits.
            if (context.Audits.IgnoreQueryFilters().Any())
            {
                return;   // DB has been seeded
            }

            //Audit Types
            var auditTypes = new AuditType[]
            {
                new AuditType { Id = 1, Description = "Audit Event Type 1" },
                new AuditType { Id = 2, Description = "Audit Event Type 2" },
                new AuditType { Id = 3, Description = "Audit Event Type 3" }
            };

            foreach (AuditType s in auditTypes)
            {
                context.AuditTypes.Add(s);
            }
            context.SaveChanges();

           //Audit Levels
           var auditLevels = new AuditLevel[]
            {
                new AuditLevel { Id = 1, Description = "Information" },
                new AuditLevel { Id = 2, Description = "Warning" },
                new AuditLevel { Id = 3, Description = "Critical" }
            };

            foreach (AuditLevel s in auditLevels)
            {
                context.AuditLevels.Add(s);
            }
            context.SaveChanges();

            var audits = new AuditEntity[]
            {
                new AuditEntity { Id = 1, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 1).Id, AuditLevelId = auditLevels.Single(a => a.Id == 1).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 2, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 2).Id, AuditLevelId = auditLevels.Single(a => a.Id == 2).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 3, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 3).Id, AuditLevelId = auditLevels.Single(a => a.Id == 3).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 4, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 1).Id, AuditLevelId = auditLevels.Single(a => a.Id == 1).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 5, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 2).Id, AuditLevelId = auditLevels.Single(a => a.Id == 2).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 6, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 3).Id, AuditLevelId = auditLevels.Single(a => a.Id == 3).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 7, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 1).Id, AuditLevelId = auditLevels.Single(a => a.Id == 1).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 8, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 2).Id, AuditLevelId = auditLevels.Single(a => a.Id == 2).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 9, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 3).Id, AuditLevelId = auditLevels.Single(a => a.Id == 3).Id, Source = "Courts", Details = "some payload or something" },
                new AuditEntity { Id = 10, DateCreated = DateTime.Now, AuditTypeId = auditTypes.Single(a => a.Id == 1).Id, AuditLevelId = auditLevels.Single(a => a.Id == 1).Id, Source = "Courts", Details = "some payload or something" }
            };

            foreach (AuditEntity s in audits)
            {
                context.Audits.Add(s);
            }
            context.SaveChanges();
        }
    }
}