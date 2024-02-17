using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly SimUDbContext _dbContext;
    
    public LocationRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    private async Task<Character?> GetCharacter(Guid characterId, bool isAgent)
    {
        return isAgent
            ? await _dbContext.Agents.FirstOrDefaultAsync(a => a.Id == characterId)
            : await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == characterId);
    }

    public async Task<Location?> GetLocation(Guid characterId, bool isAgent = false)
    {
        var character = await GetCharacter(characterId, isAgent)
            ?? throw new NotFoundException(isAgent ? nameof(Agent) : nameof(User), characterId);
        return character.Location;
    }

    public async Task UpdateLocation(Guid characterId, int x_coord, int y_coord, bool isAgent = false)
    {
        var character = await GetCharacter(characterId, isAgent)
            ?? throw new NotFoundException(isAgent ? nameof(Agent) : nameof(User), characterId);
        character.UpdateLocation(x_coord, y_coord);
        await _dbContext.SaveChangesAsync();
    }
}