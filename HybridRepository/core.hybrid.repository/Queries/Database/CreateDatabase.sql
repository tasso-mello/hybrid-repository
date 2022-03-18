USE [master]

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'HybridRepoDb')
    BEGIN
        CREATE DATABASE [HybridRepoDb]
    END