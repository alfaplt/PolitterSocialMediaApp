using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
	public interface IUnitOfWork : IDisposable
	{
		IPostRepository Posts { get; }
		IUserRepository Users { get; }
		ICommentRepository Comments { get; }
        Task<int> CommitAsync();
    }
}
