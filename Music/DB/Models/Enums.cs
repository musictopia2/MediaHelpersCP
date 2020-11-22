namespace MediaHelpersCP.Music.ModelInterfaces
{
	public enum EnumSpecialPlayListCategory
	{
		NormalTravelSongsCustom = 1,
		CustomTripNonTravel,
		CustomNormalPlayList,
		WorkPlayLists,
		SavedTrip,
		TemporaryCommuting
	}
	public enum EnumSpecialFormatCategory
	{
		None = 35,
		CountryContemporary = 1, //has to write my own converter
		SwingWestern = 6,
		SwingBigBand = 7,
		Spanish = 8,
		Salsa = 9,
		Standards = 10,
		Oldies = 11,
		Classical = 12,
		Disco = 14,
		Dance = 15,
		Rap = 16,
		Gospel = 17,
		Jazz = 18,
		NewWave = 19,
		Reggae = 20,
		DancePop = 28,
		Urban = 29,
		SwingRevival = 36,
		Bluegrass = 38,
		CountryPop = 39, //i think
		CountryTraditional = 40,
		Folk = 41,
		MexicanPop = 42,
		Remix = 43 //these are all the possible options.
	}
	public enum EnumShowTypeCategory
	{
		None = 44,
		Broadway = 27,
		Disney = 24,
		Games = 37,
		Movie = 26,
		TV = 25,
	}
	public enum EnumSecondaryFormat
	{
		None = 72,
		Acapella = 82,
		Country = 74,
		Jazz = 75,
		Rap = 71,
		Retro = 83,
		Rock = 79,
		Rockabilly = 69,
		Surf = 78,
		Swing = 77,
		Talk = 81,
		Techo = 80,
		ToDo = 76,
		Tropical = 70,
		Unique = 84,
	}
	public enum EnumFormatCategory
	{
		None, Main, Show, Secondary
	}
}