using System;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Dates;
namespace MediaHelpersCP.Video.TelevisionMiscClasses
{
	public enum EnumTelevisionLengthType
	{
		HalfHour = 1,
		FullHour,
		ShortCartoon,
		NormalCartoon
	}
	public enum EnumTelevisionHoliday
	{
		None,
		Christmas,
		Thanksgiving,
		ValentinesDay,
		Halloween
	}
	public enum EnumTelevisionCategory
	{
		None,
		FirstRun,
		Reruns
	}
	public static class HolidayExtensions
	{
		public static EnumTelevisionHoliday WhichHoliday(this DateTime whichDate)
		{
			whichDate = whichDate.Date; //taking a risk here.
										//whichDate = new DateTime(whichDate.Year, whichDate.Month, whichDate.Day);
			if (whichDate.Month == 10 && whichDate.Day >= 29)
			{
				return EnumTelevisionHoliday.Halloween;
			}

			if (whichDate.Month == 11)
			{
				DateTime tempDate = DateTime.Now.WhenIsThanksgivingThisYear();
				int Day = tempDate.Day - 7;
				if (whichDate.Day >= Day && whichDate <= tempDate)
				{
					return EnumTelevisionHoliday.Thanksgiving;
				}

				if (whichDate > tempDate)
				{
					return EnumTelevisionHoliday.Christmas;
				}

				return EnumTelevisionHoliday.None;
			}
			if (whichDate.Month == 12 && whichDate.Day <= 25)
			{
				return EnumTelevisionHoliday.Christmas;
			}

			if (whichDate.Month == 2 && whichDate.Day <= 14 && whichDate.Day >= 8)
			{
				return EnumTelevisionHoliday.ValentinesDay;
			}

			return EnumTelevisionHoliday.None;
		}
	}
}