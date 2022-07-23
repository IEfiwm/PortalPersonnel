using CodeFirstStoredProcs;
using System.Collections.Generic;
using System.Data.Common;

namespace FarzinTools.Models
{
    public class StoreProcedureConfiguration
    {
        private readonly eOrganizationEntities1 _context = new eOrganizationEntities1();

        public ResultStoredProcedure<TOutput1, TOutput2, TOutput3, TOutput4> ExecuteStoredProcedure<T, TOutput1, TOutput2, TOutput3, TOutput4>(string spName, T data, string schemaName = "", int? commandTimeout = null, DbTransaction transaction = null) where T : class
        {
            var storedProc = new StoredProc<T>(_context)
                .HasName(spName)
                .HasOwner(schemaName.ToLower())
                .ReturnsTypes(
                    typeof(TOutput1),
                    typeof(TOutput2),
                    typeof(TOutput3),
                    typeof(TOutput4));

            var resultList = _context.CallStoredProc(storedProc, commandTimeout, transaction, data);

            return new ResultStoredProcedure<TOutput1, TOutput2, TOutput3, TOutput4>
            {
                List1 = resultList.ToList<TOutput1>(),
                List2 = resultList.ToList<TOutput2>(),
                List3 = resultList.ToList<TOutput3>(),
                List4 = resultList.ToList<TOutput4>()
            };
        }
    }

    public class ResultStoredProcedure<T1, T2, T3, T4>
    {
        public IEnumerable<T1> List1 { get; set; }

        public IEnumerable<T2> List2 { get; set; }

        public IEnumerable<T3> List3 { get; set; }

        public IEnumerable<T4> List4 { get; set; }
    }
}