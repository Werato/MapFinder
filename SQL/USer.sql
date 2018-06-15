USE [LearnDB]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 15.06.2018 17:36:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Name] [varchar](15) NULL,
	[Soname] [varchar](15) NULL,
	[Phone] [varchar](12) NULL,
	[Password] [varchar](max) NULL,
	[Lon] [real] NULL,
	[Lat] [real] NULL,
	[Email] [varchar](25) NULL,
	[Price] [money] NULL,
	[Description] [varchar](max) NULL,
	[UserId] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


