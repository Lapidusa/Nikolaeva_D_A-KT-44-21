using project.Database.Helpers;
using project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace project.Database.Configurations
{
    public class ObjectConfiguration : IEntityTypeConfiguration<Objects>
    {
        private const string TableName = "cd_object";

        public void Configure(EntityTypeBuilder<Objects> builder)
        {
            builder
                .HasKey(p => p.ObjectId)
                .HasName($"pk_{TableName}_object_id");

            builder.Property(p => p.ObjectId)
                .ValueGeneratedOnAdd()
                .HasColumnName("object_id")
                .HasComment("Идентификатор записи предмета");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("c_object_name")
                .HasColumnType("varchar") // Измените на nvarchar, если нужно поддерживать русские символы
                .HasMaxLength(100)
                .HasComment("Название предмета");

            builder.ToTable(TableName);
        }
    }
}