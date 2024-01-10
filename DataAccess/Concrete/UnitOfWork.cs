using DataAccess.Abstract;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private PostRepository _postRepository;
        private UserRepository _userRepository;
        private CommentRepository _commentRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IPostRepository Posts => _postRepository = _postRepository ?? new PostRepository(_context);
        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);
        public ICommentRepository Comments => _commentRepository = _commentRepository ?? new CommentRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
