namespace Core.Entities;

public class Entity<TId>
{
  protected Entity()
  {
    CreatedDate = DateTime.Now;
  }
  protected Entity(TId id) : this()
  {
    Id = id;
  }

  public TId Id { get; set; }
  public DateTime CreatedDate { get; set; }
  public DateTime? UpdatedDate { get; set; }
}
