namespace data.hybrid.repository.Configurations
{
    using data.hybrid.repository.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasIndex(i => i.Id).IsUnique();
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
        }
    }
}
