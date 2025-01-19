using adsmap.EntityFrameworkModel;
using Microsoft.EntityFrameworkCore;

namespace adsmap.DataAccessLayer
{
    public partial class ScadaDbContext : DbContext
    {

        public ScadaDbContext(DbContextOptions<ScadaDbContext> options) : base(options)
        {

        }
        public DbSet<UnitItem> UnitItems { get; set; }
        public DbSet<TerminalComponent> TerminalComponents { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActiveMode> ActiveModes { get; set; }
        public DbSet<TerminalItem> TerminalItems { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<AccountTerminal> AccountTerminals { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Account> Accounts { get; set; }

    }
}
