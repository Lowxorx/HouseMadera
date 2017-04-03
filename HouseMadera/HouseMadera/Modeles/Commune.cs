

namespace HouseMadera.Modeles
{
    public class Commune
    {
        public string Code_commune_INSEE { get; set; }
        public string Nom_commune { get; set; }
        public int Code_postal { get; set; }

        public override string ToString()
        {
            return Code_postal + " - " + Nom_commune;
        }

    }
}
