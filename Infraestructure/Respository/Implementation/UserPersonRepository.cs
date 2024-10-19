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
    public class UserPersonRepository : RepositoryAsync<UserPerson>, IUserPersonRepository
    {
        public UserPersonRepository(string connectionString) : base(connectionString)
        {

        }
        public async Task<UserPerson> LoginAsync(UserPerson user)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("pusername", user.Email, OracleMappingType.NVarchar2, ParameterDirection.Input);
                dyParam.Add("pmoduleid", user.ModuleID, OracleMappingType.Int16, ParameterDirection.Input);
                dyParam.Add("cursorOut", null, OracleMappingType.RefCursor, ParameterDirection.Output);

                using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    return await conn.QueryFirstOrDefaultAsync<UserPerson>("PKG_SECURITY.SEC_USERPERSON_LOGIN", dyParam, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
