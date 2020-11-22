using CommonBasicStandardLibraries.Exceptions;
using MediaHelpersCP.Music.ModelInterfaces;

namespace MediaHelpersCP.Music.DB.Helpers
{
    public static class SongTableExtension
    {
        public static EnumSecondaryFormat GetSecondaryFormat(this string secondaryString)
        {
            switch (secondaryString)
            {
                case "todo":
                case "ToDo":
                    return EnumSecondaryFormat.ToDo;
                case "":
                case "None":
                    return EnumSecondaryFormat.None;
                case "Acapella":
                    return EnumSecondaryFormat.Acapella;
                case "Country":
                    return EnumSecondaryFormat.Country;
                case "Jazz":
                    return EnumSecondaryFormat.Jazz;
                case "Rap":
                    return EnumSecondaryFormat.Rap;
                case "Retro":
                    return EnumSecondaryFormat.Retro;
                case "Rock":
                    return EnumSecondaryFormat.Rock;
                case "Rockabilly":
                    return EnumSecondaryFormat.Rockabilly;
                case "Surf":
                    return EnumSecondaryFormat.Surf;
                case "Swing":
                    return EnumSecondaryFormat.Swing;
                case "Talk":
                    return EnumSecondaryFormat.Talk;
                case "Techo":
                    return EnumSecondaryFormat.Techo;
                case "Tropical":
                    return EnumSecondaryFormat.Tropical;
                case "Unique":
                    return EnumSecondaryFormat.Unique;
                default:
                    return EnumSecondaryFormat.None;
            }
        }
        public static EnumShowTypeCategory GetShowEnumCategory(this string stringShow)
        {
            switch (stringShow)
            {
                case "":
                    return EnumShowTypeCategory.None;
                case "Broadway":
                    return EnumShowTypeCategory.Broadway;
                case "Disney":
                    return EnumShowTypeCategory.Disney;
                case "Games":
                    return EnumShowTypeCategory.Games;
                case "Movie":
                    return EnumShowTypeCategory.Movie;
                case "TV":
                    return EnumShowTypeCategory.TV;
                case "New Wave": //because there could have been some with the mistake.  it was intended to be none
                case "None":
                    return EnumShowTypeCategory.None;
                default:
                    return EnumShowTypeCategory.None;
            }
        }
        public static EnumSpecialFormatCategory GetSpecialEnumCategory(this string stringSpecial)
        {
            switch (stringSpecial)
            {
                case "new wave":
                case "New Wave":
                    return EnumSpecialFormatCategory.NewWave;
                case "Bluegrass":
                    return EnumSpecialFormatCategory.Bluegrass;
                case "Classical":
                    return EnumSpecialFormatCategory.Classical;
                case "Country/Contemporary":
                    return EnumSpecialFormatCategory.CountryContemporary;
                case "Country/Pop":
                    return EnumSpecialFormatCategory.CountryPop;
                case "Country/Traditional":
                    return EnumSpecialFormatCategory.CountryTraditional;
                case "Dance":
                    return EnumSpecialFormatCategory.Dance;
                case "Dance Pop":
                    return EnumSpecialFormatCategory.DancePop;
                case "Disco":
                    return EnumSpecialFormatCategory.Disco;
                case "Folk":
                    return EnumSpecialFormatCategory.Folk;
                case "Gospel":
                    return EnumSpecialFormatCategory.Gospel;
                case "Jazz":
                    return EnumSpecialFormatCategory.Jazz;
                case "Mexican Pop":
                    return EnumSpecialFormatCategory.MexicanPop;
                case "":
                case "None":
                    return EnumSpecialFormatCategory.None;
                case "Oldies":
                    return EnumSpecialFormatCategory.Oldies;
                case "Rap":
                    return EnumSpecialFormatCategory.Rap;
                case "Reggae":
                    return EnumSpecialFormatCategory.Reggae;
                case "Remix":
                    return EnumSpecialFormatCategory.Remix;
                case "Salsa":
                    return EnumSpecialFormatCategory.Salsa;
                case "Spanish":
                    return EnumSpecialFormatCategory.Spanish;
                case "Standards":
                    return EnumSpecialFormatCategory.Standards;
                case "Swing/Big Band":
                    return EnumSpecialFormatCategory.SwingBigBand;
                case "Swing/Revival":
                    return EnumSpecialFormatCategory.SwingRevival;
                case "Swing/Western":
                    return EnumSpecialFormatCategory.SwingWestern;
                case "Urban":
                    return EnumSpecialFormatCategory.Urban;
                default:
                    return EnumSpecialFormatCategory.None;
            }
        }
        public static string GetSpecialString(this EnumSpecialFormatCategory category)
        {
            return category switch
            {
                EnumSpecialFormatCategory.None => "None",
                EnumSpecialFormatCategory.CountryContemporary => "Country/Contemporary",
                EnumSpecialFormatCategory.SwingWestern => "Swing/Western",
                EnumSpecialFormatCategory.SwingBigBand => "Swing/Big Band",
                EnumSpecialFormatCategory.Spanish => "Spanish",
                EnumSpecialFormatCategory.Salsa => "Salsa",
                EnumSpecialFormatCategory.Standards => "Standards",
                EnumSpecialFormatCategory.Oldies => "Oldies",
                EnumSpecialFormatCategory.Classical => "Classical",
                EnumSpecialFormatCategory.Disco => "Disco",
                EnumSpecialFormatCategory.Dance => "Dance",
                EnumSpecialFormatCategory.Rap => "Rap",
                EnumSpecialFormatCategory.Gospel => "Gospel",
                EnumSpecialFormatCategory.Jazz => "Jazz",
                EnumSpecialFormatCategory.NewWave => "new wave",
                EnumSpecialFormatCategory.Reggae => "Reggae",
                EnumSpecialFormatCategory.DancePop => "Dance Pop",
                EnumSpecialFormatCategory.Urban => "Urban",
                EnumSpecialFormatCategory.SwingRevival => "Swing/Revival",
                EnumSpecialFormatCategory.Bluegrass => "Bluegrass",
                EnumSpecialFormatCategory.CountryPop => "Country/Pop",
                EnumSpecialFormatCategory.CountryTraditional => "Country/Traditional",
                EnumSpecialFormatCategory.Folk => "Folk",
                EnumSpecialFormatCategory.MexicanPop => "Mexican Pop",
                EnumSpecialFormatCategory.Remix => "Remix",
                _ => throw new BasicBlankException("Category Not Supported"),
            };
        }
    }
}