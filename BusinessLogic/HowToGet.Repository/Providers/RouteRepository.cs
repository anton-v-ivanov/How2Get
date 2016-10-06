using System.Collections.Generic;
using BLToolkit.Data;
using BLToolkit.Mapping;
using HowToGet.Models.Exceptions;
using HowToGet.Repository.Interfaces;
using HowToGet.Models.Routes;
using HowToGet.Repository.Mappers;

namespace HowToGet.Repository.Providers
{
    public class RouteRepository : IRouteRepository
    {
        public IEnumerable<RoutePart> GetRoutePartsForOrigin(int originCityId, int destinationCityId, int maxTransferCount)
        {
            using (var db = new DbManager())
            {
                var table = db.SetSpCommand("GetRoutePartsForOrigin",
                                  db.Parameter("@originCityId", originCityId),
                                  db.Parameter("@destinationCityId", destinationCityId),
                                  db.Parameter("@iterations", maxTransferCount))
                    .ExecuteDataTable();
                
                return RouteMapper.MapDataTableToRoutePartsList(table);
            }
        }

        public Route GetRouteById(int id)
        {
            using (var db = new DbManager())
            {
                var dataSet = db.SetSpCommand("GetRouteById", db.Parameter("@routeId", id)).ExecuteDataSet();
                var route = RouteMapper.MapDataSetToSingleRoute(dataSet);
                if (route == null)
                    throw new ObjectNotFoundException(string.Format("Route with id = {0} was not found", id));
                return route;
            }
        }

        public void AddRoutePart(RoutePart routePart)
        {
            using (var db = new DbManager())
            {
                db
                    .SetSpCommand("InsertRoutePart",
                                  db.Parameter("@globalRouteId", routePart.GlobalRouteId),
                                  db.Parameter("@originCityId", routePart.OriginCityId),
                                  db.Parameter("@destinationCityId", routePart.DestinationCityId),
                                  db.Parameter("@carrierId", routePart.CarrierId),
                                  db.Parameter("@description", routePart.Description),
                                  db.Parameter("@time", routePart.Time),
                                  db.Parameter("@timeType", routePart.TimeType),
                                  db.Parameter("@cost", routePart.Cost),
                                  db.Parameter("@costCurrency", routePart.CostCurrency),
                                  db.Parameter("@date", routePart.Date))
                    .ExecuteNonQuery();
            }
        }

        public int CreateEmptyRoute()
        {
            using (var db = new DbManager())
            {
                return db.SetSpCommand("CreateEmptyRoute").ExecuteScalar<int>();
            }
        }
    }
}