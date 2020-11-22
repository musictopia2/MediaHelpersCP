using System;
namespace MediaHelpersCP.Music.TemporaryModels
{
    public class ArtistResult : IComparable<ArtistResult> //c# is more picky about this unfortunately.
    {
        public int ID { get; set; }
        public string ArtistName { get; set; } = "";

        int IComparable<ArtistResult>.CompareTo(ArtistResult other)
        {
            int Temps = ArtistName.CompareTo(other.ArtistName);
            if (Temps != 0)
                return Temps;

            return ID.CompareTo(other.ID);
        }
    }
}