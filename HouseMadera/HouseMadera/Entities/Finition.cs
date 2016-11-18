using System.ComponentModel.DataAnnotations;

namespace HouseMadera.Entities
{
    public class Finition
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Nom { get; set; }

        public TypeFinition TypeFinition { get; set; }
    }
}