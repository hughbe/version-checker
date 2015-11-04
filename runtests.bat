
set "test_utils_path=C:\Users\Hugh\Documents\Jenkins-Addons"

set "test_relative_path=\VersionChecker\artifacts\bin\VersionCheckerTests\Debug\dotnet\VersionCheckerTests.dll"
set "source_relative_path=\VersionChecker\src\VersionChecker"

set "current_path=%cd%"
set "tests_full_path=%current_path%%test_relative_path%"
set "source_full_path=%current_path%%source_relative_path%"

set "xunit_path=%test_utils_path%\xUnit\xunit.console.exe"
set "opencover_path=%test_utils_path%\OpenCover\"

set "converter_path=%test_utils_path%\ReportConverter\"
set "generator_path=%test_utils_path%\ReportGenerator\"

set "xunit_coverage_path=%test_utils_path%\coverage.xunit.xml"
set "cobertura_coverage_path=%test_utils_path%\coverage.cobertura.xml"

set "bin_path=%test_utils_path%\Bin\"

cd %opencover_path%

opencover.console.exe -register:user -output:"%xunit_coverage_path%" -target:"%xunit_path%" -targetargs:"%tests_full_path%" -filter:"+[VersionChecker]* =[VersionCheckerTests]*" -hideskipped:Filter

cd %converter_path%

OpenCoverToCoberturaConverter.exe -input:"%xunit_coverage_path%" -output:"%cobertura_coverage_path%" -sources:"%source_full_path%"

cd %generator_path%

ReportGenerator.exe -reports:"%xunit_coverage_path%" -targetDir:"%bin_path%"

cd %current_path%