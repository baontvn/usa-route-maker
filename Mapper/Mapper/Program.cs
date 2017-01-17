using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Mapper;
using Newtonsoft.Json;

namespace Mapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //var directory = @"C:\Users\BaoNT\Desktop\route_maker.json";

            var listOfStates = GetStates(false);
            var listOfCities = GetCities(listOfStates, false);
            //var listOfRoutes = GetRoute(listOfCities, false);

            #region Routes request
            var cityRequest = GenerateCityResquest(listOfCities);

            foreach (var city in cityRequest.GroupBy(c => c.State).ToArray())
            {
                var data = JsonConvert.SerializeObject(city);

                WriteFile(data, @"D:\lab\usa-route-maker\routes_data\" + city.Key.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_routes.json");
            }
            #endregion

            #region Read route json -> Generate to request Json
            //string text = System.IO.File.ReadAllText(@"D:\lab\usa-route-maker\AL_routes.json");

            //var rawData = JsonConvert.DeserializeObject<List<Mappers.CityRequest>>(text);

            //var cityRequest =  GetRouteRequest(rawData);

            //var json = JsonConvert.SerializeObject(cityRequest);

            //WriteFile(json, @"D:\lab\usa-route-maker\fm_routes.json");
            #endregion

            Console.WriteLine("Done");
            Console.ReadLine();

        }

        static void WriteFile(string data, string directory)
        {
            File.WriteAllText(directory, data);
        }

        static List<Mappers.States> GetStates(bool isGetLocation)
        {
            #region RAW DATA - States

            var rawData = @"Alabama 
Alaska 
Arizona 
Arkansas 
California 
Colorado 
Connecticut 
Delaware 
Florida 
Georgia 
Hawaii 
Idaho 
Illinois 
Indiana 
Iowa 
Kansas 
Kentucky 
Louisiana 
Maine 
Maryland 
Massachusetts 
Michigan 
Minnesota 
Mississippi 
Missouri 
Montana 
Nebraska 
Nevada 
New Hampshire 
New Jersey 
New Mexico 
New York 
North Carolina 
North Dakota 
Ohio 
Oklahoma 
Oregon 
Pennsylvania 
Rhode Island 
South Carolina 
South Dakota 
Tennessee 
Texas 
Utah 
Vermont 
Virginia 
Washington 
West Virginia 
Wisconsin 
Wyoming";
            #endregion

            string pattern = " \\r\\n";
            Regex rgx = new Regex(pattern);
            var states = rgx.Split(rawData); ;

            var result = new List<Mappers.States>();
            var shortStateName = GetStateCode();
            for (int i = 0; i < states.Length; i++)
            {
                var state = new Mappers.States();
                if (isGetLocation)
                {
                    var location = GetLocation(states[i]);
                    state.Name = states[i];
                    state.ShortName = shortStateName[states[i]];
                    state.Location = location[0] + "," + location[1];
                }
                else
                {
                    state.Name = states[i];
                    state.ShortName = shortStateName[states[i]];
                }

                result.Add(state);
            }
            return result;
        }

        static List<Mappers.City> GetCities(List<Mappers.States> states, bool isGetLocation)
        {
            #region Crawl data

            //string Url = "https://en.wikipedia.org/wiki/List_of_U.S._states\'_largest_cities_by_population";
            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load(Url);

            //string metascore = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/table[2]")[0].InnerText;

            #endregion

            string metascore = @"Alabama\n4,817,786\nBirmingham\n212,237\nMontgomery\nMobile\nHuntsville\nTuscaloosa|Alaska\n710,231\nAnchorage\n291,826\nJuneau\nFairbanks\nSitka\nKetchikan|Arizona\n6,392,017\nPhoenix\n1,445,632\nTucson\nMesa\nChandler\nGlendale|Arkansas\n2,915,918\nLittle Rock\n193,524\nFort Smith\nFayetteville\nSpringdale\nJonesboro|California\n37,253,956\nLos Angeles\n3,792,621\nSan Diego\nSan Jose\nSan Francisco\nFresno\nSacramento|Colorado\n5,029,196\nDenver\n600,158\nColorado Springs\nAurora\nFort Collins\nLakewood|Connecticut\n3,574,097\nBridgeport\n144,229\nNew Haven\nStamford\nHartford\nWaterbury|Delaware\n897,934\nWilmington\n70,851\nDover\nNewark\nMiddletown\nSmyrna|Florida\n18,801,310\nJacksonville\n821,784\nMiami\nTampa\nOrlando\nSt. Petersburg\nTallahassee|Georgia\n9,687,653\nAtlanta\n420,003\nColumbus\nAugusta\nMacon\nSavannah|Hawaii\n1,360,301\nHonolulu\n337,256\nHilo1\nKailua1\nKapolei1\nKaneohe1|Idaho\n1,567,582\nBoise\n205,671\nNampa\nMeridian\nIdaho Falls\nPocatello|Illinois\n12,830,632\nChicago\n2,695,598\nAurora\nRockford\nJoliet\nNaperville\nSpringfield|Indiana\n6,483,802\nIndianapolis\n820,445\nFort Wayne\nEvansville\nSouth Bend\nCarmel|Iowa\n3,046,355\nDes Moines\n203,433\nCedar Rapids\nDavenport\nSioux City\nWaterloo|Kansas\n2,853,118\nWichita\n382,368\nOverland Park\nKansas City\nOlathe\nTopeka|Kentucky\n4,339,367\nLouisville\n597,337\nLexington\nBowling Green\nOwensboro\nCovington\nFrankfort|Louisiana\n4,533,372\nNew Orleans\n343,829\nBaton Rouge\nShreveport\nLafayette\nLake Charles|Maine\n1,328,361\nPortland\n66,194\nLewiston\nBangor\nSouth Portland\nAuburn\nAugusta|Maryland\n5,773,552\nBaltimore\n620,961\nColumbia\nGermantown\nSilver Spring\nWaldorf\nAnnapolis|Massachusetts\n6,547,629\nBoston\n617,594\nWorcester\nSpringfield\nLowell\nCambridge|Michigan\n9,883,640\nDetroit\n713,777\nGrand Rapids\nWarren\nSterling Heights\nAnn Arbor\nLansing|Minnesota\n5,303,925\nMinneapolis\n382,578\nSaint Paul\nRochester\nBloomington\nDuluth|Mississippi\n2,967,297\nJackson\n173,514\nGulfport\nSouthaven\nHattiesburg\nBiloxi|Missouri\n5,988,927\nKansas City\n459,787\nSaint Louis\nSpringfield\nIndependence\nColumbia\nJefferson City|Montana\n989,415\nBillings\n104,170\nMissoula\nGreat Falls\nBozeman\nButte\nHelena|Nebraska\n1,826,341\nOmaha\n408,958\nLincoln\nBellevue\nGrand Island\nKearney|Nevada\n2,700,551\nLas Vegas\n583,756\nHenderson\nReno\nNorth Las Vegas\nSparks\nCarson City|New Hampshire\n1,316,470\nManchester\n109,565\nNashua\nConcord\nDerry\nRochester|New Jersey\n8,791,894\nNewark\n277,140\nJersey City\nPaterson\nElizabeth\nEdison\nTrenton|New Mexico\n2,059,179\nAlbuquerque\n545,852\nLas Cruces\nRio Rancho\nSanta Fe\nRoswell|New York\n19,378,102\nNew York City\n8,175,133\nBuffalo\nRochester\nYonkers\nSyracuse\nAlbany|North Carolina\n9,535,483\nCharlotte\n731,424\nRaleigh\nGreensboro\nDurham\nWinston-Salem|North Dakota\n672,591\nFargo\n105,549\nBismarck\nGrand Forks\nMinot\nWest Fargo|Ohio\n11,536,504\nColumbus\n787,033\nCleveland\nCincinnati\nToledo\nAkron|Oklahoma\n3,751,351\nOklahoma City\n579,999\nTulsa\nNorman\nBroken Arrow\nLawton|Oregon\n3,831,074\nPortland\n583,776\nSalem\nEugene\nGresham\nHillsboro|Pennsylvania\n12,702,379\nPhiladelphia\n1,526,006\nPittsburgh\nAllentown\nErie\nReading\nHarrisburg|Rhode Island\n1,052,567\nProvidence\n178,042\nWarwick\nCranston\nPawtucket\nEast Providence|South Carolina\n4,625,364\nColumbia\n129,272\nCharleston\nNorth Charleston\nMount Pleasant\nRock Hill|South Dakota\n814,180\nSioux Falls\n153,888\nRapid City\nAberdeen\nBrookings\nWatertown\nPierre|Tennessee\n6,346,105\nMemphis\n646,889\nNashville\nKnoxville\nChattanooga\nClarksville|Texas\n25,145,561\nHouston\n2,099,451\nSan Antonio\nDallas\nAustin\nFort Worth|Utah\n2,763,885\nSalt Lake City\n186,440\nWest Valley City\nProvo\nWest Jordan\nOrem|Vermont\n625,741\nBurlington\n42,417\nEssex\nSouth Burlington\nColchester\nRutland\nMontpelier|Virginia\n8,001,024\nVirginia Beach\n437,994\nNorfolk\nChesapeake\nArlington\nRichmond|Washington\n6,724,540\nSeattle\n608,660\nSpokane\nTacoma\nVancouver\nBellevue\nOlympia|West Virginia\n1,852,994\nCharleston\n51,400\nHuntington\nParkersburg\nMorgantown\nWheeling|Wisconsin\n5,686,986\nMilwaukee\n594,833\nMadison\nGreen Bay\nKenosha\nRacine|Wyoming\n563,626\nCheyenne\n59,466\nCasper\nLaramie\nGillette\nRock Springs";
            string pattern = @"|";
            Regex rgx = new Regex(pattern);
            var data = metascore.Split('|');
            var citiesOfState = new List<Mappers.City>();
            for (int i = 0; i < data.Length; i++)
            {
                var cities = ParseCities(data[i], states, isGetLocation);
                citiesOfState = ConcatArrays<Mappers.City>(citiesOfState.ToArray(), cities.ToArray());
            }
            return citiesOfState;
        }

        static List<Mappers.City> ParseCities(string rawData, List<Mappers.States> states, bool isGetLocation)
        {
            var result = new List<Mappers.City>();

            string pattern = @"\\n";
            Regex rgx = new Regex(pattern);
            var data = rgx.Split(rawData);
            var state = states.FirstOrDefault(s => s.Name.Equals(data[0]))?.Name;

            var cityLocation = isGetLocation == true ? GetLocation(data[2], state) : new List<string>();
            var cityLocation2 = isGetLocation == true ? GetLocation(data[4], state) : new List<string>();
            var cityLocation3 = isGetLocation == true ? GetLocation(data[5], state) : new List<string>();
            var cityLocation4 = isGetLocation == true ? GetLocation(data[6], state) : new List<string>();
            var cityLocation5 = isGetLocation == true ? GetLocation(data[7], state) : new List<string>();
            result.Add(new Mappers.City()
            {
                Name = data[2],
                Location = cityLocation.Any() ? cityLocation[0] + "," + cityLocation[1] : "",
                StateName = state
            });

            result.Add(new Mappers.City()
            {
                Name = data[4],
                Location = cityLocation2.Any() ? cityLocation2[0] + "," + cityLocation2[1] : "",
                StateName = state
            });
            result.Add(new Mappers.City()
            {
                Name = data[5],
                Location = cityLocation3.Any() ? cityLocation3[0] + "," + cityLocation3[1] : "",
                StateName = state
            });
            result.Add(new Mappers.City()
            {
                Name = data[6],
                Location = cityLocation4.Any() ? cityLocation4[0] + "," + cityLocation4[1] : "",
                StateName = state
            });
            result.Add(new Mappers.City()
            {
                Name = data[7],
                Location = cityLocation5.Any() ? cityLocation5[0] + "," + cityLocation5[1] : "",
                StateName = state
            });

            return result;
        }

        static List<Mappers.Route> GetRoute(List<Mappers.City> cities, bool isGetLocation)
        {
            var routes = new List<Mappers.Route>();
            var counter = 1;
            foreach (var city in cities)
            {
                var currentRoutes = new List<Mappers.Route>();
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_East",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_East_Return",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_West",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_West_Return",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_South",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_South_Return",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_North",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_North_Return",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_Central",
                    Location = ""
                });
                currentRoutes.Add(new Mappers.Route()
                {
                    Id = counter++,
                    StateCode = city.StateName,
                    CityName = city.Name,
                    Name = city.StateName + "_" + city.Name.ToTitleCase(TitleCase.All).Replace(" ", String.Empty) + "_Central_Return",
                    Location = ""
                });
                routes = ConcatArrays<Mappers.Route>(routes.ToArray(), currentRoutes.ToArray());
            }
            return routes;
        }

        static List<Mappers.CityRequest> GenerateCityResquest(List<Mappers.City> rawData)
        {
            var result = new List<Mappers.CityRequest>();
            foreach (var data in rawData)
            {
                result.Add(new Mappers.CityRequest()
                {
                    State = data.StateName,
                    City = data.Name,
                    Routes = new List<Mappers.RouteLocation>()
                    {
                        new Mappers.RouteLocation()
                        {
                            Pole = "West",
                            StartingPoint = String.Empty,
                            Destination = String.Empty
                        },
                        new Mappers.RouteLocation()
                        {
                            Pole = "East",
                            StartingPoint = String.Empty,
                            Destination = String.Empty
                        },
                        new Mappers.RouteLocation()
                        {
                            Pole = "South",
                            StartingPoint = String.Empty,
                            Destination = String.Empty
                        },
                        new Mappers.RouteLocation()
                        {
                            Pole = "North",
                            StartingPoint = String.Empty,
                            Destination = String.Empty
                        },
                        new Mappers.RouteLocation()
                        {
                            Pole = "Central",
                            StartingPoint = String.Empty,
                            Destination = String.Empty
                        }
                    }
                });
            }
            return result;
        }

        static Dictionary<string, string> GetStateCode()
        {
            var rawData = @"
Alabama: AL,
Alaska: AK,
American Samoa: AS,
Arizona: AZ,
Arkansas: AR,
California: CA,
Colorado: CO,
Connecticut: CT,
Delaware: DE,
District Of Columbia: DC,
Federated States Of Micronesia: FM,
Florida: FL,
Georgia: GA,
Guam: GU,
Hawaii: HI,
Idaho: ID,
Illinois: IL,
Indiana: IN,
Iowa: IA,
Kansas: KS,
Kentucky: KY,
Louisiana: LA,
Maine: ME,
Marshall Islands: MH,
Maryland: MD,
Massachusetts: MA,
Michigan: MI,
Minnesota: MN,
Mississippi: MS,
Missouri: MO,
Montana: MT,
Nebraska: NE,
Nevada: NV,
New Hampshire: NH,
New Jersey: NJ,
New Mexico: NM,
New York: NY,
North Carolina: NC,
North Dakota: ND,
Northern Mariana Islands: MP,
Ohio: OH,
Oklahoma: OK,
Oregon: OR,
Palau: PW,
Pennsylvania: PA,
Puerto Rico: PR,
Rhode Island: RI,
South Carolina: SC,
South Dakota: SD,
Tennessee: TN,
Texas: TX,
Utah: UT,
Vermont: VT,
Virgin Islands: VI,
Virginia: VA,
Washington: WA,
West Virginia: WV,
Wisconsin: WI,
Wyoming: WY";

            var result = new Dictionary<string, string>();
            var test1 = rawData.Split(',');
            foreach (var test in test1)
            {
                var keyValue = test.Split(':');
                string pattern = @"\r\n";
                Regex rgx = new Regex(pattern);
                var data = rgx.Split(keyValue[0]);

                var name = data[1];
                var code = keyValue[1].Split(' ')[1];
                result.Add(name, code);
            }
            return result;
        }

        static List<string> GetLocation(string position, string state = "")
        {
            var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + position + "&sensor=true_or_false&key=AIzaSyAiU7gfCLVXQiCricvn2HHtm8rFtMDcdV4";
            if (state.Length > 0)
                url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + position + "+" + state +
                      "&sensor=true_or_false&key=AIzaSyAiU7gfCLVXQiCricvn2HHtm8rFtMDcdV4";

            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            var test = Regex.Replace(responseFromServer, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            // Display the content.


            var regex = @"\u0022location\u0022:{\u0022lat\u0022:(?:\d*\.)?\d+,\u0022lng\u0022:(?:-?\d*\.)?\d+}";

            var test2 = new Regex(regex);
            var abc = "{" + test2.Match(test);

            var latLongReg = new Regex(@"(?:-?\d*\.)?\d+");

            var latLong = latLongReg.Matches(abc);


            var result = new List<string>();
            foreach (Match match in latLong)
            {
                result.Add(match.Value);
            }

            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            return result;
        }

        static List<T> ConcatArrays<T>(params T[][] list)
        {
            var result = new T[list.Sum(a => a.Length)];
            int offset = 0;
            for (int x = 0; x < list.Length; x++)
            {
                list[x].CopyTo(result, offset);
                offset += list[x].Length;
            }
            return result.ToList();
        }
    }
}
