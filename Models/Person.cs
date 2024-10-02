namespace todoListApp.Models
{
    public class Person
    {
        public int PersonId { get; set; }          // ID autoincremental
        public string FullName { get; set; }       // Nombre completo
        public int Age { get; set; }               // Edad
        public string Skills { get; set; }         // Skills como string (lista separada por comas)

        public int? TaskId { get; set; }
    }
}
