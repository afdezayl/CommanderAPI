using System;
using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    private readonly ICommanderRepo _repo;
    private readonly IMapper _mapper;

    public CommandsController(ICommanderRepo repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    // GET api/commands
    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
    {
      var commandItems = _repo.GetAllCommands();

      return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }

    // GET api/commands/{id}
    [HttpGet("{id}", Name = "GetCommandById")]
    public ActionResult<CommandReadDto> GetCommandById(int id)
    {
      var commandItem = _repo.GetCommandById(id);

      if (commandItem is null)
      {
        return NotFound();
      }

      return Ok(_mapper.Map<CommandReadDto>(commandItem));
    }

    // POST api/commands
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto newCommand, ApiVersion version)
    {
      var command = _mapper.Map<Command>(newCommand);
      _repo.CreateCommand(command);
      _repo.SaveChanges();

      var createdCommand = _mapper.Map<CommandReadDto>(command);

      return CreatedAtRoute(
        nameof(GetCommandById),
        new
        {
          id = createdCommand.Id,
          version = version.ToString()
        },
        createdCommand
      );
    }

    // PUT api/commands/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdate)
    {
      var command = _repo.GetCommandById(id);

      if (command is null)
      {
        return NotFound();
      }
      _mapper.Map(commandUpdate, command);

      _repo.UpdateCommand(command);
      _repo.SaveChanges();

      return NoContent();
    }

    // PATCH api/commands/{id}
    [HttpPatch("{id}")]
    public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
    {
      var command = _repo.GetCommandById(id);

      if (command is null)
      {
        return NotFound();
      }

      var commandToPatch = _mapper.Map<CommandUpdateDto>(command);
      patchDoc.ApplyTo(commandToPatch, ModelState);

      if (!TryValidateModel(commandToPatch))
      {
        return ValidationProblem(ModelState);

      }

      _mapper.Map(commandToPatch, command);

      _repo.UpdateCommand(command);
      _repo.SaveChanges();

      return NoContent();
    }

    // DELETE api/commands/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteCommand(int id)
    {
      var command = _repo.GetCommandById(id);

      if (command is null)
      {
        return NotFound();
      }

      _repo.DeleteCommand(command);
      _repo.SaveChanges();
			
      return NoContent();
    }
  }

}