# -------------------------
# Configurable Variables
# -------------------------
$server = ""               # SQL Server name
$database = ""           # Database name
$outputDir = ""  # Git repo path
$gitBranch = ""                # Branch to commit changes to

# -------------------------
# Ensure output directory exists
# -------------------------
if (!(Test-Path $outputDir))
{
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}

# -------------------------
# Query all stored procedures (and functions/views if needed)
# -------------------------
$query = @"
SET NOCOUNT ON;
SELECT 
    o.name AS ObjectName,
    o.type_desc AS ObjectType,
    sm.definition AS ObjectDefinition
FROM sys.sql_modules sm
JOIN sys.objects o ON sm.object_id = o.object_id
WHERE o.type IN ('P', 'FN', 'TF', 'IF', 'V')
ORDER BY o.type, o.name;
"@

# -------------------------
# Run SQL with credentials
# -------------------------
$results = Invoke-Sqlcmd -ServerInstance $server -Database $database -Query $query

foreach ($row in $results)
{
    $safeName = ($row.ObjectName -replace '[^a-zA-Z0-9_]', '_')
    $fileName = "$safeName.sql"
    $filePath = Join-Path $outputDir $fileName

    # Add a header for clarity
    $content = @"
-- =============================================
-- Object: $( $row.ObjectName )  ($( $row.ObjectType ))
-- Generated: $( Get-Date -Format "yyyy-MM-dd HH:mm:ss" )
-- =============================================
$( $row.ObjectDefinition )
"@

    # Save to file
    $content | Out-File -FilePath $filePath -Encoding UTF8
}

# -------------------------
# Git commit and push
# -------------------------
Set-Location $outputDir
git add .
if (-not (git diff --cached --quiet))
{
    git commit -m "Automated SP export $( Get-Date -Format "yyyy-MM-dd HH:mm:ss" )"
    git push origin $gitBranch
}
else
{
    Write-Output "No changes detected, nothing to commit."
}
