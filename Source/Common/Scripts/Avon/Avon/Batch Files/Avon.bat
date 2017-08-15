:: Command lines for running various Avon subprocesses.
@echo off

:: Input arguments.
SET solutionDirectoryPath="C:\Organizations\Minex\Repositories\Public\Source\Common\Experiments\Granville"

:: Call Avon.
SET avonExecutablePath="C:\Organizations\Minex\Binaries\Current\Avon\Avon.VS2010\Avon.exe"

:: Choose one of the operations below.
echo on
::CALL %avonExecutablePath% CreateNewSolutionSet Library Lib.XXX Library
::CALL %avonExecutablePath% CreateSolutionSetFromInitialVsVersionSolution
::CALL %avonExecutablePath% DistributeChangesFromDefaultVsVersionSolution %solutionDirectoryPath%
::CALL %avonExecutablePath% DistributeChangesFromSpecificVsVersionSolution %solutionDirectoryPath% VS2010
::CALL %avonExecutablePath% EnsureVsVersionedBinAndObjProperties
CALL %avonExecutablePath% SetDefaultVsVersionSolution %solutionDirectoryPath% VS2010
@echo off

:: Keep the command window open if this was the entry point batch file.
set interactive=1
echo %cmdcmdline% | find /i "%~0" >nul
if not errorlevel 1 set interactive=0

if _%interactive%_ == _0_ cmd /k