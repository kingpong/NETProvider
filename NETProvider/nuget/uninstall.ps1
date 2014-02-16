param($installPath, $toolsPath, $package, $project)
write-host uninstall.ps1

$solutionDir = [IO.Path]::GetDirectoryName($dte.Solution.FullName) + "\"
$path = $installPath.Replace($solutionDir, "`$(SolutionDir)")
$nativeAssembliesDir = Join-Path $path "NativeBinaries\*.*"

$postBuildCmd = "
C:\Windows\System32\xcopy /s /y /d `"$nativeAssembliesDir`" `"`$(TargetDir)`""

#Remove
try {
    # Get the current Post Build Event cmd
    $currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value

    # Remove our post build command from it (if it's there)
    $project.Properties.Item("PostBuildEvent").Value = $currentPostBuildCmd.Replace($postBuildCmd, '')
} catch {
    # Accessing $project.Properties might throw
}
