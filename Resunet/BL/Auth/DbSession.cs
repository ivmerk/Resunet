using Resunet.DAL.Models;
using Resunet.DAL;

namespace Resunet.BL.Auth
{
  public class DbSession : IDbSession
  {
    private readonly IDbSessionDAL sessionDAL;
    private readonly IHttpContextAccessor httpContextAccessor;

    public DbSession(IDbSessionDAL sessionDAL, IHttpContextAccessor httpContextAccessor)
    {
      this.sessionDAL = sessionDAL;
      this.httpContextAccessor = httpContextAccessor;
    }

    private void CreateCookie(Guid sessionid)
    {
      CookieOptions options = new()
      {
        Path = "/",
        HttpOnly = true,
        Secure = true
      };
      httpContextAccessor?.HttpContext?.Response.Cookies.Delete(AuthConstants.SessionCookieName);
      httpContextAccessor?.HttpContext?.Response.Cookies.Append(AuthConstants.SessionCookieName, sessionid.ToString(), options);
    }

    private async Task<SessionModel> Create()
    {
      var data = new SessionModel()
      {
        DbSessionId = Guid.NewGuid(),
        Created = DateTime.Now,
        LastAccessed = DateTime.Now
      };
      await sessionDAL.Create(data);
      return data;
    }

    private SessionModel? sessionModel = null;
    public async Task<SessionModel> Get()
    {
      if (sessionModel != null)
        return sessionModel;

      Guid sessionId;
      var cookie = httpContextAccessor?.HttpContext?.Request?.Cookies.FirstOrDefault(m => m.Key == AuthConstants.SessionCookieName);
      if (cookie != null && cookie.Value.Value != null)
        sessionId = Guid.Parse(cookie.Value.Value);
      else
        sessionId = Guid.NewGuid();

      var data = await sessionDAL.Get(sessionId);
      if (data == null)
      {
        data = await Create();
        CreateCookie(data.DbSessionId);
      }
      sessionModel = data;
      return data;
    }

    public async Task<int> SetUserId(int userId)
    {
      var data = await Get();
      data.UserId = userId;
      data.DbSessionId = Guid.NewGuid();
      CreateCookie(data.DbSessionId);
      return await sessionDAL.Create(data);
    }

    public async Task<int?> GetUserId()
    {
      var data = await Get();
      return data.UserId;
    }

    public async Task<bool> IsLoggedIn()
    {
      var data = await Get();
      return data.UserId != null;
    }

    public async Task Lock()
    {
      var data = await Get();
      await sessionDAL.Lock(data.DbSessionId);
    }
  }
}

