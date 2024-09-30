﻿using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;

namespace Shop.Data
{
    public class ShopContext : DbContext

    {
        public ShopContext(DbContextOptions<ShopContext> options)
        : base(options) { }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
    }
}
