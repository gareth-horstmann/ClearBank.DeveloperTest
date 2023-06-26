if(!(Test-Path -Path "$PSScriptRoot/.tools/reportgenerator.exe")) {
  dotnet tool install dotnet-reportgenerator-globaltool --tool-path "$PSScriptRoot/.tools"
}

dotnet test --logger trx --results-directory $PSScriptRoot/test_results --configuration Release /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Include=ConstructionLine.*;
.tools/reportgenerator -reports:"$PSScriptRoot/**/*.cobertura.xml" -targetdir:"CodeCoverage" -reporttypes:"Html";
Invoke-Item $PSScriptRoot/CodeCoverage/index.html;
