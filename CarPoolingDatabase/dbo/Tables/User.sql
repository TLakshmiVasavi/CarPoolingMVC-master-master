CREATE TABLE [dbo].[User] (
    [Id]       VARCHAR (20)  NOT NULL,
    [Name]     VARCHAR (10)  NOT NULL,
    [Mail]     VARCHAR (10)  NOT NULL,
    [Password] NVARCHAR (10) NOT NULL,
    [Number]   NUMERIC (10)  NOT NULL,
    [Age]      INT           NULL,
    [Gender]   VARCHAR (10)  NULL,
    [Balance] MONEY NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

