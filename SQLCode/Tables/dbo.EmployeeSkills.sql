CREATE TABLE [dbo].[EmployeeSkills] (
  [EmployeeSkillUid] [uniqueidentifier] NOT NULL DEFAULT (newid()),
  [EmployeeUid] [uniqueidentifier] NOT NULL,
  [SkillUid] [uniqueidentifier] NOT NULL,
  CONSTRAINT [PK_EmployeeSkills_EmployeeSkillUid] PRIMARY KEY CLUSTERED ([EmployeeSkillUid])
)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX [IDX_EmployeeSkills]
  ON [dbo].[EmployeeSkills] ([EmployeeUid], [SkillUid])
  ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeSkills]
  ADD CONSTRAINT [FK_EmployeeSkills_Employees_EmployeeUid] FOREIGN KEY ([EmployeeUid]) REFERENCES [dbo].[Employees] ([EmployeeUid])
GO

ALTER TABLE [dbo].[EmployeeSkills]
  ADD CONSTRAINT [FK_EmployeeSkills_Skills_SkillUid] FOREIGN KEY ([SkillUid]) REFERENCES [dbo].[PositionSkills] ([SkillUid])
GO