CREATE TABLE [dbo].[Vehicle] (
    [Number]   VARCHAR (50) NOT NULL,
    [Model]    VARCHAR (50) NOT NULL,
    [Capacity] INT          NOT NULL,
    [Type]     VARCHAR (50) NOT NULL,
    [OwnerId]  VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Number] ASC),
    CONSTRAINT [FK_Vehicle_User] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([Id])
);

