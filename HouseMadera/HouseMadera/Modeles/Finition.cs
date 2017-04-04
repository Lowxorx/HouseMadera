
namespace HouseMadera.Modeles
{

    /// <summary>
    /// Classe représentant les Finitions
    /// </summary>
    public class Finition
    {
        /// <summary>
        /// Id de la finission
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la finission
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Type de la finission
        /// </summary>
        public TypeFinition TypeFinition { get; set; }
    }
}