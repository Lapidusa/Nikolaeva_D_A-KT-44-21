using project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace project.Database.Configurations
{
    public class CurriculumConfiguration : IEntityTypeConfiguration<Curriculum>
    {
        private const string TableName = "cd_curriculum";

        public void Configure(EntityTypeBuilder<Curriculum> builder)
        {
            builder
                .HasKey(c => c.CurriculumId)
                .HasName($"pk_{TableName}_curriculum_id");

            builder.Property(c => c.CurriculumId)
                .ValueGeneratedOnAdd()
                .HasColumnName("curriculum_id")
                .HasComment("Идентификатор учебного плана");

            builder.Property(c => c.GroupId)
                .IsRequired()
                .HasColumnName("f_group_id")
                .HasColumnType("int")
                .HasComment("Идентификатор группы");

            builder.Property(c => c.ObjectId)
                .IsRequired()
                .HasColumnName("f_object_id")
                .HasColumnType("int")
                .HasComment("Идентификатор предмета");

            builder.Property(c => c.Hours)
                .IsRequired()
                .HasColumnName("hours")
                .HasColumnType("int")
                .HasComment("Количество часов для учебного плана");
            
            builder.ToTable(TableName);
            
            builder.HasOne(c => c.Group)
                .WithMany()
                .HasForeignKey(c => c.GroupId)
                .OnDelete(DeleteBehavior.Cascade); // Укажите поведение при удалении

            builder.HasOne(c => c.Objects)
                .WithMany()
                .HasForeignKey(c => c.ObjectId)
                .OnDelete(DeleteBehavior.Cascade); // Укажите поведение при удалении
        }
    }
}