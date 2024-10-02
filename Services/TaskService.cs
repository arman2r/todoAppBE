using todoListApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace todoListApp.Services
{
    public class TaskService
    {
        private readonly TaskManagerContext _context;

        public TaskService(TaskManagerContext context)
        {
            _context = context;
        }

        // Crear una nueva tarea
        public async Task<TaskTodo> CreateTaskAsync(TaskTodo task)
        {
            // Validar que la tarea no sea nula
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "La tarea no puede ser nula.");
            }

            // Agregar la tarea al contexto
            _context.Task.Add(task);
            await _context.SaveChangesAsync(); // Guardar la tarea en la base de datos para obtener el TaskId

            // Recuperar la tarea creada
            var createdTask = await _context.Task             
                .FirstOrDefaultAsync(t => t.TaskId == task.TaskId);

            return createdTask; // Devolver la tarea creada
        }

        // Obtener todas las tareas con paginación y ordenación
        public async Task<(List<TaskTodo> tasks, int totalCount)> GetAllTasksAsync(int page, int limit, string sortBy = "TaskId", string order = "asc")
        {
            // Calcular el total de tareas
            var totalCount = await _context.Task.CountAsync();

            // Aplicar ordenación
            IQueryable<TaskTodo> query = _context.Task;

            if (order.ToLower() == "desc")
            {
                query = query.OrderByDescending(t => EF.Property<object>(t, sortBy));
            }
            else
            {
                query = query.OrderBy(t => EF.Property<object>(t, sortBy));
            }

            // Aplicar paginación
            var tasks = await query
                .Skip((page - 1) * limit) // Saltar las tareas de las páginas anteriores
                .Take(limit) // Tomar las tareas de la página actual
                .ToListAsync();

            return (tasks, totalCount); // Devolver las tareas y el total
        }

    }
}
