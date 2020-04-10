CREATE TABLE [dbo].[Ride] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [VehicleId]        VARCHAR (50)  NOT NULL,
    [ProviderId]       VARCHAR (50)  NOT NULL,
    [StartDateTime]    DATETIME      NOT NULL,
    [NoOfOfferedSeats] INT           NOT NULL,
    [Distance]         FLOAT (53)    NOT NULL,
    [UnitDistanceCost] FLOAT (53)    NOT NULL,
    [Locations]        VARCHAR (MAX) NOT NULL,
    [Distances]        VARCHAR (MAX) NOT NULL,
    [Durations]        VARCHAR (MAX) NOT NULL,
    [Duration]         DATETIME      NOT NULL,
    CONSTRAINT [PK_Ride] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ride_User] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Ride_Vehicle] FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicle] ([Number])
);

