// https://refactoring.guru/design-patterns/facade
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HealthSync.BuildingBlocks.Abstraction.Persistence;

public interface IDbFacadeResolver
{
    DatabaseFacade Database { get; }
}