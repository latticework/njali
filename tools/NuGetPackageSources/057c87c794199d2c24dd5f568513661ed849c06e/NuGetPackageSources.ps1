# Copied from https://gist.githubusercontent.com/AArnott/45d884642684046c81cb/raw/057c87c794199d2c24dd5f568513661ed849c06e/NuGetPackageSources.ps1
# Per http://blog.nuget.org/20150922/Accelerate-Package-Source.html

Function Create-NuPkgSha512File {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,Position=1)]
        [string]$Path,
        [Parameter()]
        [string]$Sha512Path="$Path.sha512"
    )

    $sha512 = [Security.Cryptography.SHA512]::Create()
    $fileBytes = [IO.File]::ReadAllBytes($Path)
    $hashBytes = $sha512.ComputeHash($fileBytes)
    $hashBase64 = [convert]::ToBase64String($hashBytes)
    Set-Content -Path $Sha512Path -Value $hashBase64
}

Function Publish-Package {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,Position=1)]
        [string]$Path,
        [Parameter(Mandatory=$true,Position=2)]
        [string]$PackageSource
    )

    $nupkgLeafName = Split-Path $Path -Leaf

    $shell = New-Object -ComObject Shell.Application
    $TempFolder = Join-Path -Path ([IO.Path]::GetTempPath()) -ChildPath ([IO.Path]::GetRandomFileName())

    $null = mkdir $TempFolder
    $tempZipPath = "$TempFolder\$nupkgLeafName.zip"
    Copy-Item -Path $Path -Destination $tempZipPath
    $zipItems = $shell.NameSpace($tempZipPath).Items()
    $nuSpecItem = @($zipItems |? { $_.Path -like '*.nuspec' })[0]
    $tempNuSpecPath = "$tempFolder\$([IO.Path]::GetFileName($nuspecItem.Path))"
    $Shell.NameSpace($tempFolder).CopyHere($nuspecItem)
    $nuspec = [xml](Get-Content $tempNuSpecPath)

    $id = $nuspec.package.metadata.id
    $version = $nuspec.package.metadata.version
        
    If (-not (Test-Path "$PackageSource\$id\$version")) { $null = mkdir "$PackageSource\$id\$version" }
    Copy-Item -Path $tempZipPath -Destination "$PackageSource\$id\$version\$nupkgLeafName"
    Create-NuPkgSha512File -Path $tempZipPath -Sha512Path "$PackageSource\$id\$version\$nupkgLeafName.sha512"
    Copy-Item -Path $tempNuSpecPath -Destination "$PackageSource\$id\$version\$id.nuspec"

    Remove-Item $TempFolder -Recurse -Force
}

Function Upgrade-PackageSource {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true,Position=1)]
        [string]$NuGet2Source,
        [Parameter(Mandatory=$true,Position=2)]
        [string]$NuGet3Source
    )

    # Exclude symbols packages. NuGet clients don't use them from package sources anyway.
    Get-ChildItem "$NuGet2Source\*.nupkg" -Exclude *.symbols.nupkg |% {
        Publish-Package -Path $_ -PackageSource $NuGet3Source
        Write-Host "Published $($_.Name)"
    }
}