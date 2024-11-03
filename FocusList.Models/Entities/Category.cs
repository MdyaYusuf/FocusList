using Core.Entities;

namespace FocusList.Models.Entities;

public class Category : Entity<int>
{
  public Category()
  {

  }

  public string Name { get; set; } = default!;
  public List<ToDo>? ToDos { get; set; }
}
