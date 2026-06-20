using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement_CQRS_Pattern.Api.Features.Tasks.Commands.CreateTasks;
using TaskManagement_CQRS_Pattern.Api.Features.Tasks.Commands.DeleteTask;
using TaskManagement_CQRS_Pattern.Api.Features.Tasks.Commands.UpdateTask;
using TaskManagement_CQRS_Pattern.Api.Features.Tasks.Queries.GetAllTasks;
using TaskManagement_CQRS_Pattern.Api.Features.Tasks.Queries.GetTaskById;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskManagement_CQRS_Pattern.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _mediator.Send(new GetAllTasksQuery());
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _mediator.Send(new GetTaskByIdQuery(id));
        if (task is null) return NotFound("No item found");
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        var message = result > 0 ? "Create successful" : "Create failed";
        return Ok(message);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateTask(UpdateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var result = await _mediator.Send(new DeleteTaskCommand(id));
        return Ok(result);
    }
}
