using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommetRespository : ICommentRepository
    {
        private readonly DataBaseContextFactory _contextFactory;
        public CommetRespository(DataBaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreateAsync(CommentEntity comment)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);
            if(comment == null ) return;
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Comments
                .FirstOrDefaultAsync(x => x.CommentId == commentId);        
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Update(comment);
            await context.SaveChangesAsync();
        }
    }
}