using System;
using System.Linq;
using EA.Audit.AuditService.Model.Admin;
using EA.Audit.AuditService.Models;
using Microsoft.EntityFrameworkCore;

namespace EA.Audit.AuditService.Data
{
        public static class DbInitializer
    {
        public static void Initialize(AuditContext context)
        {
            context.Database.EnsureCreated();
            context.ClientId = "3inpv3ubfmag4k97cu5iqsesg8";

            // Look for any audits.
            if (context.Audits.IgnoreQueryFilters().Any())
            {
                return;   // DB has been seeded
            }

            //Audit Application
            var application = new AuditApplication()
            {
                Id = 123456,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Description = "Test Audit Application",
                Name = "TestAuditApplication",
                ClientId = "3inpv3ubfmag4k97cu5iqsesg8"
            };

            context.AuditApplications.Add(application);
            context.SaveChanges();

            var audits = new AuditEntity[]
            {
                new AuditEntity { Id = 1, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 2, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 3, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 4, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 5, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 6, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 7, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 8, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 9, DateCreated = DateTime.Now, AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" },
                new AuditEntity { Id = 10, DateCreated = DateTime.Now,AuditApplication = application,ClientId = "3inpv3ubfmag4k97cu5iqsesg8", SubjectId = 123456789, Subject = "Subject Info", ActorId = 987654321, Actor = "Actor Info", Description = "Subject updated", Properties = "{json: 'stuff'}" }
            };

            foreach (AuditEntity s in audits)
            {
                context.Audits.Add(s);
            }
            context.SaveChanges();
        }
    }
}