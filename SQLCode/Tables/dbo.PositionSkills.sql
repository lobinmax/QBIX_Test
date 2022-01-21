CREATE TABLE [dbo].[PositionSkills] (
  [SkillUid] [uniqueidentifier] NOT NULL DEFAULT (newid()),
  [PositionUid] [uniqueidentifier] NOT NULL,
  [Name] [varchar](200) NOT NULL,
  CONSTRAINT [PK_Skills_SkillUid] PRIMARY KEY CLUSTERED ([SkillUid])
)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX [UK_Skills]
  ON [dbo].[PositionSkills] ([PositionUid], [Name])
  ON [PRIMARY]
GO

ALTER TABLE [dbo].[PositionSkills]
  ADD CONSTRAINT [FK_Skills_Positions_PositionUid] FOREIGN KEY ([PositionUid]) REFERENCES [dbo].[Positions] ([PositionUid])
GO