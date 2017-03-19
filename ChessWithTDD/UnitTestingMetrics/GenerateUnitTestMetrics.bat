REM @ECHO OFF

REM Constant declarations
set openCoverConsole="%~dp0..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
set nunitConsole="%~dp0..\..\packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe"
set namespacePrefix="ChessWithTDD.Tests."
set whitePawn="WhitePawn"
set blackPawn="BlackPawn"
set knight="Knight"
set whitePawnTests="WhitePawnTests"
set blackPawnTests="BlackPawnTests"
set knightTests="KnightTests"

set openCoverReportsFolder="%~dp0..\..\OpenCoverReports"
set openCoverResultsFolder="%~dp0..\..\OpenCoverResults"
set nunitResultsFolder="%~dp0.\NUnitResults"

REM *** Main entry point ***
call:DeleteFoldersAndContents
call:CreateFolders
call:RunOpenCoverUnitTestMetrics
call:RunLaunchReports
GOTO:EOF
REM *** The End ***

:DeleteFoldersAndContents
if exist %openCoverReportsFolder% rmdir %openCoverReportsFolder% /s /q 
if exist %openCoverResultsFolder% rmdir %openCoverResultsFolder% /s /q
if exist %nunitResultsFolder% rmdir %nunitResultsFolder% /s /q
GOTO:EOF

:CreateFolders
mkdir %openCoverReportsFolder%
mkdir %openCoverResultsFolder%
mkdir %nunitResultsFolder%
GOTO:EOF

:RunOpenCoverUnitTestMetrics
call:RunSpecificTestFixture %blackPawnTests% %blackPawn%
call:RunSpecificTestFixture %whitePawnTests% %whitePawn%
call:RunSpecificTestFixture %knightTests% %knight%
call:RunReportGeneratorFromOpenCoverResults %whitePawnTests% 
call:RunReportGeneratorFromOpenCoverResults %blackPawnTests%
call:RunReportGeneratorFromOpenCoverResults %knightTests%
GOTO:EOF

:RunLaunchReports
call:RunLaunchReport %whitePawnTests%
call:RunLaunchReport %blackPawnTests%
call:RunLaunchReport %knightTests%
GOTO:EOF

:RunSpecificTestFixture
:: -- %~1 - name of test fixture
:: -- %~2 - name of test piece class to add in report. Always include parents Piece class
%openCoverConsole% ^
-target:%nunitConsole% ^
-register:user ^
-filter:"-[ChessWithTDD.*]* +[ChessWithTDD]ChessWithTDD.Piece +[ChessWithTDD]ChessWithTDD.%~2" ^
-targetargs:"%~dp0..\bin\Debug\ChessWithTDD.dll --where \"class == %namespacePrefix%%~1\" --work=%nunitResultsFolder%" ^
-output:"%openCoverResultsFolder%\oc_"%~1"_results.xml" ^
-searchdirs:"%~dp0..\bin\Debug"
GOTO:EOF

:RunReportGeneratorFromOpenCoverResults
:: -- %~1 - report 1
ECHO ON
ECHO %openCoverResultsFolder%\oc_%~1_results.xml
"%~dp0..\..\packages\ReportGenerator.2.5.5\tools\ReportGenerator.exe" ^
-targetdir:"%openCoverReportsFolder%\%~1" ^
-reports:"%openCoverResultsFolder%\oc_%~1_results.xml" 
GOTO:EOF

:RunLaunchReport
:: -- %~1 - report 1
start "Test coverage report" "%openCoverReportsFolder%\%~1\index.htm"
GOTO:EOF