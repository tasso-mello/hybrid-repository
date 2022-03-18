USE [HybridRepoDb]

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Test' and xtype='U')
BEGIN
    CREATE TABLE Test (
        Id uniqueidentifier PRIMARY KEY,
        Name VARCHAR(100)
    )
END