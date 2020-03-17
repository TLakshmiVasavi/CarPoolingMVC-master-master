CREATE TABLE [dbo].[Vehicle] (
    [Number]   VARCHAR (10) NOT NULL,
    [Model]    VARCHAR (10) NULL,
    [Capacity] INT          NULL,
    [Type]     VARCHAR (10) NULL,
    [UserId]   VARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([Number] ASC),
    CONSTRAINT [FK_Vehicle_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

