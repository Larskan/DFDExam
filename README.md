# Database for Developers Exam
This is the background project for the written synopsis.

Topic: Benchmarking Relational & NoSQL databases

## Databases
Relational Database chosen: SQL Server.

NoSQL Database chosen: MongoDB.

## How is the test set up?
Uses a basic testing using Stopwatch and a more advanced testing using BenchmarkDotNet.

### The Stopwatch
Uses the BenchmarkRunner class to launch a basic timing of SQL INSERT, MongoDB INSERT, SQL READ and MongoDB READ.

Ensure that the Stopwatch Region in Program.cs is not out-commented and that the BenchmarkDotNet Region is out-commented.

### The BenchmarkDotNet
Uses the DatabaseBenchmark class to launch an advanced statistical analysis of SQL INSERT, MongoDB INSERT, SQL READ and MongoDB READ.

Ensure that the BenchmarkDotNet Region in Program.cs is not out-commented and that the Stopwatch Region is out-commented.

## Requirements for running
1) A successful run of:
```
docker-compose up -d
```

2) A successful build of:
```
dotnet build
```

3) BenchmarkDotNet Package and MongoDB.Driver Package

4) An active SQL Server connection.


## Run either test with
```
dotnet run -c Release
```


