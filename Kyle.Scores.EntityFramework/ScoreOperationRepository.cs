using Kyle.EntityFrameworkExtensions;
using Kyle.Scores.Domain;
using Kyle.Scores.Domain.Entities;

namespace Kyle.Scores.EntityFramework;

public class ScoreOperationRepository: EfCoreRepositoryBase<ScoreRecord>, IScoreOperationRepository
{
    public ScoreOperationRepository(MallDbContext context) : base(context)
    {
    }

    public async Task Insert(ScoreRecord model)
    {
        await dbSet.AddAsync(model);
        await Context.SaveChangesAsync();
    }
}