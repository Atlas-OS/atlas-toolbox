function Remove-TempDirectory { Pop-Location; Remove-Item -Path $tempDir -Force -Recurse -EA 0 }
$tempDir = Join-Path -Path $env:TEMP -ChildPath ([guid]::NewGuid().ToString())
New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
Push-Location $tempDir

& curl.exe -LSs "https://github.com/Atlas-OS/atlas-toolbox/releases/latest/download/AtlasToolbox-Setup.exe" -o "$tempDir\toolbox.exe" $timeouts
if (!$?) {
    Write-Error "Downloading Toolbox failed."
    exit 1
}
Write-Output "Installing Toolbox..."
Start-Process -FilePath "$tempDir\toolbox.exe"
exit
