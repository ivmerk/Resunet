using System;
using Dapper;
using Npgsql;
using Resunet.DAL.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Resunet.DAL
{
  public class AuthDAL : IAuthDAL
  {
    public async Task<UserModel> GetUser(string email)
    {
      using (var connection = new NpgsqlConnection(DbHelper.ConnString))
      {
        await connection.OpenAsync();

        return await connection.QueryFirstOrDefaultAsync<UserModel>(@"
                        select UserId, Email, Password, Salt, Status
                        from AppUser where Email = @email", new { email = email }) ?? new UserModel(); ;
      }
    }
    public async Task<UserModel> GetUser(int id)
    {
      using (var connection = new NpgsqlConnection(DbHelper.ConnString))
      {
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<UserModel>(@"
                        select UserId, Email, Password, Salt, Status
                        from AppUser where UserId = @id", new { id = id }) ?? new UserModel(); ;
      }
    }
    public async Task<int> CreateUser(UserModel model)
    {
      Console.WriteLine(model.Email, model.Password);

      using (var connection = new NpgsqlConnection(DbHelper.ConnString))
      {
        await connection.OpenAsync();
        string sql = @"insert into AppUser(Email, Password, Salt, Status)
                        values(@Email, @Password, @Salt, @Status) returning UserId";
        return await connection.QuerySingleAsync<int>(sql, model);
      }
    }
  }
}

