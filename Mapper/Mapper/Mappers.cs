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
    }
}
