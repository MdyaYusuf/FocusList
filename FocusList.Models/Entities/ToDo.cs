using Core.Entities;
using FocusList.Models.Enums;

namespace FocusList.Models.Entities;

public class ToDo : Entity<Guid>
{
  public ToDo()
  {
    IsCompleted = false;    
  }

  public string Title { get; set; } = default!;
  public string Description { get; set; } = default!;
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public Priority Priority { get; set; }
  public bool IsCompleted { get; set; }
  public int CategoryId { get; set; }
  public Category Category { get; set; } = null!;
  public string UserId { get; set; } = default!;
  public User User { get; set; } = null!;
}
