using Microsoft.AspNetCore.Mvc;
using TodoApi.Controllers;
using TodoApi.Models;

namespace TodoApi.Tests;

public class TasksControllerTests
{
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _controller = new TasksController();
    }

    // ─── GET ALL ────────────────────────────────────────────────
    [Fact]
    public void GetAll_ShouldReturnOkResult()
    {
        var result = _controller.GetAll();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetAll_ShouldReturnListOfTasks()
    {
        var result = _controller.GetAll().Result as OkObjectResult;
        var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(result!.Value);
        Assert.NotEmpty(tasks);
    }

    // ─── GET BY ID ──────────────────────────────────────────────
    [Fact]
    public void GetById_ExistingId_ShouldReturnOkResult()
    {
        var result = _controller.GetById(1);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetById_NonExistingId_ShouldReturnNotFound()
    {
        var result = _controller.GetById(999);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectTask()
    {
        var result = _controller.GetById(1).Result as OkObjectResult;
        var task = Assert.IsType<TaskItem>(result!.Value);
        Assert.Equal(1, task.Id);
    }

    // ─── GET PENDING ─────────────────────────────────────────────
    [Fact]
    public void GetPending_ShouldReturnOnlyIncompleteTasks()
    {
        var result = _controller.GetPending().Result as OkObjectResult;
        var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(result!.Value);
        Assert.All(tasks, t => Assert.False(t.IsCompleted));
    }

    // ─── CREATE ──────────────────────────────────────────────────
    [Fact]
    public void Create_ValidTask_ShouldReturnCreatedResult()
    {
        var newTask = new TaskItem
        {
            Title = "Nueva tarea de prueba",
            Description = "Descripción de prueba",
            Priority = "High"
        };

        var result = _controller.Create(newTask);
        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public void Create_EmptyTitle_ShouldReturnBadRequest()
    {
        var invalidTask = new TaskItem { Title = "" };
        var result = _controller.Create(invalidTask);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void Create_ShouldAssignNewId()
    {
        var newTask = new TaskItem { Title = "Tarea con ID asignado", Priority = "Low" };
        var result = _controller.Create(newTask).Result as CreatedAtActionResult;
        var createdTask = Assert.IsType<TaskItem>(result!.Value);
        Assert.True(createdTask.Id > 0);
    }

    // ─── COMPLETE ────────────────────────────────────────────────
    [Fact]
    public void Complete_ExistingId_ShouldMarkAsCompleted()
    {
        // Crear una tarea fresca para este test
        var newTask = new TaskItem { Title = "Tarea a completar", Priority = "Medium" };
        var created = (_controller.Create(newTask).Result as CreatedAtActionResult)!.Value as TaskItem;

        var result = _controller.Complete(created!.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void Complete_NonExistingId_ShouldReturnNotFound()
    {
        var result = _controller.Complete(9999);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    // ─── DELETE ──────────────────────────────────────────────────
    [Fact]
    public void Delete_ExistingId_ShouldReturnNoContent()
    {
        // Crear una tarea para luego eliminarla
        var newTask = new TaskItem { Title = "Tarea a eliminar", Priority = "Low" };
        var created = (_controller.Create(newTask).Result as CreatedAtActionResult)!.Value as TaskItem;

        var result = _controller.Delete(created!.Id);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_NonExistingId_ShouldReturnNotFound()
    {
        var result = _controller.Delete(9999);
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
