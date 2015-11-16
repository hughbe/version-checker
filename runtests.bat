set "open_cover=src\packages\OpenCover.4.6.166\tools\OpenCover.Console.exe"
set "report_generator=src\packages\ReportGenerator.2.3.4.0\tools\ReportGenerator.exe"

set "xunit=src\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe"

set "test_name=src\tests\bin\debug\VersionChecker.Tests.dll"

set "report_name=coverage.xml"
set "report_path=resources\coverage"

%open_cover% -register:user -output:"%report_name%" -filter:"+[VersionChecker*]* -[VersionChecker.Tests]*" -target:"%xunit%" -targetargs:"%test_name% -noshadow"

%report_generator% -reports:"%report_name%" -targetdir:"%report_path%"

PAUSE
