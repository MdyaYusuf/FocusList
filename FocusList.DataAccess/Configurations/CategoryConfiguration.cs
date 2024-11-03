using FocusList.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FocusList.DataAccess.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
  public void Configure(EntityTypeBuilder<Category> builder)
  {
    builder.ToTable("Categories").HasKey(c => c.Id);
    builder.Property(c => c.Id).HasColumnName("CategoryId");
    builder.Property(c => c.CreatedDate).HasColumnName("CreatedDate");
    builder.Property(c => c.UpdatedDate).HasColumnName("UpdatedDate");
    builder.Property(c => c.Name).HasColumnName("CategoryName");

    builder
      .HasMany(c => c.ToDos)
      .WithOne(t => t.Category)
      .HasForeignKey(c => c.CategoryId)
      .OnDelete(DeleteBehavior.NoAction);

    builder.HasData(new Category()
    {
      Id = 1,
      Name = "Temizlik",
      CreatedDate = DateTime.Now
    },
    new Category()
    {
      Id = 2,
      Name = "Yemek",
      CreatedDate = DateTime.Now
    },
    new Category()
    {
      Id = 3,
      Name = "Spor",
      CreatedDate = DateTime.Now
    },
    new Category()
    {
      Id = 4,
      Name = "Seyahat",
      CreatedDate = DateTime.Now
    },
    new Category()
    {
      Id = 5,
      Name = "İş",
      CreatedDate = DateTime.Now
    });
  }
}
