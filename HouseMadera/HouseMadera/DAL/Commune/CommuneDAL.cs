using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseMadera.Modeles;

namespace HouseMadera.DAL
{
    public class CommuneDAL : DAL
    {
        public CommuneDAL(string nomBdd) : base(nomBdd)
        {

        }

        public List<Commune> GetFilteredCommunes(int value)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"@1", value+"%" }
            };
            string sql = @"
                            SELECT Nom_commune,Code_postal 
                            FROM Commune
                            WHERE Code_postal LIKE @1";
            var communes = new List<Commune>();
            var reader = Get(sql, parameters);
            while (reader.Read())
            {
                var commune = new Commune();
                commune.Nom_commune = Convert.ToString(reader["Nom_commune"]);
                commune.Code_postal = Convert.ToInt32(reader["Code_postal"]);
                communes.Add(commune);
            }
            return communes;
        }

    }
}
