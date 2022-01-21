CREATE TABLE [dbo].[Departaments] (
  [DepartamentUid] [uniqueidentifier] NOT NULL DEFAULT (newid()),
  [Name] [varchar](150) NOT NULL,
  CONSTRAINT [PK_Deportaments_DeportamentUid] PRIMARY KEY CLUSTERED ([DepartamentUid])
)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX [UK_Deportaments_Name]
  ON [dbo].[Departaments] ([Name])
  ON [PRIMARY]
GO