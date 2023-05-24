using Domain;

namespace Services.DataContract;

public interface IReactionRepository
{
    Task CreateReaction(AdvertReaction advertReaction);
    
    Task UpdateReaction(AdvertReaction advertReaction);
}