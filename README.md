# SqlCodeScanner
Scan for Breaking SQL Changes!

## Overview

The SqlCodeScanner intended to catch backwards compatibility issues in changes to SqlServer projects. It is intended to be run during automated builds: one for the master branch and one for each additional branch. For the non-master branches, it will generate an html report which will list all non-backwards compatible changes.
To use it, build the project and add the SqlScanner.exe file to your automated build. Run it with the appropriate arguments and it will make the backwards compatibility report.

## License

Uses the Apache 2 license: https://www.apache.org/licenses/LICENSE-2.0.txt

## Assumptions

All SqlServer projects must follow the specified folder structure:
DatabaseName\SchemaName\Stored Procedures\
The stored procedures must be .sql files inside of the “Stored Procedures” folder. It is possible to use a different name for the stored procedures folder by changing the “StoredProcedureDirectoryName” setting in the app.config file.

## Backwards Compatibility Rules

Stored Procedure Parameters:
1.	Parameters cannot be removed from existing stored procedures. 
2.	New parameters to existing stored procedures must be defaulted.
3.	Parameters of existing stored procedures cannot be re-ordered.
4.	Existing stored procedures cannot be renamed the same name with different case.

TODO
Check return types of stored parameters. Currently only BIGINT change is checked.

Stored Procedure Return Values:
1.	Return values cannot be removed from existing stored procedures. 
2.	Return values cannot have their order changes in an existing stored procedure. New ones can be added but only at the end of the select or output statement.

## Command Line Arguments

Enter no arguments, -h or -help for instructions

For scanning, there are two commands: create and compare

Specify 'create' argument to generate an xml data file
Specify 'compare' argument to compare to an xml data file and then generate an html report

For create, specify the following arguments:
-solution-path or -s, the path to SQL to project to be scanned
-data-file or -d, path to the data file to be generated

For compare, specify the following arguments: 
-solution-path or -s, the path to SQL to project to be scanned
-data-file or -d, path to the xml data file to do the comparison against
-report or -r, path to the html report file to be generated

create example: SqlScanner.exe create -solution-path c:\git\SqlProj\ -data-file c:\reports\datafile.xml
compare example: SqlScanner.exe compare -solution-path c:\git\SqlProj\ -data-file c:\reports\datafile.xml -report c:\reports\comparison.html

## Reading the Results

If the scan completes with no backwards compatibility issues found, a 0 will be returned. If there were backwards compatibility issues found, a 2 will be returned instead.
These values can be used to fail builds, such as in Jenkins or a similar tool.
## Storing the Master XML Report

The output of the create command is an xml file that can be used for further compare scans. This should be created based off the project’s master branch, that is it should match what is deployed to production. 
Once the file has been created it should be archived somewhere, keeping only the most recent. This can be done using the archiveArtifacts command in a Jenkins pipeline.
All other branches should run using the compare argument and use that master xml file as their data file.

## Reading the HTML Report

The html report should also be published for each branch so the developers can read it. If there are no errors, it will simply say “No errors found.”. If there are errors, it will list them in an html table. The table will show the following information:
Database | SP Name | Parameter Name | Error
These details will tell the developers what to fix.
