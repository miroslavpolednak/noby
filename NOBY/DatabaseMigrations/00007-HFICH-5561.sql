CREATE TABLE [dbo].[FeAvailableUserPermission](
	[PermissionCode] [int] NOT NULL,
 CONSTRAINT [PK_FeAvailableUserPermission] PRIMARY KEY CLUSTERED 
(
	[PermissionCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dbo].[FeAvailableUserPermission] ([PermissionCode]) VALUES (1)
GO
INSERT [dbo].[FeAvailableUserPermission] ([PermissionCode]) VALUES (100)
GO
INSERT [dbo].[FeAvailableUserPermission] ([PermissionCode]) VALUES (101)
GO
INSERT [dbo].[FeAvailableUserPermission] ([PermissionCode]) VALUES (110)
GO
