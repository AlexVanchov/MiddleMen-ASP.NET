using Microsoft.EntityFrameworkCore;
using MiddleMan.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "World Of Warcraft", Position = 1, });
            await dbContext.Categories.AddAsync(new Category { Name = "League Of Legends", Position = 2, });
            await dbContext.Categories.AddAsync(new Category { Name = "CS:GO", Position = 3, });
            await dbContext.Categories.AddAsync(new Category { Name = "Valorant", Position = 4, });
        }
    }
}
