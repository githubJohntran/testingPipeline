# Set itself execution policy to unrestricted
# Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force

# The location of StreetlightVision.dll
$workingDirectory = ".."
# Get time to name ouput files
$now = Get-Date -Format "yyyy.MM.dd.hh.mm.ss.fffffff"
# Timeout of each test case (10 minutes)
$testCaseTimeout = 600000

# Set parallel count to run
$parallelCount = 2

# Change current directory to the directory of StreetlightVision.dll
Set-Location $PSScriptRoot 
Set-Location $workingDirectory

# 1. Back Office tests should be run firstly
nunit3-console StreetlightVision.dll --workers=$parallelCount --where="test==StreetlightVision.Tests.Coverage.Apps.BackOfficeAppTests" --output=BackOffice_Logs_$now.txt --result=BackOffice_Results_$now.xml --timeout=$testCaseTimeout

# 2. Then run tests to set values of Back Office options to default ones for other tests
nunit3-console StreetlightVision.dll --workers=$parallelCount --where="test==StreetlightVision.Tests.Xxx.XxxBackOffice" --output=XxxBackOffice_Logs_$now.txt --result=XxxBackOffice_Results_$now.xml --timeout=$testCaseTimeout

# 3. Then run tests to clean up left-over stuffs created during test running (this is optional)
nunit3-console StreetlightVision.dll --workers=$parallelCount --where="test==StreetlightVision.Tests.Xxx.XxxDataCleaner" --output=XxxDataCleaner_Logs_$now.txt --result=XxxDataCleaner_Results_$now.xml --timeout=$testCaseTimeout

# 4. Run other tests
nunit3-console StreetlightVision.dll --workers=$parallelCount --where="test!=StreetlightVision.Tests.Coverage.Apps.BackOfficeAppTests && test!=StreetlightVision.Tests.Xxx.XxxBackOffice && test!=StreetlightVision.Tests.Xxx.XxxDataCleaner" --output=AllOtherTests_Logs_$now.txt --result=AllOtherTests_Results_$now.xml --timeout=10800000