CREATE TABLE [dbo].[Positions] (
  [PositionUid] [uniqueidentifier] NOT NULL DEFAULT (newid()),
  [DepartamenUid] [uniqueidentifier] NOT NULL,
  [Name] [varchar](100) NOT NULL,
  [Description] [varchar](300) NULL,
  CONSTRAINT [PK_Positions_PositionUid] PRIMARY KEY CLUSTERED ([PositionUid])
)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX [UK_Positions]
  ON [dbo].[Positions] ([DepartamenUid], [Name])
  ON [PRIMARY]
GO

ALTER TABLE [dbo].[Positions]
  ADD CONSTRAINT [FK_Positions_Departaments_DepartamentUid] FOREIGN KEY ([DepartamenUid]) REFERENCES [dbo].[Departaments] ([DepartamentUid])
GO