namespace HowToGet.Models.Dictionaries
{
    public class CityShortInfo
    {
	    public CityShortInfo()
	    {
		    
	    }

	    public CityShortInfo(City city, bool needToMapState)
	    {
			Id = city.Id;
			Name = city.Name;
			Country = city.CountryName;
		    CountryId = city.CountryId;
			Latitude = city.Latitude;
			Longitude = city.Longitude;
			if(needToMapState)
				StateName = city.StateName;
	    }

	    public CityShortInfo(City city)
			:this(city, true)
	    {
	    }

        public string Id { get; set; }

        public string Name { get; set; }
        
        public string Country { get; set; }
        
		public string CountryId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string StateName { get; set; }
    }
}