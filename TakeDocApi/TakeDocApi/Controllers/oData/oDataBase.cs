using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using TakeDocModel;
using Microsoft.Data.OData;

namespace TakeDocApi.Controllers.oData
{
    public abstract class oDataBase<T> : ODataController where T : class
    {
        protected TakeDocEntities1 _db = new TakeDocEntities1();

        public ICollection<T> Get(ODataQueryOptions<T> queryOptions, ODataValidationSettings validationSettings)
        {
            ICollection<T> retour;
            try
            {
                queryOptions.Validate(validationSettings);
                IQueryable query = queryOptions.ApplyTo(_db.Set<T>().AsQueryable());
                IQueryable<T> data = query as IQueryable<T>;
                retour = data.ToList();
            }
            catch (ODataException ex)
            {
                retour = null;
            }
            return retour;
        }
    }
}