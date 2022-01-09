/****** Object:  Table [dbo].[Project]    Script Date: 04.01.2022 10:49:59 ******/

CREATE TABLE [dbo].[Project](
                                [Id] [bigint] NOT NULL,
                                [Name] [nvarchar](250) NOT NULL,
                                [ProjectManagerId] [bigint] NOT NULL,
                                CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED
                                    (
                                     [Id] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 04.01.2022 10:49:59 ******/

CREATE TABLE [dbo].[Resource](
                                 [Id] [bigint] NOT NULL,
                                 [FirstName] [nvarchar](500) NOT NULL,
                                 [LastName] [nvarchar](500) NOT NULL,
                                 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED
                                     (
                                      [Id] ASC
                                         )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 04.01.2022 10:49:59 ******/

CREATE TABLE [dbo].[Task](
                             [Id] [bigint] NOT NULL,
                             [ProjectId] [bigint] NOT NULL,
                             [Name] [nvarchar](250) NOT NULL,
                             CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED
                                 (
                                  [Id] ASC
                                     )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [wasp]
GO

/****** Object:  Table [dbo].[LayerTest]    Script Date: 04.01.2022 15:58:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LayerTest](
                                  [Id] [int] NOT NULL,
                                  [Name] [varchar](100) NOT NULL,
                                  [ParentId] [int] NULL,
                                  CONSTRAINT [PK_LayerTest] PRIMARY KEY CLUSTERED
                                      (
                                       [Id] ASC
                                          )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (1, N'Test', 5)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (111, N'TestProjekt', 4)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (198, N'Test-Projekt', 2)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (227, N'4711 Test-Projekt', 4)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (332, N'Keine Ahnung', 4)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (487, N'Mein Test', 2)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (532, N'Auch ein TestProjekt', 3)
GO
INSERT [dbo].[Project] ([Id], [Name], [ProjectManagerId]) VALUES (777, N'Test Neu', 3)
GO
INSERT [dbo].[Resource] ([Id], [FirstName], [LastName]) VALUES (1, N'Matthias', N'Burger')
GO
INSERT [dbo].[Resource] ([Id], [FirstName], [LastName]) VALUES (2, N'Peter', N'Griffin')
GO
INSERT [dbo].[Resource] ([Id], [FirstName], [LastName]) VALUES (3, N'Meg', N'Griffin')
GO
INSERT [dbo].[Resource] ([Id], [FirstName], [LastName]) VALUES (4, N'Roger', N'Smith')
GO
INSERT [dbo].[Resource] ([Id], [FirstName], [LastName]) VALUES (5, N'Peter', N'Griffin')
GO
INSERT [dbo].[Resource] ([Id], [FirstName], [LastName]) VALUES (10, N'Anna', N'Nass')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (12, 227, N'Vorgang 1')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (23, 198, N'Vorgang 1')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (34, 227, N'Vorgang 2')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (37, 198, N'Vorgang 4')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (51, 198, N'Vorgang 5')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (53, 332, N'Vorgang 2')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (56, 198, N'Vorgang 3')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (77, 487, N'Vorgang 1')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (89, 198, N'Vorgang 2')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (90, 332, N'Vorgang 1')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (92, 227, N'Vorgang 3')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (566, 1, N'Test Vorgang')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (3333, 198, N'Test Task')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (5556, 532, N'TEST VORGANG')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (6789, 227, N'Test')
GO
INSERT [dbo].[Task] ([Id], [ProjectId], [Name]) VALUES (7887, 111, N'vorgang')
GO


INSERT [dbo].[Modules] ([Id], [Name]) VALUES (N'000000', N'Project-Demo')
GO
INSERT [dbo].[DataAreas] ([Id], [ModuleId], [Name], [ParentId], [DataTableId]) VALUES (N'000000', N'000000', N'Project', NULL, N'Project')
GO
INSERT [dbo].[DataAreas] ([Id], [ModuleId], [Name], [ParentId], [DataTableId]) VALUES (N'000001', N'000000', N'Task', N'000000', N'Task')
GO
INSERT [dbo].[DataAreas] ([Id], [ModuleId], [Name], [ParentId], [DataTableId]) VALUES (N'000002', N'000000', N'Resource', N'000000', N'Resource')
GO
SET IDENTITY_INSERT [dbo].[DataAreaReference] ON
GO
INSERT [dbo].[DataAreaReference] ([Id], [DataAreaId], [ReferenceDataItemId], [ReferenceDataTableId], [KeyDataItemId], [KeyDataItemDataTableId]) VALUES (3, N'000001', N'Id', N'Project', N'ProjectId', N'Task')
GO
INSERT [dbo].[DataAreaReference] ([Id], [DataAreaId], [ReferenceDataItemId], [ReferenceDataTableId], [KeyDataItemId], [KeyDataItemDataTableId]) VALUES (4, N'000002', N'Id', N'Resource', N'ProjectManagerId', N'Project')
GO
SET IDENTITY_INSERT [dbo].[DataAreaReference] OFF
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000000', N'000000', N'Project', N'Id', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000001', N'000000', N'Project', N'Name', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000002', N'000001', N'Task', N'Id', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000003', N'000001', N'Task', N'Name', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000004', N'000001', N'Project', N'Name', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000005', N'000002', N'Resource', N'FirstName', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000006', N'000002', N'Resource', N'LastName', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000007', N'000000', N'Resource', N'Id', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000008', N'000002', N'Resource', N'Id', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000009', N'000000', N'Project', N'ProjectManagerId', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000010', N'000001', N'Task', N'ProjectId', NULL)
GO
INSERT [dbo].[DataFields] ([Id], [DataAreaId], [DataTableId], [DataItemId], [FilterFrom]) VALUES (N'000011', N'000001', N'Project', N'Id', NULL)
GO
