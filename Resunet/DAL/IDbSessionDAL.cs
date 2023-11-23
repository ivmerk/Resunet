using System;
using Resunet.DAL.Models;


namespace Resunet.DAL
{

  public interface IDbSessionDAL
  {
    Task<int> CreateSession(SessionModel model);

    Task<SessionModel?> GetSession(Guid sessionId);

    Task<int> UpdateSession(SessionModel model);
  }

}