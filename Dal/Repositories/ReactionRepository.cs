using Domain;
using Services.DataContract;

namespace Dal.Repositories;

public class ReactionRepository : IReactionRepository
{
    private readonly BulletinBoardDbContext _dbContext;

    public ReactionRepository(BulletinBoardDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task CreateReaction(AdvertReaction advertReaction)
    {
        _dbContext.AdvertReactions.Add(advertReaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateReaction(AdvertReaction advertReaction)
    {
        _dbContext.AdvertReactions.Update(advertReaction);
        await _dbContext.SaveChangesAsync();
    }
}