using System;
using System.Collections.Generic;
using todoListApp.Dtos;

namespace todoListApp.Models
{
    public class TaskTodo
    {
        public int TaskId { get; set; }               // ID autoincremental, clave primaria
        public string Title { get; set; }             // Título de la tarea
        public DateTime DeadLine { get; set; }        // Fecha límite de la tarea        
    }
}
