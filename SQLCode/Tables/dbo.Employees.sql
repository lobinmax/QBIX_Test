CREATE TABLE [dbo].[Employees] (
  [EmployeeUid] [uniqueidentifier] NOT NULL CONSTRAINT [DF__Employees__Emplo__1DE57479] DEFAULT (newid()),
  [DepartamentUid] [uniqueidentifier] NOT NULL,
  [PositionUid] [uniqueidentifier] NOT NULL,
  [Surname] [varchar](50) NOT NULL,
  [Name] [varchar](50) NOT NULL,
  [Patronymic] [varchar](50) NOT NULL,
  [DtOfBirth] [date] NOT NULL,
  [Address] [varchar](300) NULL,
  [DtHired] [date] NOT NULL,
  [DtDismissed] [date] NULL,
  [PhoneNumber] [varchar](30) NULL,
  [Email] [varchar](50) NULL,
  [Room] [varchar](15) NULL,
  [TabelianNumber] [varchar](15) NOT NULL,
  CONSTRAINT [PK_Employees_EmployeeUid] PRIMARY KEY CLUSTERED ([EmployeeUid])
)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX [UK_Employees]
  ON [dbo].[Employees] ([Name], [Surname], [Patronymic], [DtOfBirth])
  ON [PRIMARY]
GO

ALTER TABLE [dbo].[Employees]
  ADD CONSTRAINT [FK_Employees_Departaments_DepartamentUid] FOREIGN KEY ([DepartamentUid]) REFERENCES [dbo].[Departaments] ([DepartamentUid])
GO

ALTER TABLE [dbo].[Employees]
  ADD CONSTRAINT [FK_Employees_Positions_PositionUid] FOREIGN KEY ([PositionUid]) REFERENCES [dbo].[Positions] ([PositionUid])
GO