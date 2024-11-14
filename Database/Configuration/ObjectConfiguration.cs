using project.Database.Helpers;
using project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                    .ValueGeneratedOnAdd();

            builder.Property(p => p.ObjectId)
                .HasColumnName("object_id")
                .HasComment("Идентификатор записи предмета");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("c_object_name")
                .HasColumnType(ColumnType.String).HasMaxLength(100)
                .HasComment("Название предмета");

            builder.Property(p => p.GroupId)
                .IsRequired()
                .HasColumnName("f_group_id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Идентификатор группы");

            builder.ToTable(TableName)
                .HasOne(p => p.Group)
                .WithMany()
                .HasForeignKey(p => p.GroupId)
                .HasConstraintName("fk_f_group_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(TableName)
                .HasIndex(p => p.GroupId, $"idx_{TableName}_fk_f_group_id");

            builder.Navigation(p => p.Group)
                .AutoInclude();
        }
    }
}
