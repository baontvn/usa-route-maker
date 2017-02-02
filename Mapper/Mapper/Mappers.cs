using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class Mappers
    {
        public class States
        {
            public string Name { get; set; }
            public string ShortName { get; set; }
            public string Location { get; set; }
        }

        public class City
        {
            public string StateName { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
        }

        public class Route
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
            public string CityName { get; set; }
            public string StateCode { get; set; }
        }

        public class RouteRequest
        {
            public long id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string type { get; set; }
            public string startLocation { get; set; }
            public string endLocation { get; set; }
            public string outFile { get; set; }
        }

        public class Country
        {
            public string Name { get; set; }
            public List<States> States { get; set; }
        }

        public class Location
        {
            public Position location { get; set; }
        }
        public class Position
        {
            public string Lat { get; set; }
            public string Lng { get; set; }

        }

        public class CityRequest
        {
            public string State { get; set; }
            public string City { get; set; }
            public List<RouteLocation> Routes { get; set; }

        }

        public class RouteLocation
        {
            public string Pole { get; set; }
            public string StartingPoint { get; set; }
            public string Destination { get; set; }
        }
        
    }
}
