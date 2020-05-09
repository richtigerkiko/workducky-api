namespace WorkduckyLib.DataObjects
{
    public class Location
    {
        public string LocationName { get; set; }
        public Coordinate Coordinates { get; set; }
        public int RadiusInMeters { get; set; }

        public bool isInsideLocation(Coordinate coordinate)
        {
            var distance = Geolocation.GeoCalculator.GetDistance(transformFloatCoordinate(Coordinates), transformFloatCoordinate(coordinate));
            if (distance <= RadiusInMeters)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Geolocation.Coordinate transformFloatCoordinate(Coordinate coordinates)
        {
            return new Geolocation.Coordinate()
            {
                Latitude = coordinates.Latitude,
                Longitude = coordinates.Longitude
            };
        }
    }
}