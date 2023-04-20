using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options) { }
    
    public DbSet<PersonalInfo>? PersonalInfos { get; set; }
    public DbSet<ProgressBar>? ProgressBars { get; set; }
    public DbSet<CertificacaoItem>? CertificacaoItems { get; set; }
    public DbSet<PortifolioItem>? PortifolioItems { get; set; }
    public DbSet<TimeLineItem>? TimeLineItems { get; set; }
    public DbSet<AboutItem>? AboutItems { get; set; }
    public DbSet<ContactItem>? ContactItems { get; set; }
    public DbSet<SocialItem>? SocialItems { get; set; }
}
