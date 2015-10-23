# publish.ps1
# Publishes NJali NuGet packages.

# https://technet.microsoft.com/en-us/magazine/ff677563.aspx
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true, Position=0)]
    [ValidateSet("LOC", "DEV", "PRD")]
    [Alias("Level")]
    [string]$RepositoryLevel
)

begin
{
    # https://rkeithhill.wordpress.com/2009/08/03/effective-powershell-item-16-dealing-with-errors/
    # http://www.powershellmagazine.com/2011/09/14/custom-errors/
    if ($RepositoryLevel -iin "DEV", "PRD")
    {

        $message = "Level '$RepositoryLevel' is not yet implemented."
        $exception = New-Object NotImplementedException $message
        $errorID = 'NotImplementedFeature'
        $errorCategory = [Management.Automation.ErrorCategory]::NotImplemented
        $target = $RepositoryLevel
        $errorRecord = New-Object Management.Automation.ErrorRecord $exception, $errorID, $errorCategory, $target
        $PSCmdlet.ThrowTerminatingError($errorRecord)
    }

    $packages = @(
        @{Name = "Jali.Core"}
        @{Name = "Jali"}
        @{Name = "Jali.Serve"}
        @{Name = "Jali.Serve.Server"}
        @{Name = "Jali.Serve.AspNet.Mvc"; ClrType = "Net45"}
    )

    $localAppData = [Environment]::GetFolderPath([Environment+SpecialFolder]::LocalApplicationData)

    $workingName = if ($workingName) {$workingName} else {"njali-working"}
    $packageSourceRootName = 'NuGetSources'

    $baseDir = Resolve-Path $PSScriptRoot
    $publishScriptPath = "$baseDir\tools\NuGetPackageSources\057c87c794199d2c24dd5f568513661ed849c06e\NuGetPackageSources.ps1"
    $workingDir = "$baseDir\..\$workingName"
    $packageDropRoot = "$workingDir\$packageSourceRootName"

    $packageSourceRoot = "$localAppData\$packageSourceRootName"

    $packagesData = @()

    foreach ($package in $packages)
    {
        $packageId = $package.Name
        $packageDropPath = "$workingDir\NuGet\$packageId"

        $packageFileSpec = "$packageDropPath\*.nupkg"

        $fileList += @(Get-ChildItem $packageFileSpec -Recurse | 
            Where-Object -Property Name -NotLike "*symbols*")

        foreach ($file in $fileList)
        {
            $packageName = $file.Directory.Name; 
            $version = $file.BaseName.Substring($packageName.Length)
            $packageFilePath = $file.FullName; 
            $symbolsFilePath = $file.DirectoryName + "\" + $file.BaseName + ".symbols.nupkg"

            $packagesData += New-Object -TypeName PSObject -Property @{
                'PackageName' = $packageName
                'Version' = $version
                'PackageFilePath' = $packageFilePath
                'SymbolsFilePath' = $symbolsFilePath
            }
        }
    }

}

process
{
    # http://blogs.technet.com/b/heyscriptingguy/archive/2014/05/30/powershell-best-practices-advanced-functions.aspx
    . $publishScriptPath
    
    try
    {
        if ($RepositoryLevel -ieq "LOC")
        {

            foreach ($packageData in $packagesData)
            {
                # http://blog.nuget.org/20150922/Accelerate-Package-Source.html
                Publish-Package $packageData.PackageFilePath -PackageSource $packageSourceRoot
            }
        }
    }
    finally
    {

    }
}

end
{

}