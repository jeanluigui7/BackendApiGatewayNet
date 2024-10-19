using Dapper;
using Dapper.Oracle;
using Domain.Models;
using Infraestructure.Respository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infraestructure.Respository.Implementation
{
    public class UserTokenRepository : RepositoryAsync<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task<int> CreateUserTokenAsync(UserToken userToken)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("puserid", userToken.UserID, OracleMappingType.Int32, ParameterDirection.Input);
                dyParam.Add("ptoken", userToken.Token, OracleMappingType.NVarchar2, ParameterDirection.Input);
                dyParam.Add("pstatusid", userToken.StatusID, OracleMappingType.Int32, ParameterDirection.Input);
                dyParam.Add("pcreatedby", userToken.CreatedBy, OracleMappingType.Int32, ParameterDirection.Input);
                dyParam.Add("IDOUT", null, OracleMappingType.Decimal, ParameterDirection.Output);

                using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    await conn.ExecuteAsync("PKG_SECURITY.SEC_USERTOKEN_INSERT", dyParam, commandType: CommandType.StoredProcedure);
                    var Id = dyParam.Get<decimal>("IDOUT");
                    return Convert.ToInt32(Id);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserToken> GetByTokenAsync(string token)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("ptoken", token, OracleMappingType.Varchar2, ParameterDirection.Input);
                dyParam.Add("cursorUserToken", null, OracleMappingType.RefCursor, ParameterDirection.Output);
                using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<UserToken>("PKG_SECURITY.SEC_USERTOKEN_GETACTIVE", dyParam, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> UpdateUserTokenAsync(UserToken userToken)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("ptoken", userToken.Token, OracleMappingType.NVarchar2, ParameterDirection.Input);
                dyParam.Add("pstatusid", userToken.StatusID, OracleMappingType.Int32, ParameterDirection.Input);
                dyParam.Add("pupdatedby", userToken.UpdatedBy, OracleMappingType.Int32, ParameterDirection.Input);
                dyParam.Add("rowsOut", null, OracleMappingType.Decimal, ParameterDirection.Output);
                using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    await conn.ExecuteAsync("PKG_SECURITY.SEC_USERTOKEN_UPDATE", dyParam, commandType: CommandType.StoredProcedure);
                    var id = (int)dyParam.Get<decimal>("rowsOut");
                    return id;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
