using BTB.Domain.Entities;
using BTB.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests.Common
{
    public class BTBContextFactory
    {
        public static BTBDbContext Create()
        {
            var options = new DbContextOptionsBuilder<BTBDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new BTBDbContext(options);

            context.Database.EnsureCreated();

            context.UserProfileInfo.AddRange(new[] {
                new UserProfileInfo { Id = 1, UserId = "1", Username = "UserOne", ProfileBio = "", FavouriteTradingPair = ""},
                new UserProfileInfo { Id = 2, UserId = "2", Username = "UserTwo", ProfileBio = "", FavouriteTradingPair = ""},
                new UserProfileInfo { Id = 3, UserId = "3", Username = "UserThree", ProfileBio = "", FavouriteTradingPair = ""}

            });

            context.SaveChanges();

            return context;
        }

        public static void Destroy(BTBDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
