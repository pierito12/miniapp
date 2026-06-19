using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> _tasks = new()
    {
        new TaskItem { Id = 1, Title = "Diseñar base de datos", Description = "Crear el modelo entidad-relación del sistema", Priority = "High" },
        new TaskItem { Id = 2, Title = "Implementar autenticación JWT", Description = "Configurar Spring Security con tokens JWT", Priority = "High" },
        new TaskItem { Id = 3, Title = "Desarrollar módulo de pagos", Description = "Integrar Stripe para pagos de membresías", Priority = "Medium" },
        new TaskItem { Id = 4, Title = "Crear panel de administración", Description = "Dashboard para gestión de miembros y clases", Priority = "Medium" },
        new TaskItem { Id = 5, Title = "Pruebas de integración", Description = "Ejecutar pruebas end-to-end del sistema", Priority = "Low" }
    };

    private static int _nextId = 6;

    // GET api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll()
    {
        return Ok(_tasks);
    }

    // GET api/tasks/5
    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return NotFound(new { message = $"Tarea con ID {id} no encontrada." });
        return Ok(task);
    }

    // GET api/tasks/pending
    [HttpGet("pending")]
    public ActionResult<IEnumerable<TaskItem>> GetPending()
    {
        var pending = _tasks.Where(t => !t.IsCompleted);
        return Ok(pending);
    }

    // POST api/tasks
    [HttpPost]
    public ActionResult<TaskItem> Create([FromBody] TaskItem newTask)
    {
        if (string.IsNullOrWhiteSpace(newTask.Title))
            return BadRequest(new { message = "El título es obligatorio." });

        newTask.Id = _nextId++;
        newTask.CreatedAt = DateTime.UtcNow;
        _tasks.Add(newTask);

        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
    }

    // PUT api/tasks/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] TaskItem updatedTask)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return NotFound(new { message = $"Tarea con ID {id} no encontrada." });

        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.IsCompleted = updatedTask.IsCompleted;
        task.Priority = updatedTask.Priority;

        return NoContent();
    }

    // PATCH api/tasks/5/complete
    [HttpPatch("{id}/complete")]
    public IActionResult Complete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return NotFound(new { message = $"Tarea con ID {id} no encontrada." });

        task.IsCompleted = true;
        return Ok(task);
    }

    // DELETE api/tasks/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) return NotFound(new { message = $"Tarea con ID {id} no encontrada." });

        _tasks.Remove(task);
        return NoContent();
    }
}
