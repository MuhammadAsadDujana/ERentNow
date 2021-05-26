using Core.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Billing
{
    public class BillingService
    {
        eRentEntities DbContext;

        public BillingService()
        {
            DbContext = new eRentEntities();
        }

        public dynamic BLLGetBilling(RequestModel request)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@Offset",request.Offset),
                new SqlParameter("@PageSize",request.PageSize),
                new SqlParameter("@UserID",request.UserID)
            };
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(request.search))
            {
                param.Add(new SqlParameter("@CustomerPO", "%" + request.search + "%"));
                param.Add(new SqlParameter("@CreatedOn", "%" + request.search + "%"));
                param.Add(new SqlParameter("@TotalRentAmount", "%" + request.search + "%"));
                sb.Append("where CustomerPO LIKE @CustomerPO Or convert(varchar(10),CreatedOn,103) Like @CreatedOn Or TotalRentAmount Like @TotalRentAmount");//Or convert(varchar(10),alh.Created,103) Like @Created
            }
            string Query = @"select * from ReservationHdr {0} order by CreatedOn desc OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
            string AppendedQuery = String.Format(Query, sb.ToString());

            return DbContext.ReservationHdrs.SqlQuery(AppendedQuery, param.ToArray()).Where(x => x.IsDeleted == false && x.CreatedBy == request.UserID).Select(x => new { id= x.id, CustomerPO = x.CustomerPO, CreatedOn = x.CreatedOn, TotalRentAmount = x.TotalRentAmount }).ToList();

        }
    }
}
