using HouseMadera.Modeles;
using HouseMadera.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public class SlotPlaceDAL : DAL, IDAL<SlotPlace>
    {
        public SlotPlaceDAL(string nomBdd) : base(nomBdd)
        {
        }

        #region SYNCHRONISATION
        public int DeleteModele(SlotPlace modele)
        {
            {
                string sql = @"
                        UPDATE SlotPlace
                        SET Suppression= @2
                        WHERE Id=@1
                      ";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                {"@1",modele.Id},
                {"@2",DateTimeDbAdaptor.FormatDateTime(modele.Suppression,Bdd)}

            };
                int result = 0;
                try
                {
                    result = Update(sql, parameters);
                }
                catch (Exception e)
                {
                    result = -1;
                    Console.WriteLine(e.Message);
                    //Logger.WriteEx(e);
                }

                return result;
            }
        }

        public List<SlotPlace> GetAllModeles()
        {
            List<SlotPlace> listeSlotsPlaces = new List<SlotPlace>();
            try
            {

                string sql = @"SELECT sp.*,m.Id AS module_Id , m.Nom AS module_Nom , s.Id AS slot_Id ,s.Nom AS slot_Nom,tmp.Id AS typeModulePlacable_Id,tmp.Nom AS typeModulePlacable_Nom
                               FROM SlotPlace sp
                               LEFT JOIN Module m ON sp.Module_Id = m.Id
                               LEFT JOIN Slot s ON sp.Slot_Id = s.Id
                               LEFT JOIN TypeModulePlacable tmp ON sp.TypeModulePlacable_Id = tmp.Id";

                using (DbDataReader reader = Get(sql, null))
                {
                    while (reader.Read())
                    {
                        SlotPlace sp = new SlotPlace()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Libelle = Convert.ToString(reader["Libelle"]),
                            MiseAJour = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["MiseAJour"])),
                            Suppression = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Suppression"])),
                            Creation = DateTimeDbAdaptor.InitialiserDate(Convert.ToString(reader["Creation"])),
                            Module = new Module()
                            {
                                Id = Convert.ToInt32(reader["module_Id"]),
                                Nom = Convert.ToString(reader["module_Nom"]),
                            },
                            Slot = new Slot()
                            {
                                Id = Convert.ToInt32(reader["slot_Id"]),
                                Nom = Convert.ToString(reader["slot_Nom"])
                            },
                            TypeModulePlacable = new TypeModulePlacable()
                            {
                                Id = Convert.ToInt32(reader["typeModulePlacable_Id"]),
                                Nom = Convert.ToString(reader["typeModulePlacable_Nom"]),
                            }
                        };
                        listeSlotsPlaces.Add(sp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.WriteEx(ex);
            }

            return listeSlotsPlaces;
        }

        public int InsertModele(SlotPlace modele)
        {
            throw new NotImplementedException();
        }

        public int UpdateModele(SlotPlace modele1, SlotPlace modele2)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
