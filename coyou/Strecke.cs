namespace coyou
{
    public class Strecke
    {
        public string Startpunkt { get; set; }
        public string Endpunkt { get; set; }
        public string Fortbewegungsmittel { get; set; }

        public Strecke(string startpunkt, string endpunkt, string fortbewegungsmittel)
        {
            Startpunkt = startpunkt;
            Endpunkt = endpunkt;
            Fortbewegungsmittel = fortbewegungsmittel;
        }

    }
}
