# Copied from https://github.com/JamesNK/Jali/blob/master/Build/build.ps1
properties { 
  $zipFileName = "Jali00r1.zip"
  $majorVersion = "0.0"
  $majorWithReleaseVersion = "0.0.1"
  $nugetPrelease = "prealpha1"
  $version = GetVersion $majorWithReleaseVersion
  $signAssemblies = $false
#  $signKeyPath = "C:\Development\Releases\newtonsoft.snk"
  $buildDocumentation = $false
  $buildNuGet = $true
  $treatWarningsAsErrors = $false
  $workingName = if ($workingName) {$workingName} else {"working"}
#  $dnvmVersion = "1.0.0-beta8-15618"
  $dnvmVersion = "1.0.0-beta7"
  
  $baseDir  = Resolve-Path $PSScriptRoot
  $buildDir = "$baseDir\build"
  $sourceDir = "$baseDir\src"
  $toolsDir = "$baseDir\tools"
#  $docDir = "$baseDir\doc"
  $releaseDir = "$baseDir\dist"
  $workingDir = "$baseDir\..\$workingName"
  $workingSourceDir = "$workingDir\src"
  $builds = @(
    @{Name = "Jali.Dnx"; ClrType = "Dnx"; SeparateProjectFolder = $false; TestsName = "Jali.Tests.Dnx"; BuildFunction = "DnxBuild"; TestsFunction = "DnxTests"; Constants="DNX"; FinalDir="Dnx"; NuGetDir = "dnx"; Framework=$null},
    @{Name = "Jali.Pcl"; ClrType = "Pcl"; SeparateProjectFolder = $true; TestsName = "Jali.Tests.Pcl"; BuildFunction = "MSBuildBuild"; TestsFunction = "XUnitTests"; Constants="PCL"; FinalDir="Pcl"; NuGetDir = "portable-net40+sl5+wp80+win8+wpa81"; Framework="net-4.0"}
  )
  $packages = @(
    @{Name = "Jali.Core"}
    @{Name = "Jali"}
  )
}

framework '4.6x86'

#task default -depends Test
task default -depends Package

# Ensure a clean working directory
task Clean {
  Write-Host "Setting location to $baseDir"
  Set-Location $baseDir
  
  if (Test-Path -path $workingDir)
  {
    Write-Host "Deleting existing working directory $workingDir"
    
    Execute-Command -command { del $workingDir -Recurse -Force }
  }
  
  Write-Host "Creating working directory $workingDir"
  New-Item -Path $workingDir -ItemType Directory
}

# Build each solution, optionally signed
task Build -depends Clean { 

  Write-Host "Copying source to working source directory $workingSourceDir"
#  robocopy $sourceDir $workingSourceDir /MIR /NP /XD bin obj TestResults AppPackages $packageDirs /XF *.suo *.user *.project.lock.json | Out-Default
  robocopy $baseDir $workingDir /MIR /NP /XD bin obj TestResults AppPackages $packageDirs /XF *.suo *.user *.project.lock.json | Out-Default

  Write-Host -ForegroundColor Green "Updating assembly version"
  Write-Host
  Update-AssemblyInfoFiles $workingSourceDir ($majorVersion + '.0.0') $version

  Update-Project $workingSourceDir\Jali\project.json $signAssemblies

  foreach ($build in $builds)
  {
    $name = $build.Name
    if ($name -ne $null)
    {
      Write-Host -ForegroundColor Green "Building " $name
      Write-Host -ForegroundColor Green "Signed " $signAssemblies
      Write-Host -ForegroundColor Green "Key " $signKeyPath

      & $build.BuildFunction $build
    }
  }
}

# Optional build documentation, add files to final zip
task Package -depends Build {
  foreach ($build in $builds)
  {
    $finalDir = $build.FinalDir
    
    foreach ($package in $packages)
    {
      if ($build.BuildFunction -eq 'DnxBuild')
      {
        continue
      }

      $projectFolder = $package.Name + $(if ($build.SeparateProjectFolder) {"." + $build.ClrType} else {""})
      $artifactPath = "$workingDir\src\$projectFolder\bin\Release\$finalDir"
      $artifactRoot = $package.Name + "." + $finalDir

      robocopy $artifactPath $workingDir\Package\Bin\$finalDir "$artifactRoot.dll" "$artifactRoot.pdb" "$artifactRoot.xml" /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
    }
  }
  
  if ($buildNuGet)
  {
    $nugetVersion = $majorWithReleaseVersion
    if ($nugetPrelease -ne $null)
    {
      $nugetVersion = $nugetVersion + "-" + $nugetPrelease
    }

    foreach ($package in $packages)
    {
      $packageId = $package.Name
      $packageDropPath = "$workingDir\NuGet\$packageId"
      $nuspecName = "$packageId.nuspec"
      $nuspecPath = "$packageDropPath\$nuspecName"
      $buildNuspecPath = "$buildDir\$packageId\$nuspecName"

      New-Item -Path $packageDropPath -ItemType Directory

      Copy-Item -Path "$buildNuspecPath" -Destination $nuspecPath -recurse

      Write-Host "Updating nuspec file at $nuspecPath" -ForegroundColor Green
      Write-Host

      $xml = [xml](Get-Content $nuspecPath)
      Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'id']" -value $packageId
      Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'version']" -value $nugetVersion

      Write-Host $xml.OuterXml

      $xml.save($nuspecPath)

      #New-Item -Path $packageDropPath\tools -ItemType Directory
      #Copy-Item -Path "$buildDir\$packageId\install.ps1" -Destination $packageDropPath\install.ps1 -recurse

      foreach ($build in $builds)
      {
        if ($build.NuGetDir)
        {
          $finalDir = $build.FinalDir
          $frameworkDirs = $build.NuGetDir.Split(",")

          $projectFolder = $package.Name + $(if ($build.SeparateProjectFolder) {"." + $build.ClrType} else {""})
          $projectPath = "$workingDir\src\$projectFolder"
          $artifactPath = "$projectPath\bin\Release\$finalDir"
          $artifactRoot = $package.Name + "." + $finalDir
        
          if ($build.BuildFunction -ne 'DnxBuild')
          {
            foreach ($frameworkDir in $frameworkDirs)
            {
              robocopy $artifactPath $packageDropPath\lib\$frameworkDir "$artifactRoot.dll" "$artifactRoot.pdb" "$artifactRoot.xml" /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
            }
          }
        }
    
        robocopy $projectPath $packageDropPath\src *.cs /S /NFL /NDL /NJS /NC /NS /NP /XD Jali.Tests Jali.TestConsole obj .vs artifacts | Out-Default
      }


      Write-Host "Building NuGet package with ID $packageId and version $nugetVersion" -ForegroundColor Green
      Write-Host

      exec { & $workingDir\tools\nuget\3.2.0\nuget.exe pack $nuspecPath -Symbols }
      move -Path .\*.nupkg -Destination $packageDropPath
    }
  }

  Write-Host "Build documentation: $buildDocumentation"
  
  #if ($buildDocumentation)
  #{
  #  $mainBuild = $builds | where { $_.Name -eq "Jali" } | select -first 1
  #  $mainBuildFinalDir = $mainBuild.FinalDir
  #  $documentationSourcePath = "$workingDir\Package\Bin\$mainBuildFinalDir"
  #  $docOutputPath = "$workingDir\Documentation\"
  #  Write-Host -ForegroundColor Green "Building documentation from $documentationSourcePath"
  #  Write-Host "Documentation output to $docOutputPath"

  #  # Sandcastle has issues when compiling with .NET 4 MSBuild - http://shfb.codeplex.com/Thread/View.aspx?ThreadId=50652
  #  exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:DocumentationSourcePath=$documentationSourcePath" "/p:OutputPath=$docOutputPath" $docDir\doc.shfbproj | Out-Default } "Error building documentation. Check that you have Sandcastle, Sandcastle Help File Builder and HTML Help Workshop installed."
    
  #  move -Path $workingDir\Documentation\LastBuild.log -Destination $workingDir\Documentation.log
  #}
  
  #Copy-Item -Path $docDir\readme.txt -Destination $workingDir\Package\
  #Copy-Item -Path $docDir\license.txt -Destination $workingDir\Package\

  ## exclude package directories but keep packages\repositories.config
  #$packageDirs = gci $workingSourceDir\packages | where {$_.PsIsContainer} | Select -ExpandProperty Name

  #robocopy $workingSourceDir $workingDir\Package\Source\Src /MIR /NFL /NDL /NJS /NC /NS /NP /XD bin obj TestResults AppPackages $packageDirs /XF *.suo *.user | Out-Default
  #robocopy $buildDir $workingDir\Package\Source\Build /MIR /NFL /NDL /NJS /NC /NS /NP /XF runbuild.txt | Out-Default
  #robocopy $docDir $workingDir\Package\Source\Doc /MIR /NFL /NDL /NJS /NC /NS /NP | Out-Default
  #robocopy $toolsDir $workingDir\Package\Source\Tools /MIR /NFL /NDL /NJS /NC /NS /NP | Out-Default
  
  #exec { .\Tools\7-zip\7za.exe a -tzip $workingDir\$zipFileName $workingDir\Package\* | Out-Default } "Error zipping"
}

# Unzip package to a location
#task Deploy -depends Package {
#  exec { .\Tools\7-zip\7za.exe x -y "-o$workingDir\Deployed" $workingDir\$zipFileName | Out-Default } "Error unzipping"
#}

# Run tests on deployed files
#task Test -depends Deploy {

#  Update-Project $workingSourceDir\Jali\project.json $false

#  foreach ($build in $builds)
#  {
#    if ($build.TestsFunction -ne $null)
#    {
#      & $build.TestsFunction $build
#    }
#  }
#}

function MSBuildBuild($build)
{
  $name = $build.Name
  $finalDir = $build.FinalDir

  Write-Host
  Write-Host "Restoring $workingDir\$name.sln" -ForegroundColor Green
  [Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Process")
  exec { .\tools\nuget\3.2.0\NuGet.exe update -self }
#  exec { .\tools\nuget\3.2.0\NuGet.exe restore "$workingSourceDir\$name.sln" -verbosity detailed -configfile $workingSourceDir\..\nuget.config | Out-Default } "Error restoring $name"
  exec { .\tools\nuget\3.2.0\NuGet.exe restore "$workingDir\$name.sln" -verbosity detailed -configfile $workingDir\nuget.config | Out-Default } "Error restoring $name"

  $constants = GetConstants $build.Constants $signAssemblies

  Write-Host
  Write-Host "Building $workingDir\$name.sln" -ForegroundColor Green
  exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:CopyNuGetImplementations=true" "/p:Platform=Any CPU" "/p:PlatformTarget=AnyCPU" /p:OutputPath=bin\Release\$finalDir\ /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$signAssemblies" "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" /p:DefineConstants=`"$constants`" "$workingDir\$name.sln" | Out-Default } "Error building $name"
}

function DnxBuild($build)
{
  $name = $build.Name
  $projectPath = "$workingSourceDir\Jali\project.json"

#  exec { dnvm install $dnvmVersion -r clr -u | Out-Default }
  exec { dnvm use $dnvmVersion -r clr | Out-Default }

  Write-Host -ForegroundColor Green "Restoring packages for $name"
  Write-Host
  exec {
    try {
      dnu restore $projectPath | Out-Default
      Write-Host "Restore last exit code: $lastexitcode"
    }
    catch [System.Management.Automation.RemoteException]
    {
      Write-Host "Restore last exit code: $lastexitcode"
      Write-Host ("Restore exception: " + $_.ToString())
      $global:lastexitcode = 0
      $lastexitcode = 0
    }
  }

  Write-Host -ForegroundColor Green "Building $projectPath"
  exec { dnu build $projectPath --configuration Release | Out-Default }
}

function DnxTests($build)
{
  $name = $build.TestsName

  exec { dnvm install $dnvmVersion -r coreclr -u | Out-Default }
  exec { dnvm use $dnvmVersion -r coreclr | Out-Default }

  Write-Host -ForegroundColor Green "Restoring packages for $name"
  Write-Host
  exec {
    try {
      dnu restore "$workingSourceDir\Jali.Tests\project.json" | Out-Default
      Write-Host "Restore last exit code: $lastexitcode"
    }
    catch [System.Management.Automation.RemoteException]
    {
      Write-Host "Restore last exit code: $lastexitcode"
      Write-Host ("Restore exception: " + $_.ToString())
      $global:lastexitcode = 0
      $lastexitcode = 0
    }
  }

  Write-Host -ForegroundColor Green "Ensuring test project builds for $name"
  Write-Host

  try
  {
    Set-Location "$workingSourceDir\Jali.Tests"
    exec { dnx --configuration Release test | Out-Default }
  }
  finally
  {
    Set-Location $baseDir
  }
}

#function NUnitTests($build)
#{
#  $name = $build.TestsName
#  $finalDir = $build.FinalDir
#  $framework = $build.Framework

#  Write-Host -ForegroundColor Green "Copying test assembly $name to deployed directory"
#  Write-Host
#  robocopy "$workingSourceDir\Jali.Tests\bin\Release\$finalDir" $workingDir\Deployed\Bin\$finalDir /MIR /NFL /NDL /NJS /NC /NS /NP /XO | Out-Default

#  Copy-Item -Path "$workingSourceDir\Jali.Tests\bin\Release\$finalDir\Jali.Tests.dll" -Destination $workingDir\Deployed\Bin\$finalDir\

#  Write-Host -ForegroundColor Green "Running NUnit tests " $name
#  Write-Host
#  exec { .\Tools\NUnit\nunit-console.exe "$workingDir\Deployed\Bin\$finalDir\Jali.Tests.dll" /framework=$framework /xml:$workingDir\$name.xml | Out-Default } "Error running $name tests"
#}

function GetConstants($constants, $includeSigned)
{
  $signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

  return "CODE_ANALYSIS;TRACE;$constants$signed"
}

function GetVersion($majorVersion)
{
    $now = [DateTime]::UtcNow
    
    $year = $now.Year - 2000
    $month = $now.Month
    $totalMonthsSince2000 = ($year * 12) + $month
    $day = $now.Day
    $minor = "{0}{1:00}" -f $totalMonthsSince2000, $day
    
    $hour = $now.Hour
    $minute = $now.Minute
    $revision = "{0:00}{1:00}" -f $hour, $minute
    
    return $majorVersion + "." + $minor
}

function Update-AssemblyInfoFiles ([string] $workingSourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber)
{
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
    $fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
    
    Get-ChildItem -Path $workingSourceDir -r -filter AssemblyInfo.cs | ForEach-Object {
        
        $filename = $_.Directory.ToString() + '\' + $_.Name
        Write-Host $filename
        $filename + ' -> ' + $version
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}

function Edit-XmlNodes {
    param (
        [xml] $doc,
        [string] $xpath = $(throw "xpath is a required parameter"),
        [string] $value = $(throw "value is a required parameter")
    )
    
    $nodes = $doc.SelectNodes($xpath)
    $count = $nodes.Count

    Write-Host "Found $count nodes with path '$xpath'"
    
    foreach ($node in $nodes) {
        if ($node -ne $null) {
            if ($node.NodeType -eq "Element")
            {
                $node.InnerXml = $value
            }
            else
            {
                $node.Value = $value
            }
        }
    }
}

function Update-Project {
  param (
    [string] $projectPath,
    [string] $sign
  )

  $file = switch($sign) { $true { $signKeyPath } default { $null } }

  $json = (Get-Content $projectPath) -join "`n" | ConvertFrom-Json
  $options = @{"warningsAsErrors" = $true; "keyFile" = $file; "define" = ((GetConstants "dotnet" $sign) -split ";") }
  Add-Member -InputObject $json -MemberType NoteProperty -Name "compilationOptions" -Value $options -Force

  ConvertTo-Json $json -Depth 10 | Set-Content $projectPath
}

function Execute-Command($command) {
    $currentRetry = 0
    $success = $false
    do {
        try
        {
            & $command
            $success = $true
        }
        catch [System.Exception]
        {
            if ($currentRetry -gt 5) {
                throw $_.Exception.ToString()
            } else {
                write-host "Retry $currentRetry"
                Start-Sleep -s 1
            }
            $currentRetry = $currentRetry + 1
        }
    } while (!$success)
}