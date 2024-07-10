namespace MyOwnWebsite.Api;

public class MigrationCommand
{
    private string Persistence = "dotnet ef --startup-project ./MyOwnWebsite.Api.csproj migrations add AddProfile --context ApplicationDbContext --output-dir Migrations --project ../MyOwnWebsite.Persistence/MyOwnWebsite.Persistence.csproj";
    private string UpdatePersistence = "dotnet ef --startup-project ./MyOwnWebsite.Api.csproj database update --context ApplicationDbContext --project ../MyOwnWebsite.Persistence/MyOwnWebsite.Persistence.csproj";

    private string Identity = "dotnet ef --startup-project ./MyOwnWebsite.Api.csproj migrations add MyMigration --context ApplicationIdentityDbContext --output-dir Migrations --project ../MyOwnWebsite.Identity/MyOwnWebsite.Identity.csproj";
    private string UpdateIdentity = "dotnet ef --startup-project ./MyOwnWebsite.Api.csproj database update --context ApplicationIdentityDbContext --project ../MyOwnWebsite.Identity/MyOwnWebsite.Identity.csproj";

    private string pass = "duLiQAX1VpBSYOEg";

    private string devCon = "User ID =postgres; Password=postgres;Server=localhost:5432;Database=MyOwnWebsite.Database;TrustServerCertificate=True;Pooling=true";

    private string railwayDb = "PGPASSWORD=NLgyJhxanjBBrTZDhTIOUUYrRxHNQUIE psql -h roundhouse.proxy.rlwy.net -U postgres -p 36285 -d railway";

    private string supaBase = "User Id=postgres.cutuyiichjqjquyiumrb;Password=duLiQAX1VpBSYOEg;Server=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;";

}
