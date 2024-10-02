namespace todoListApp.Dtos
{
    public class CreateTaskDto
    {
        public string Title { get; set; }             // Título de la tarea
        public DateTime DeadLine { get; set; }        // Fecha límite de la tarea
    }
}
