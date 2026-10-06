using HealthSync.BuildingBlocks.Abstraction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace HealthSync.BuildingBlocks.Core.Persistence.EfCore;

public class EfRepository<TDbContext, TEntity, TKey>(TDbContext dbContext, ISieveProcessor sieveProcessor)
    : EfRepositoryBase<TDbContext, TEntity, TKey>(dbContext, sieveProcessor)
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext;