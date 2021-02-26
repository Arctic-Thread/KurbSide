USE [master]
GO
/****** Object:  Database [KurbSide]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE DATABASE [KurbSide]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KurbSide', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\KurbSide.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'KurbSide_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\KurbSide_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [KurbSide] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KurbSide].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KurbSide] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KurbSide] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KurbSide] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KurbSide] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KurbSide] SET ARITHABORT OFF 
GO
ALTER DATABASE [KurbSide] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [KurbSide] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KurbSide] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KurbSide] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KurbSide] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KurbSide] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KurbSide] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KurbSide] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KurbSide] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KurbSide] SET  ENABLE_BROKER 
GO
ALTER DATABASE [KurbSide] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KurbSide] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KurbSide] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KurbSide] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KurbSide] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KurbSide] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KurbSide] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KurbSide] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [KurbSide] SET  MULTI_USER 
GO
ALTER DATABASE [KurbSide] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KurbSide] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KurbSide] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KurbSide] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [KurbSide] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [KurbSide] SET QUERY_STORE = OFF
GO
USE [KurbSide]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountSettings]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountSettings](
	[AspNetId] [nvarchar](450) NOT NULL,
	[TimeZone] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AspNetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Business]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Business](
	[AspNetId] [nvarchar](450) NOT NULL,
	[BusinessId] [uniqueidentifier] NOT NULL,
	[BusinessName] [nvarchar](255) NOT NULL,
	[PhoneNumber] [nvarchar](22) NOT NULL,
	[OpenTime] [time](7) NULL,
	[CloseTime] [time](7) NULL,
	[Street] [nvarchar](255) NOT NULL,
	[StreetLn2] [nvarchar](255) NULL,
	[City] [nvarchar](60) NOT NULL,
	[Postal] [nvarchar](10) NOT NULL,
	[ProvinceCode] [nvarchar](2) NULL,
	[CountryCode] [nvarchar](2) NULL,
	[BusinessNumber] [nvarchar](255) NULL,
	[ContactPhone] [nvarchar](22) NOT NULL,
	[ContactFirst] [nvarchar](100) NOT NULL,
	[ContactLast] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AspNetId] ASC,
	[BusinessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusinessHours]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessHours](
	[BusinessId] [uniqueidentifier] NOT NULL,
	[MonOpen] [time](7) NULL,
	[MonClose] [time](7) NULL,
	[TuesOpen] [time](7) NULL,
	[TuesClose] [time](7) NULL,
	[WedOpen] [time](7) NULL,
	[WedClose] [time](7) NULL,
	[ThuOpen] [time](7) NULL,
	[ThuClose] [time](7) NULL,
	[FriOpen] [time](7) NULL,
	[FriClose] [time](7) NULL,
	[SatOpen] [time](7) NULL,
	[SatClose] [time](7) NULL,
	[SunOpen] [time](7) NULL,
	[SunClose] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[BusinessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryCode] [nvarchar](2) NOT NULL,
	[FullName] [nvarchar](60) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [uniqueidentifier] NOT NULL,
	[BusinessId] [uniqueidentifier] NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
	[Details] [nvarchar](500) NULL,
	[Price] [float] NULL,
	[SKU] [nvarchar](50) NULL,
	[UPC] [nvarchar](50) NULL,
	[ImageLocation] [nvarchar](255) NULL,
	[Category] [nvarchar](50) NULL,
	[Removed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC,
	[BusinessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[AspNetId] [nvarchar](450) NOT NULL,
	[MemberId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AspNetId] ASC,
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Province]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Province](
	[ProvinceCode] [nvarchar](2) NOT NULL,
	[CountryCode] [nvarchar](2) NOT NULL,
	[FullName] [nvarchar](60) NOT NULL,
	[TaxRate] [float] NULL,
	[TaxCode] [nvarchar](5) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProvinceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeZones]    Script Date: 2021-02-25 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeZones](
	[TimeZoneID] [uniqueidentifier] NOT NULL,
	[Offset] [varchar](25) NOT NULL,
	[Label] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TimeZoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1461b282-1370-48f3-b1cf-d6e209b9055a', N'test@kurbsi.de', N'TEST@KURBSI.DE', N'test@kurbsi.de', N'TEST@KURBSI.DE', 1, N'AQAAAAEAACcQAAAAEBanqm80P3sgAsUU6wODfmTuxjs8Bu/A1904qDU37BfGvqZBxnD/2RYF+2dDJiw5eQ==', N'BNPXHVD65SASAR27V54VIBY5ZS4N7GP3', N'0a346b0e-a561-44bc-91b3-745c991d5d41', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'44d8f59e-23ff-401d-8932-5d4c3fe22b0e', N'member@kurbsi.de', N'MEMBER@KURBSI.DE', N'member@kurbsi.de', N'MEMBER@KURBSI.DE', 1, N'AQAAAAEAACcQAAAAEGNg9+nf+f01ixXEJokyLtllC1WeGl8Z8jEFw5QbNol/mIAY9GIJpDq7aybAI+AmAg==', N'LONYPQHQSH3TALVX5OXNFWDLBAQYM5JN', N'c0c63736-8f38-47ac-ad99-d7c525a2a21c', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'f9dcdab4-083d-409c-b7d2-3e149c5f35f7', N'business@kurbsi.de', N'BUSINESS@KURBSI.DE', N'business@kurbsi.de', N'BUSINESS@KURBSI.DE', 1, N'AQAAAAEAACcQAAAAEAqBq621vjdG/y7cj6D9iQBOIRT990O9yMqCmDgFcG8Jxjk+WiwTrH7wyI6lmEbrfg==', N'PJINFKG6KS3LIG3QXEMYWP6DQJJKMMJG', N'1c43dcd2-09ad-48c2-8770-7ae77dcbb104', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Business] ([AspNetId], [BusinessId], [BusinessName], [PhoneNumber], [OpenTime], [CloseTime], [Street], [StreetLn2], [City], [Postal], [ProvinceCode], [CountryCode], [BusinessNumber], [ContactPhone], [ContactFirst], [ContactLast]) VALUES (N'1461b282-1370-48f3-b1cf-d6e209b9055a', N'b44daa0f-c19a-42bb-a782-28248db54386', N'Test Business', N'1231231234', CAST(N'20:09:18.3533333' AS Time), CAST(N'20:09:18.3533333' AS Time), N'Test Address', NULL, N'Waterloo', N'N2K 2M2', N'ON', N'CA', N'9238492384', N'1231231234', N'Steve', N'Wozniak')
INSERT [dbo].[Business] ([AspNetId], [BusinessId], [BusinessName], [PhoneNumber], [OpenTime], [CloseTime], [Street], [StreetLn2], [City], [Postal], [ProvinceCode], [CountryCode], [BusinessNumber], [ContactPhone], [ContactFirst], [ContactLast]) VALUES (N'f9dcdab4-083d-409c-b7d2-3e149c5f35f7', N'7d49352f-5611-471d-ad53-a155a368b6f2', N'Microsoft', N'8775682495', NULL, NULL, N'Microsoft Building 92', NULL, N'Redmond', N'N2K2M2', N'ON', N'CA', N'123456789', N'8775682495', N'Bill', N'Gates')
GO
INSERT [dbo].[BusinessHours] ([BusinessId], [MonOpen], [MonClose], [TuesOpen], [TuesClose], [WedOpen], [WedClose], [ThuOpen], [ThuClose], [FriOpen], [FriClose], [SatOpen], [SatClose], [SunOpen], [SunClose]) VALUES (N'b44daa0f-c19a-42bb-a782-28248db54386', CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time), CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time), CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time), CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time), CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time), CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time), CAST(N'01:23:00' AS Time), CAST(N'13:23:00' AS Time))
INSERT [dbo].[BusinessHours] ([BusinessId], [MonOpen], [MonClose], [TuesOpen], [TuesClose], [WedOpen], [WedClose], [ThuOpen], [ThuClose], [FriOpen], [FriClose], [SatOpen], [SatClose], [SunOpen], [SunClose]) VALUES (N'7d49352f-5611-471d-ad53-a155a368b6f2', CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'06:00:00' AS Time), CAST(N'16:00:00' AS Time))
GO
INSERT [dbo].[Country] ([CountryCode], [FullName]) VALUES (N'CA', N'Canada')
GO
INSERT [dbo].[Item] ([ItemId], [BusinessId], [ItemName], [Details], [Price], [SKU], [UPC], [ImageLocation], [Category], [Removed]) VALUES (N'bcd0b5b3-c417-4e07-83ad-14c81b47a294', N'7d49352f-5611-471d-ad53-a155a368b6f2', N'Microsoft Surface', NULL, 123, NULL, N'12345678910', NULL, N'PC', 0)
GO
INSERT [dbo].[Province] ([ProvinceCode], [CountryCode], [FullName], [TaxRate], [TaxCode]) VALUES (N'ON', N'CA', N'ONTARIO', 0.13, N'HST')
GO
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'043406d6-2884-4e54-926b-052c219f5c38', N'+00:00', N'(GMT) Western Europe Time, London, Lisbon, Casablanca')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'3761f075-bc26-4878-b70d-13c081660742', N'+10:50', N'(GMT +10:30) Lord Howe Island')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'46f80240-9775-40ba-9ba4-1519a7bb04ea', N'+11:00', N'(GMT +11:00) Magadan, Solomon Islands, New Caledonia')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'21ee0cfa-0c90-4cc3-ac0d-1561cc97f8c0', N'-03:00', N'(GMT -3:00) Brazil, Buenos Aires, Georgetown')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'd28ac79d-4829-4b1e-8b4e-161c65ac5492', N'-02:00', N'(GMT -2:00) Mid-Atlantic')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'd4d66784-c477-4ed7-9269-16ea61c7d81f', N'-07:00', N'(GMT -7:00) Mountain Time (US &amp; Canada)')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'90aaa6e4-befa-4d25-a977-179ad0ff9dd9', N'+04:50', N'(GMT +4:30) Kabul')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'58107db9-8a3d-4d76-b964-1826b70090b9', N'-04:00', N'(GMT -4:00) Atlantic Time (Canada), Caracas, La Paz')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'ee96272f-1db9-460e-a8e9-19147c7ce166', N'+10:00', N'(GMT +10:00) Eastern Australia, Guam, Vladivostok')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'4cb52b2b-9ae1-4031-94b6-197c10145e14', N'+04:00', N'(GMT +4:00) Abu Dhabi, Muscat, Baku, Tbilisi')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'6d2c0d9c-d36d-4c28-ad20-1a9d9cfdb7b9', N'+07:00', N'(GMT +7:00) Bangkok, Hanoi, Jakarta')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'2357bc8d-ce51-457d-9319-348295ed64bc', N'+09:00', N'(GMT +9:00) Tokyo, Seoul, Osaka, Sapporo, Yakutsk')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'7cebf0f3-c35f-4c78-b2f9-3680f1c41ca8', N'-12:00', N'(GMT -12:00) Eniwetok, Kwajalein')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'058c274d-5eaf-43ba-a73f-3d834a3ff0c0', N'-04:50', N'(GMT -4:30) Caracas')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'1e9d6dea-1184-4dd2-aaf7-465f392e4c52', N'-10:00', N'(GMT -10:00) Hawaii')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'e2eea908-2287-4dee-b5fc-6043a1f0dde8', N'-06:00', N'(GMT -6:00) Central Time (US &amp; Canada), Mexico City')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'fd50ef86-136e-468c-95b5-6b3ab63f8fe4', N'+03:00', N'(GMT +3:00) Baghdad, Riyadh, Moscow, St. Petersburg')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'2be5336a-41cc-41de-b6bf-6de06ea49a9c', N'-11:00', N'(GMT -11:00) Midway Island, Samoa')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'a579aa3a-7da9-49f8-b251-724d5501c819', N'+01:00', N'(GMT +1:00) Brussels, Copenhagen, Madrid, Paris')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'3b0651ac-5b3f-4f2c-b4f0-7786d65d8159', N'+08:00', N'(GMT +8:00) Beijing, Perth, Singapore, Hong Kong')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'ae43cb56-ca3f-445b-8601-7bd6d3451a89', N'+12:00', N'(GMT +12:00) Auckland, Wellington, Fiji, Kamchatka')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'93f64cb0-b484-46d4-83c8-82360236ee40', N'+11:50', N'(GMT +11:30) Norfolk Island')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'e39367c0-d54f-4183-bb45-8390ebc215ab', N'-09:00', N'(GMT -9:00) Alaska')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'98716a1b-1a5f-4765-8d9f-896c4ad61b1d', N'-09:50', N'(GMT -9:30) Taiohae')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'630ce6f5-6cd3-443b-b60e-951facdc1f8a', N'-01:00', N'(GMT -1:00) Azores, Cape Verde Islands')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'45bc2a1b-2436-4d59-adf8-9b68006e7e75', N'-08:00', N'(GMT -8:00) Pacific Time (US &amp; Canada)')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'6448fa61-59be-40fd-899a-ac5bce772894', N'+05:50', N'(GMT +5:30) Bombay, Calcutta, Madras, New Delhi')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'ab9c0f97-02aa-42c0-92ee-b3714fe622bc', N'-05:00', N'(GMT -5:00) Eastern Time (US &amp; Canada), Bogota, Lima')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'af3c693b-7e43-4adb-a742-b5aff358b3d6', N'-03:50', N'(GMT -3:30) Newfoundland')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'217c6558-2dca-4c4e-aee4-b609e61a3616', N'+06:50', N'(GMT +6:30) Yangon, Mandalay')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'83b1cd28-eb7b-4f7c-84e9-b621bc738c21', N'+14:00', N'(GMT +14:00) Line Islands, Tokelau')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'8a5dd2a7-5a53-434b-886a-bc95647fcc7e', N'+05:75', N'(GMT +5:45) Kathmandu, Pokhar')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'e456eaed-baaa-4aea-be6f-bce70813899d', N'+13:00', N'(GMT +13:00) Apia, Nukualofa')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'313b6491-aa9a-427b-a9ad-c1ccf3b7119e', N'+08:75', N'(GMT +8:45) Eucla')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'693937b6-c1f7-4245-9281-c9592f01940c', N'+05:00', N'(GMT +5:00) Ekaterinburg, Islamabad, Karachi, Tashkent')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'aaf03d20-a78c-4a82-9621-d577b23de757', N'+12:75', N'(GMT +12:45) Chatham Islands')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'f538908b-6b49-437b-91f2-e09ab45ecd6a', N'+09:50', N'(GMT +9:30) Adelaide, Darwin')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'26217338-5a05-461a-b218-f3850258df4d', N'+03:50', N'(GMT +3:30) Tehran')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'73ed3dd3-2dc2-4fd8-9cf1-f9bedcae55dd', N'+06:00', N'(GMT +6:00) Almaty, Dhaka, Colombo')
INSERT [dbo].[TimeZones] ([TimeZoneID], [Offset], [Label]) VALUES (N'acb3ddc0-3317-4796-9c4f-fd5847e41cdc', N'+02:00', N'(GMT +2:00) Kaliningrad, South Africa')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 2021-02-25 8:16:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [PK_Business_Unique]    Script Date: 2021-02-25 8:16:14 PM ******/
ALTER TABLE [dbo].[Business] ADD  CONSTRAINT [PK_Business_Unique] UNIQUE NONCLUSTERED 
(
	[BusinessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [PK_Member_Unique]    Script Date: 2021-02-25 8:16:14 PM ******/
ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [PK_Member_Unique] UNIQUE NONCLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountSettings] ADD  DEFAULT ((9)) FOR [TimeZone]
GO
ALTER TABLE [dbo].[Business] ADD  DEFAULT (newid()) FOR [BusinessId]
GO
ALTER TABLE [dbo].[Business] ADD  DEFAULT (getdate()) FOR [OpenTime]
GO
ALTER TABLE [dbo].[Business] ADD  DEFAULT (getdate()) FOR [CloseTime]
GO
ALTER TABLE [dbo].[Business] ADD  DEFAULT ('ON') FOR [ProvinceCode]
GO
ALTER TABLE [dbo].[Business] ADD  DEFAULT ('CA') FOR [CountryCode]
GO
ALTER TABLE [dbo].[Item] ADD  DEFAULT (newid()) FOR [ItemId]
GO
ALTER TABLE [dbo].[Item] ADD  DEFAULT ('FALSE') FOR [Removed]
GO
ALTER TABLE [dbo].[Member] ADD  DEFAULT (newid()) FOR [MemberId]
GO
ALTER TABLE [dbo].[Province] ADD  DEFAULT ((0)) FOR [TaxRate]
GO
ALTER TABLE [dbo].[Province] ADD  DEFAULT ('') FOR [TaxCode]
GO
ALTER TABLE [dbo].[TimeZones] ADD  DEFAULT (newid()) FOR [TimeZoneID]
GO
ALTER TABLE [dbo].[AccountSettings]  WITH CHECK ADD FOREIGN KEY([AspNetId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD FOREIGN KEY([AspNetId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD FOREIGN KEY([CountryCode])
REFERENCES [dbo].[Country] ([CountryCode])
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD FOREIGN KEY([ProvinceCode])
REFERENCES [dbo].[Province] ([ProvinceCode])
GO
ALTER TABLE [dbo].[BusinessHours]  WITH CHECK ADD FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([BusinessId])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([BusinessId])
GO
ALTER TABLE [dbo].[Member]  WITH CHECK ADD FOREIGN KEY([AspNetId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Province]  WITH CHECK ADD FOREIGN KEY([CountryCode])
REFERENCES [dbo].[Country] ([CountryCode])
GO
USE [master]
GO
ALTER DATABASE [KurbSide] SET  READ_WRITE 
GO
