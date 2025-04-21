param (
    [string]$ConfigurationName,
    [string]$TargetPath,
    [string]$TargetName,
    [string]$SolutionDir
)

# Define the base directory
$TargetDir = "${SolutionDir}bin\upload"

# Function to log messages to the console
function Log {
    param (
        [string]$message
    )
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    Write-Host "$timestamp - $message"
}

Log "Script started"
Log "ConfigurationName: $ConfigurationName"
Log "TargetPath: $TargetPath"
Log "TargetName: $TargetName"
Log "TargetDir: $TargetDir"

# Get the assembly version
$assembly = [System.Reflection.Assembly]::LoadFile($TargetPath)
$version = $assembly.GetName().Version.ToString()
Log "Assembly version: $version"

# Determine the directory of the deployed DLL
$deployDir = Split-Path -Parent $TargetPath
Log "DeployDir: $deployDir"

# Check if the deploy directory is the base directory or one level further
$zipPath = "$TargetDir\$TargetName-v$version.zip"
Log "ZipPath: $zipPath"

# Remove existing zip file if it exists
if (Test-Path $zipPath) {
    Log "ZipPath exists, removing"
    Remove-Item $zipPath -Force
}

# Create the temp directory structure
$tempZipDir = "$TargetDir\tempZip"
if (Test-Path $tempZipDir) {
    Log "TempZipDir exists, removing"
    Remove-Item $tempZipDir -Recurse -Force
}

New-Item -ItemType Directory -Path $tempZipDir
Log "TempZipDir created: $tempZipDir"

# Create the required folder structure within the temp directory
$bepinexPluginsDir = Join-Path $tempZipDir "BepInEx\plugins"
New-Item -ItemType Directory -Path $bepinexPluginsDir -Force
Log "BepInEx\plugins directory created: $bepinexPluginsDir"

# Copy the single DLL to the new structure
Copy-Item -Path $TargetPath -Destination $bepinexPluginsDir -Force
Log "DLL copied to BepInEx\plugins directory"

# Create the final zip file
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($tempZipDir, $zipPath)
Log "Final zip file created: $zipPath"

# Clean up temp directory
Remove-Item $tempZipDir -Recurse -Force
Log "TempZipDir removed"

Log "Script finished"
