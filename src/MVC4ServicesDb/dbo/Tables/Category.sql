CREATE TABLE [dbo].[Category] (
    [CategoryId]  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100)  NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    [ts]          rowversion      NOT NULL,
    PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

