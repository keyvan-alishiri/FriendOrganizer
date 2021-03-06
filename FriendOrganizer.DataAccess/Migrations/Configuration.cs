﻿namespace FriendOrganizer.DataAccess.Migrations
{
    using FriendOrganizer.Model;
    using System;
   using System.Collections.Generic;
   using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizer.DataAccess.FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(f => f.FirstName,
                new Friend { FirstName = "keyvan", LastName = "Alishir" },
            new Friend { FirstName = "peyman", LastName = "Meier" },
             new Friend { FirstName = "Sara", LastName = "SHafie" },
              new Friend { FirstName = "Thomas", LastName = "Huber" }
             );

         context.ProgrammingLanguages.AddOrUpdate(pl=> pl.Name,
                new ProgrammingLanguage { Name = "C#" },
            new ProgrammingLanguage { Name = "TypeScript" },
             new ProgrammingLanguage { Name = "F#" },
              new ProgrammingLanguage { Name = "Java" },
               new ProgrammingLanguage { Name = "Swift" }
             );

         context.SaveChanges();
         context.FriendPhoneNumbers.AddOrUpdate(pn => pn.Number,
          new FriendPhoneNumber { Number = "+98 12345678", FriendId = context.Friends.First().Id });

         context.Meetings.AddOrUpdate(m => m.Title,

            new Meeting
            {
               Title = "Watvhing Soccer",
               DateFrom = new DateTime(2020, 5, 26),
               DateTo = new DateTime(2020, 5, 26),
               Friends = new List<Friend>()
               {
                  context.Friends.Single(f=>f.FirstName == "Sara" && f.LastName == "Shafie"),
                  context.Friends.Single(f=>f.FirstName == "peyman" && f.LastName == "Meier"),
               }


            });
        








       
         //  This method will be called after migrating to the latest version.

         //  You can use the DbSet<T>.AddOrUpdate() helper extension method
         //  to avoid creating duplicate seed data.
      }
    }
}
