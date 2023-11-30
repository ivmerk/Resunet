using System;
using Resunet.DAL.Models;


namespace Resunet.DAL
{

  public interface IDbSessionDAL
  {
    Task<int> Create(SessionModel model);

    Task<SessionModel?> Get(Guid sessionId);

    Task<int> Update(SessionModel model);

    Task Lock(Guid sessionId);
  }

}