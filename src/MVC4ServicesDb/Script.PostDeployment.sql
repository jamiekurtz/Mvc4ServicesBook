/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


if not exists(select * from dbo.Priority where Name = 'Low')
	insert into dbo.Priority(Name, Ordinal) values('Low', 0);
if not exists(select * from dbo.Priority where Name = 'Medium')
	insert into dbo.Priority(Name, Ordinal) values('Medium', 1);
if not exists(select * from dbo.Priority where Name = 'High')
	insert into dbo.Priority(Name, Ordinal) values('High', 2);
