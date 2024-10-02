using todoListApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace todoListApp.Services
{
    public class PersonService
    {
        private readonly TaskManagerContext _context;

        public PersonService(TaskManagerContext context)
        {
            _context = context;
        }

        // Crear un arreglo de personas y devolver las entidades completas
        public async Task<List<Person>> CreatePersonsAsync(List<Person> persons)
        {
            // Validar que la lista de personas no sea nula o vacía
            if (persons == null || persons.Count == 0)
            {
                throw new ArgumentNullException(nameof(persons), "La lista de personas no puede ser nula o vacía.");
            }

            var createdPersons = new List<Person>();

            // Guardar cada persona en la base de datos
            foreach (var person in persons)
            {
                // Validar que cada persona no sea nula
                if (person == null)
                {
                    throw new ArgumentNullException(nameof(person), "Una persona no puede ser nula.");
                }

                // Agregar la persona al contexto
                _context.Person.Add(person);
                await _context.SaveChangesAsync(); // Guardar cada persona
                createdPersons.Add(person); // Agregar la persona creada a la lista
            }

            return createdPersons; // Devolver la lista de entidades completas
        }

        // Método para obtener personas por TaskId usando un procedimiento almacenado
        public async Task<List<Person>> GetPersonsByTaskIdAsync(int taskId)
        {
            var persons = new List<Person>();

            // Definir el comando para el procedimiento almacenado
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetPersonsByTaskId";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Añadir el parámetro
                var taskIdParameter = new SqlParameter("@TaskId", taskId);
                command.Parameters.Add(taskIdParameter);

                // Abrir la conexión y ejecutar el comando
                await _context.Database.OpenConnectionAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var person = new Person
                        {
                            PersonId = reader.GetInt32(0), // Ajusta el índice según tu SELECT
                            FullName = reader.GetString(1),
                            Age = reader.GetInt32(2),
                            Skills = reader.GetString(3)
                        };
                        persons.Add(person);
                    }
                }
            }

            return persons;
        }
    }
}
