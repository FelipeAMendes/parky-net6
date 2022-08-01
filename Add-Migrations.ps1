param (
    [Parameter(Mandatory=$true)]
    [string] $migrationName
)

$env:DEVELOPMENT_USERNAME_WINDOWS = $env:USERNAME;
$env:DEVELOPMENT_LOGNAME_LINUX = $env:LOGNAME;

set-content env:ConnectionString Server=orderdb;User Id=sa;Password=mendes12345678

dotnet ef migrations add $migrationName -c ApplicationDbContext -p ParkyApi -s ParkyApi
