﻿using ExceptionsDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExceptionsDemo.Database;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}