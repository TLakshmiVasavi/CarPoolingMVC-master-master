CREATE TABLE [dbo].[Booking] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [RideId]      INT          NOT NULL,
    [RiderId]     VARCHAR (50) NOT NULL,
    [Source]      VARCHAR (50) NOT NULL,
    [Destination] VARCHAR (50) NOT NULL,
    [NoOfSeats]   INT          NOT NULL,
    [StartDate]   DATE         NOT NULL,
    [Status]      VARCHAR (50) DEFAULT ('Requested') NOT NULL,
    [Cost]        FLOAT (53)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Booking_Ride] FOREIGN KEY ([RideId]) REFERENCES [dbo].[Ride] ([Id]),
    CONSTRAINT [FK_Booking_User] FOREIGN KEY ([RiderId]) REFERENCES [dbo].[User] ([Id])
);

