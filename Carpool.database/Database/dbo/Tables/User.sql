CREATE TABLE [dbo].[User] (
    [Id]           VARCHAR (50) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [Mail]         VARCHAR (50) NOT NULL,
    [Password]     VARCHAR (50) NOT NULL,
    [Balance]      MONEY        DEFAULT ((0)) NOT NULL,
    [MobileNumber] NUMERIC (10) NOT NULL,
    [Age]          INT          NOT NULL,
    [Gender]       VARCHAR (50) NOT NULL,
    [Photo]        IMAGE        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

