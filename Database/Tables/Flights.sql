CREATE TABLE [dbo].[Flights]
(
	[ClusterKey] INT IDENTITY (1, 1) NOT NULL INDEX [IX_ClusterKey] CLUSTERED,
    [Id] AS CONVERT([uniqueidentifier], JSON_VALUE([JsonDocument], '$.Id')) PERSISTED NOT NULL CONSTRAINT [PK_Flights] PRIMARY KEY NONCLUSTERED,
    [VersionTag] ROWVERSION NOT NULL,
    [JsonDocument] NVARCHAR(MAX) NOT NULL CONSTRAINT [CK_JsonDocument_IsJson] CHECK (ISJSON([JsonDocument]) = 1)
)
