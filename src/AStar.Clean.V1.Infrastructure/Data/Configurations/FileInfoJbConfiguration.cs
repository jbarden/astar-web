using AStar.Clean.V1.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AStar.Clean.V1.Infrastructure.Data.Configurations;

public class FileInfoJbConfiguration : IEntityTypeConfiguration<FileDetail>
{
    public void Configure(EntityTypeBuilder<FileDetail> builder)
    {
        builder
            .Property(b => b.FileName)
            .IsRequired();

        builder
            .Property(b => b.DirectoryName)
            .IsRequired();

        builder.HasKey(fileInfoJb => new { fileInfoJb.DirectoryName, fileInfoJb.FileName });
    }
}
