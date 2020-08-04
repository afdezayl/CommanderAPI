using Commander.Models;
using System.Collections.Generic;

namespace Commander.Data
{
  public class MockCommanderRepo : ICommanderRepo
  {
    public void CreateCommand(Command command)
    {
      throw new System.NotImplementedException();
    }

    public void DeleteCommand(Command command)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<Command> GetAllCommands()
    {
      var commands = new List<Command>{
        new Command { Id = 0, HowTo = "Boil an egg", Line = "Boil Water", Platform = "Kettle & Pan" },
        new Command { Id = 1, HowTo = "Cut bread", Line = "Get a knife", Platform = "Knife & chooping board" },
        new Command { Id = 2, HowTo = "Make cup of tea", Line = "Place teabag in cup", Platform = "Kettle & Cupn" },
      };

      return commands;
    }

    public Command GetCommandById(int id)
    {
      return new Command
      {
        Id = id,
        HowTo = "Boil an egg",
        Line = "Boil Water",
        Platform = "Kettle & Pan"
      };
    }

    public bool SaveChanges()
    {
      throw new System.NotImplementedException();
    }

    public void UpdateCommand(Command command)
    {
      throw new System.NotImplementedException();
    }
  }
}