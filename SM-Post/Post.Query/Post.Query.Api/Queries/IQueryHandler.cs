using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries
{
    public interface IQueryHandler
    {
        Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query);
        Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query);
    }
}