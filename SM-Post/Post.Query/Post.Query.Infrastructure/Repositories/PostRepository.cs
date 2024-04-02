using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataBaseContextFactory _contextFactory;
        public PostRepository(DataBaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreateAsync(PostEntity post)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);
            if(post== null) return;
            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid postId)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts
                .Include( p => p.Comments)
                .FirstOrDefaultAsync( x => x.PostId == postId);
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include( p => p.Comments).AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListByAuthorAsync(string author)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include( p => p.Comments).AsNoTracking()
                .Where(x => x.Author != null && x.Author.Contains(author))
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include( p => p.Comments).AsNoTracking()
                .Where(x=>x.Comments != null && x.Comments.Count > 0)
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include( p => p.Comments).AsNoTracking()
                .Where(x => x.Likes >= numberOfLikes )
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DataBaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Update(post);
            await context.SaveChangesAsync();
        }
    }
}