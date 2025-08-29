-- Create the News database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'NewsApiDb')
BEGIN
    CREATE DATABASE NewsApiDb;
END
GO

USE NewsApiDb;
GO

-- Create a simple health check table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HealthCheck' AND xtype='U')
BEGIN
    CREATE TABLE HealthCheck (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Status NVARCHAR(50) NOT NULL,
        Timestamp DATETIME2 DEFAULT GETUTCDATE()
    );
    
    INSERT INTO HealthCheck (Status) VALUES ('Database Initialized');
END
GO

PRINT 'Database initialization completed successfully!';
