using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using todoListApp.Dtos;
using todoListApp.Models;
using todoListApp.Services;

namespace todoListApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly PersonService _personService;

        public TasksController(TaskService taskService, PersonService personService)
        {
            _taskService = taskService;
            _personService = personService;
        }

        // POST: api/tasks
        [HttpPost("create")]
        public async Task<ActionResult<TaskTodo>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            // Verificar que el DTO no sea nulo y tenga un título y fecha límite
            if (createTaskDto == null || string.IsNullOrWhiteSpace(createTaskDto.Title) || createTaskDto.DeadLine == default)
            {
                return BadRequest("La tarea debe tener un título y una fecha límite.");
            }

            // Crear una nueva tarea
            var task = new TaskTodo
            {
                Title = createTaskDto.Title,
                DeadLine = createTaskDto.DeadLine
            };

            var createdTask = await _taskService.CreateTaskAsync(task);

            return CreatedAtAction(nameof(CreateTask), new { id = createdTask.TaskId }, createdTask);
        }

        // GET: api/tasks?page=1&limit=10&sort=TaskId&order=desc
        [HttpGet]
        public async Task<ActionResult<List<TaskTodo>>> GetAllTasks(int page = 1, int limit = 10, string sortBy = "TaskId", string order = "asc")
        {
            // Obtener las tareas con paginación y ordenación
            var (tasks, totalCount) = await _taskService.GetAllTasksAsync(page, limit, sortBy, order);

            // Retornar las tareas y la información de paginación
            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Tasks = tasks
            });
        }

        // POST: api/tasks/createPersons
        [HttpPost("createPersons")]
        public async Task<ActionResult> CreatePersonsForTask([FromBody] List<Person> taskPersons)
        {
            if (taskPersons == null || taskPersons.Count == 0)
            {
                return BadRequest("Debe proporcionar al menos una persona para asignar a la tarea.");
            }

            // Crear una lista de objetos Person
            var personsToCreate = new List<Person>();

            // Asignar el TaskId a cada persona
            foreach (var personDto in taskPersons)
            {
                // Verifica que cada persona tenga un TaskId válido
                if (personDto.TaskId <= 0)
                {
                    return BadRequest("El TaskId debe ser válido.");
                }

                // Crear una nueva instancia de Person
                var person = new Person
                {
                    FullName = personDto.FullName,
                    Age = personDto.Age,
                    Skills = personDto.Skills,
                    TaskId = personDto.TaskId
                };

                personsToCreate.Add(person); // Agregar la persona a la lista
            }


            try
            {
                await _personService.CreatePersonsAsync(personsToCreate);
                return Ok(new { message = "Personas creadas exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al crear las personas. Por favor, intente nuevamente." });
            }
        }

        // Endpoint para obtener personas asociadas a una tarea
        [HttpGet("{taskId}/persons")]
        public async Task<ActionResult<List<Person>>> GetPersonsByTaskId(int taskId)
        {
            var persons = await _personService.GetPersonsByTaskIdAsync(taskId);
            if (persons == null || persons.Count == 0)
            {
                return NotFound("No se encontraron personas asociadas a esta tarea.");
            }

            return Ok(persons);
        }
    }
}
