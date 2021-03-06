USE [master]
GO
/****** Object:  Database [HospitalManagement]    Script Date: 03/04/2019 16:09:04 ******/
CREATE DATABASE [HospitalManagement]
GO 
USE [HospitalManagement]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 03/04/2019 16:09:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 03/04/2019 16:09:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](250) NOT NULL,
	[LoginName] [nvarchar](250) NOT NULL,
	[Email] [nvarchar](250) NULL,
	[PositionID] [int] NULL,
	[Password] [nvarchar](250) NOT NULL,
	[Gender] [bit] NOT NULL,
	[DepartmentID] [int] NULL,
	[DOB] [date] NULL,
	[JobType] [int] NULL,
	[EducationID] [int] NULL,
 CONSTRAINT [PK_UserManagement] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserEducation]    Script Date: 03/04/2019 16:09:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEducation](
	[UserEduID] [int] IDENTITY(1,1) NOT NULL,
	[EducationName] [nvarchar](50) NULL,
 CONSTRAINT [PK_UserEducation] PRIMARY KEY CLUSTERED 
(
	[UserEduID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserPosition]    Script Date: 03/04/2019 16:09:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPosition](
	[PositionID] [int] IDENTITY(1,1) NOT NULL,
	[PositionName] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_UserPosition] PRIMARY KEY CLUSTERED 
(
	[PositionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (1, N'Board Of Director', N'Ban Giám Đốc')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (2, N'Admin - Financial Accounting', N'Phòng Hành Chánh Quản Trị - Tài Chánh Kế Toán')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (3, N'General Planning', N'Phòng Kế Hoạch Tổng Hợp')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (4, N'Dentail Surgery', N'Khoa Phẫu Thuật Răng Hàm Mặt')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (5, N'Plastic Surgery', N'Khoa Phẫu Thuật Tạo Hình')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (6, N'Anesthesiology - Intensive Care Unit', N'Khoa Gây Mê Hồi Sức')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (7, N'Laboratory - XRay', N'Khoa Cận Lâm Sàng - Xét Nghiệm -X Quang')
INSERT [dbo].[Department] ([DepartmentID], [DepartmentName], [Description]) VALUES (8, N'Pharmacy', N'Khoa Dược')
SET IDENTITY_INSERT [dbo].[Department] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserID], [UserName], [LoginName], [Email], [PositionID], [Password], [Gender], [DepartmentID], [DOB], [JobType], [EducationID]) VALUES (1, N'admin', N'admin', N'admin@admin.com', 1, N'21232f297a57a5a743894a0e4a801fc3', 0, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserEducation] ON 

INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (1, N'PGS. TS. Y khoa')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (2, N'PGS. TS. Y khoa')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (3, N'Ths. Kinh tế')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (4, N'Kỹ sư TCKT')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (5, N'Kỹ sư KT')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (6, N'Chuyên viên KT')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (7, N'43811')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (8, N'Bác sĩ Y khoa')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (9, N'Trung học y tế')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (10, N'ThS PTHM')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (11, N'Bác sĩ PTHM')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (12, N'Bác sĩ PTHM')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (13, N'Bác sĩ PTHM')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (14, N'Bác sĩ PTHM')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (15, N'Ths. BS. PTHM')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (16, N'Điều dưỡng')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (17, N'BS CKI')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (18, N'Ths. Bác sĩ')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (19, N'Nha sĩ')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (20, N'Điều dưỡng ')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (21, N'PGS TS Y khoa')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (22, N'PGS TS Y khoa')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (23, N'TS Bác sĩ Y khoa')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (24, N'Điều dưỡng')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (25, N'BS CKI Gây mê')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (26, N'Điều dưỡng trưởng')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (27, N'Kỹ thuật viên gây mê')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (28, N'Điều dưỡng')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (29, N'Hộ lý')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (30, N'Chẩn doán hình ảnh')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (31, N'Xét nghiệm')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (32, N'KHOA DƯỢC')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (33, N'Dược sĩ ')
INSERT [dbo].[UserEducation] ([UserEduID], [EducationName]) VALUES (34, N'Dược sĩ Trung học')
SET IDENTITY_INSERT [dbo].[UserEducation] OFF
SET IDENTITY_INSERT [dbo].[UserPosition] ON 

INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (1, N'Giám Đốc')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (2, N'Phó Giám Đốc')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (3, N'Trưởng Khoa')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (4, N'Phó Khoa')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (5, N'Trưởng Phòng')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (6, N'Phó Phòng')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (7, N'Nhân Viên')
INSERT [dbo].[UserPosition] ([PositionID], [PositionName]) VALUES (8, N'Bảo Vệ')
SET IDENTITY_INSERT [dbo].[UserPosition] OFF
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Department] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Department]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_UserEducation] FOREIGN KEY([EducationID])
REFERENCES [dbo].[UserEducation] ([UserEduID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserEducation]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_UserPosition] FOREIGN KEY([PositionID])
REFERENCES [dbo].[UserPosition] ([PositionID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserPosition]
GO
USE [master]
GO
ALTER DATABASE [HospitalManagement] SET  READ_WRITE 
GO
