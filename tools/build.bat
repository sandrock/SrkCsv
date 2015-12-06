
@ECHO OFF

echo:
echo ==========================
echo SrkCsv package builder
echo ==========================
echo:

set currentDirectory=%CD%
cd ..
cd build
set outputDirectory=%CD%
cd %currentDirectory%
set nuget=%CD%\..\tools\nuget.exe
set msbuild4="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
set vincrement=%CD%\..\tools\Vincrement.exe


echo Check CLI apps
echo -----------------------------

if not exist %nuget% (
 echo ERROR: nuget could not be found, verify path. exiting.
 echo Configured as: %nuget%
 pause
 exit
)

if not exist %msbuild4% (
 echo ERROR: msbuild 4 could not be found, verify path. exiting.
 echo Configured as: %msbuild4%
 pause
 exit
)

if not exist %vincrement% (
 echo ERROR: vincrement could not be found, verify path. exiting.
 echo Configured as: %vincrement%
 pause
 exit
)

echo Everything is fine.

REM echo:
REM echo Clean output directory
REM echo -----------------------------
REM cd ..
REM if exist lib (
REM  rmdir /s /q lib
REM  if not %ERRORLEVEL% == 0 (
REM   echo ERROR: clean failed. exiting.
REM   pause
REM   exit
REM  )
REM )
REM echo Done.

pause

echo:
echo Build solution
echo -----------------------------
cd ..
cd src
set solutionDirectory=%CD%
%msbuild4% "SrkCsv.sln" /p:Configuration=Release /nologo /verbosity:q

if not %ERRORLEVEL% == 0 (
 echo ERROR: build failed. exiting.
 cd %currentDirectory%
 pause
 exit
)
echo Done.

echo:
echo Copy libs
echo -----------------------------
mkdir %outputDirectory%\lib
mkdir %outputDirectory%\lib\net40
xcopy /Q %solutionDirectory%\SrkCsv\bin\Release\* %outputDirectory%\lib\net40\
echo Done.




echo:
echo Increment version number
echo -----------------------------

echo Hit return to continue...
pause 
%vincrement% -file=%currentDirectory%\..\version.txt 0.0.1 %currentDirectory%\..\version.txt
if not %ERRORLEVEL% == 0 (
 echo ERROR: vincrement. exiting.
 cd %currentDirectory%
 pause
 exit
)
set /p version=<%currentDirectory%\..\version.txt
echo Done: %version%



echo:
echo Build NuGet package
echo -----------------------------

echo Hit return to continue...
pause 
cd %outputDirectory%
%nuget% pack %outputDirectory%\..\SrkCsv.nuspec -BasePath %outputDirectory% -Version %version%
echo Done.




echo:
echo Push NuGet package
echo -----------------------------

echo Hit return to continue...
pause 
cd %outputDirectory%
%nuget% push SrkCsv.%version%.nupkg
echo Done.





cd %currentDirectory%
pause



