using FocusList.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusList.DataAccess.Configurations;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDo>
{
  public void Configure(EntityTypeBuilder<ToDo> builder)
  {
    builder.ToTable("ToDos").HasKey(t => t.Id);
    builder.Property(t => t.Id).HasColumnName("ToDoId");
    builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
    builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
    builder.Property(t => t.Title).HasColumnName("Title");
    builder.Property(t => t.Description).HasColumnName("Description");
    builder.Property(t => t.StartDate).HasColumnName("StartDate");
    builder.Property(t => t.EndDate).HasColumnName("EndDate");
    builder.Property(t => t.Priority).HasColumnName("Priority");
    builder.Property(t => t.CategoryId).HasColumnName("Category_Id");
    builder.Property(t => t.UserId).HasColumnName("User_Id");

    builder
      .HasOne(t => t.Category)
      .WithMany(c => c.ToDos)
      .HasForeignKey(t => t.CategoryId)
      .OnDelete(DeleteBehavior.NoAction);
  }
}
