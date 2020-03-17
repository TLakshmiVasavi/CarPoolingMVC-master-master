CREATE TABLE [dbo].[Ride] (
    [Id]               INT          NOT NULL,
    [VehicleId]        VARCHAR (10) NULL,
    [ProviderId]       VARCHAR (20) NULL,
    [NoOfOfferedSeats] INT          NULL,
    [Distance]         FLOAT (53)   NULL,
    [UnitDistanceCost] FLOAT (53)   NULL,
    [StartDateTime]    DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

