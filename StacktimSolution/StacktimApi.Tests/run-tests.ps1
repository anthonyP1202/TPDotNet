dotnet test --collect:"XPlat Code Coverage"

dotnet tool run reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

Start-Process "$PWD/coverage-report/index.html"