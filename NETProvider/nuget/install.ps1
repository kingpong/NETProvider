param($installPath, $toolsPath, $package, $project)
Write-Host install.ps1

$solutionDir = [IO.Path]::GetDirectoryName($dte.Solution.FullName) + "\"
$path = $installPath.Replace($solutionDir, "`$(SolutionDir)")
$nativeAssembliesDir = Join-Path $path "NativeBinaries\*.*"

$postBuildCmd = "
C:\Windows\System32\xcopy /s /y /d `"$nativeAssembliesDir`" `"`$(TargetDir)`""

#Add
$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value
# Append our post build command if it's not already there
if (!$currentPostBuildCmd.Contains($postBuildCmd)) {
   Write-Host Updating PostBuildEvent
   $project.Properties.Item("PostBuildEvent").Value += $postBuildCmd
}
else {
   Write-Host Skipped Updating PostBuildEvent
}
