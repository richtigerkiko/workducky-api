using System.Collections.Generic;
using NodaTime;

namespace WorkduckyLib.DataObjects.DummyData
{
    public static class GenerateDummySettings
    {
        public static Settings DummySettings()
        {
            var settings = new Settings();

            settings.Locations = new List<Location>()
            {
                new Location()
                {
                    LocationName = "Kölner Dom",
                    Coordinates = new Coordinate() {
                        Latitude = 50.9418656F,
                        Longitude = 6.9575751F
                    },
                    RadiusInMeters = 50
                },
                new Location ()  {
                    LocationName =  "Kotlett beim Lommi!",
                    Coordinates = new Coordinate(){
                        Latitude = 50.9385819F,
                        Longitude = 6.9715186F
                    },
                    RadiusInMeters = 50
                }
            };
            settings.WorkdayDefaults = new WorkdayDefaults()
            {
                breakMinutes = 30,
                defaultStartTime = new LocalTime(09, 00),
                defaultEndTime = new LocalTime(17, 30),
                DefaultWorkDays = new DefaultWorkDays()
                {
                    Monday = true,
                    Tuesday = true,
                    Wednesday = true,
                    Thursday = true,
                    Friday = true
                }
            };

            return settings;
        }
    }
}