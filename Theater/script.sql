USE [master]
GO
/****** Object:  Database [Theater]    Script Date: 11.06.2024 15:44:17 ******/
CREATE DATABASE [Theater]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Theater', FILENAME = N'C:\Users\Webmaster1\Theater.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Theater_log', FILENAME = N'C:\Users\Webmaster1\Theater_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Theater] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Theater].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Theater] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Theater] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Theater] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Theater] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Theater] SET ARITHABORT OFF 
GO
ALTER DATABASE [Theater] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Theater] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Theater] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Theater] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Theater] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Theater] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Theater] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Theater] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Theater] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Theater] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Theater] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Theater] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Theater] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Theater] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Theater] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Theater] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Theater] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Theater] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Theater] SET  MULTI_USER 
GO
ALTER DATABASE [Theater] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Theater] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Theater] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Theater] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Theater] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Theater] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Theater] SET QUERY_STORE = OFF
GO
USE [Theater]
GO
/****** Object:  Table [dbo].[actors]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[actors](
	[id_actor] [int] NOT NULL,
	[fio] [nvarchar](50) NOT NULL,
	[age] [int] NOT NULL,
	[sex] [nvarchar](1) NOT NULL,
	[height] [int] NOT NULL,
	[weight] [int] NOT NULL,
	[contact_details] [nvarchar](100) NOT NULL,
	[id_photo] [int] NOT NULL,
 CONSTRAINT [PK_Actors] PRIMARY KEY CLUSTERED 
(
	[id_actor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[actors_role]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[actors_role](
	[id_ar] [int] NOT NULL,
	[id_actor] [int] NOT NULL,
	[id_role] [int] NOT NULL,
 CONSTRAINT [PK_actors_role] PRIMARY KEY CLUSTERED 
(
	[id_ar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[basket]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[basket](
	[id_basket] [int] IDENTITY(1,1) NOT NULL,
	[id_viewer] [int] NOT NULL,
	[id_showtime] [int] NOT NULL,
	[quantity] [int] NOT NULL,
 CONSTRAINT [PK_basket] PRIMARY KEY CLUSTERED 
(
	[id_basket] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory](
	[id_inventory] [int] NOT NULL,
	[id_iin] [int] NOT NULL,
	[id_theater_building] [int] NOT NULL,
 CONSTRAINT [PK_inventory] PRIMARY KEY CLUSTERED 
(
	[id_inventory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_item_name]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory_item_name](
	[id_iin] [int] NOT NULL,
	[name] [nchar](50) NOT NULL,
 CONSTRAINT [PK_inventory_item_name] PRIMARY KEY CLUSTERED 
(
	[id_iin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[performance]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[performance](
	[id_performance] [int] NOT NULL,
	[title] [nvarchar](250) NOT NULL,
	[genre] [nvarchar](50) NOT NULL,
	[year_created] [int] NOT NULL,
	[author] [nvarchar](100) NOT NULL,
	[duration] [time](7) NOT NULL,
	[id_photo] [int] NOT NULL,
 CONSTRAINT [PK_Performance] PRIMARY KEY CLUSTERED 
(
	[id_performance] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[photo]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[photo](
	[id_photo] [int] IDENTITY(1,1) NOT NULL,
	[description] [text] NOT NULL,
	[photo1] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_photo] PRIMARY KEY CLUSTERED 
(
	[id_photo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[review]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[review](
	[id_review] [int] IDENTITY(1,1) NOT NULL,
	[critic_name] [nvarchar](100) NOT NULL,
	[review] [text] NOT NULL,
	[rating] [float] NOT NULL,
	[id_showtime] [int] NOT NULL,
 CONSTRAINT [PK_review] PRIMARY KEY CLUSTERED 
(
	[id_review] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role](
	[id_role] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[id_performance] [int] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[id_role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[showtime]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[showtime](
	[id_showtime] [int] IDENTITY(1,1) NOT NULL,
	[id_performanсe] [int] NOT NULL,
	[id_photo] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
	[price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_showtime] PRIMARY KEY CLUSTERED 
(
	[id_showtime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sponsor]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sponsor](
	[id_sponsor] [int] IDENTITY(1,1) NOT NULL,
	[company_name] [nvarchar](50) NOT NULL,
	[contact_info] [nvarchar](200) NOT NULL,
	[id_photo] [int] NOT NULL,
 CONSTRAINT [PK_sponsor] PRIMARY KEY CLUSTERED 
(
	[id_sponsor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[theater]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[theater](
	[id_theater_building] [int] IDENTITY(1,1) NOT NULL,
	[address] [nvarchar](100) NOT NULL,
	[area] [float] NOT NULL,
	[seats_number] [int] NOT NULL,
	[year_built] [int] NOT NULL,
	[stage_area] [int] NOT NULL,
 CONSTRAINT [PK_theater_building] PRIMARY KEY CLUSTERED 
(
	[id_theater_building] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tickets]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tickets](
	[id_ticket] [int] IDENTITY(1,1) NOT NULL,
	[id_showtime] [int] NOT NULL,
	[seat_row] [int] NOT NULL,
	[seat_number] [int] NOT NULL,
	[price] [float] NOT NULL,
	[date_sold] [date] NOT NULL,
	[id_viewer] [int] NOT NULL,
	[id_theater_building] [int] NOT NULL,
 CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED 
(
	[id_ticket] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_role]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_role](
	[id_user_role] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[id_user_role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](50) NOT NULL,
	[pass] [nvarchar](50) NOT NULL,
	[id_user_role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[viewer]    Script Date: 11.06.2024 15:44:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[viewer](
	[id_viewer] [int] IDENTITY(1,1) NOT NULL,
	[fio] [nvarchar](50) NOT NULL,
	[contact_details] [nvarchar](200) NOT NULL,
	[id_user] [int] NOT NULL,
 CONSTRAINT [PK_Viewer] PRIMARY KEY CLUSTERED 
(
	[id_viewer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (3, N'Иванов И.А.', 20, N'м', 180, 70, N'+79117214623', 8)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (4, N'Петров И.В.', 30, N'м', 190, 80, N'+79312746324', 11)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (6, N'Сидоров И.С.', 23, N'м', 178, 65, N'+79216235142', 12)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (9, N'Андреев И.Д.', 32, N'м', 160, 58, N'+79123264818', 5)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (10, N'Алексеева И.К.', 25, N'ж', 172, 62, N'+79324123723', 3)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (11, N'Михайлова И.Ф.', 29, N'ж', 160, 50, N'+79213742732', 9)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (12, N'Новикова И.Л.', 37, N'ж', 163, 51, N'+79214623244', 10)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (13, N'Федорова И.М.', 31, N'ж', 168, 53, N'+79412364362', 7)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (14, N'Яковлева И.Н.', 27, N'ж', 158, 49, N'+79123743625', 1003)
INSERT [dbo].[actors] ([id_actor], [fio], [age], [sex], [height], [weight], [contact_details], [id_photo]) VALUES (15, N'Егорова И.О.', 23, N'ж', 164, 50, N'+79214232354', 6)
GO
INSERT [dbo].[performance] ([id_performance], [title], [genre], [year_created], [author], [duration], [id_photo]) VALUES (1, N'Мастер и Маргарита', N'драма', 1977, N'Михаил Булгаков', CAST(N'03:30:00' AS Time), 2)
INSERT [dbo].[performance] ([id_performance], [title], [genre], [year_created], [author], [duration], [id_photo]) VALUES (2, N'Идиот', N'драма', 1899, N'Фёдор Михайлович Достоевский', CAST(N'02:50:00' AS Time), 1)
GO
SET IDENTITY_INSERT [dbo].[photo] ON 

INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (1, N'idiot', N'idiot.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (2, N'master and margarita', N'master_and_margarita.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (3, N'alekseeva', N'alekseeva.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (5, N'andreev', N'andreev.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (6, N'egorova', N'egorova.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (7, N'fedorova', N'fedorova.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (8, N'ivanov', N'ivanov.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (9, N'michailova', N'michailova.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (10, N'novikova', N'novikova.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (11, N'petrov', N'petrov.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (12, N'sidorov', N'sidorov.jpg')
INSERT [dbo].[photo] ([id_photo], [description], [photo1]) VALUES (1003, N'yakovleva', N'yakovleva.jpg')
SET IDENTITY_INSERT [dbo].[photo] OFF
GO
SET IDENTITY_INSERT [dbo].[showtime] ON 

INSERT [dbo].[showtime] ([id_showtime], [id_performanсe], [id_photo], [date], [price]) VALUES (1, 1, 2, CAST(N'2024-06-07T19:00:00.0000000' AS DateTime2), CAST(1000.00 AS Decimal(18, 2)))
INSERT [dbo].[showtime] ([id_showtime], [id_performanсe], [id_photo], [date], [price]) VALUES (2, 2, 1, CAST(N'2024-06-22T18:00:00.0000000' AS DateTime2), CAST(1500.00 AS Decimal(18, 2)))
INSERT [dbo].[showtime] ([id_showtime], [id_performanсe], [id_photo], [date], [price]) VALUES (4, 2, 1, CAST(N'2024-07-10T18:00:00.0000000' AS DateTime2), CAST(1500.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[showtime] OFF
GO
SET IDENTITY_INSERT [dbo].[user_role] ON 

INSERT [dbo].[user_role] ([id_user_role], [name]) VALUES (1, N'admin')
INSERT [dbo].[user_role] ([id_user_role], [name]) VALUES (2, N'viewer')
INSERT [dbo].[user_role] ([id_user_role], [name]) VALUES (3, N'employee')
SET IDENTITY_INSERT [dbo].[user_role] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (1, N'ivanov_ia', N'Ivanov20', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (2, N'petrov_iv', N'Petrov30', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (3, N'sidorov_is', N'Sidorov23', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (4, N'andreev_id', N'Andreev32', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (17, N'alekseeva_ik', N'Alekseeva25', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (18, N'mikhailova_if', N'Mikhailova29', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (19, N'novikova_il', N'Novikova37', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (20, N'fedorova_im', N'Fedorova31', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (21, N'yakovleva_in', N'Yakovleva27', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (22, N'egorova_io', N'Egorova23', 3)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (23, N'ilya', N'123', 2)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (1023, N'ilyailya', N'12341234', 2)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (2023, N'admin', N'admin', 1)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (3023, N'masha', N'1234567890', 2)
INSERT [dbo].[users] ([id_user], [login], [pass], [id_user_role]) VALUES (3024, N'adm', N'adm', 1)
SET IDENTITY_INSERT [dbo].[users] OFF
GO
SET IDENTITY_INSERT [dbo].[viewer] ON 

INSERT [dbo].[viewer] ([id_viewer], [fio], [contact_details], [id_user]) VALUES (1, N'Гаффаров И.Д.', N'+7932423432', 23)
INSERT [dbo].[viewer] ([id_viewer], [fio], [contact_details], [id_user]) VALUES (2, N'Ilya', N'+76352763535', 1023)
INSERT [dbo].[viewer] ([id_viewer], [fio], [contact_details], [id_user]) VALUES (1002, N'мария', N'89650184529', 3023)
SET IDENTITY_INSERT [dbo].[viewer] OFF
GO
ALTER TABLE [dbo].[actors]  WITH CHECK ADD  CONSTRAINT [FK_actors_photo] FOREIGN KEY([id_photo])
REFERENCES [dbo].[photo] ([id_photo])
GO
ALTER TABLE [dbo].[actors] CHECK CONSTRAINT [FK_actors_photo]
GO
ALTER TABLE [dbo].[actors_role]  WITH CHECK ADD  CONSTRAINT [FK_actors_role_actors] FOREIGN KEY([id_actor])
REFERENCES [dbo].[actors] ([id_actor])
GO
ALTER TABLE [dbo].[actors_role] CHECK CONSTRAINT [FK_actors_role_actors]
GO
ALTER TABLE [dbo].[actors_role]  WITH CHECK ADD  CONSTRAINT [FK_actors_role_role] FOREIGN KEY([id_role])
REFERENCES [dbo].[role] ([id_role])
GO
ALTER TABLE [dbo].[actors_role] CHECK CONSTRAINT [FK_actors_role_role]
GO
ALTER TABLE [dbo].[basket]  WITH CHECK ADD  CONSTRAINT [FK_basket_showtime] FOREIGN KEY([id_showtime])
REFERENCES [dbo].[showtime] ([id_showtime])
GO
ALTER TABLE [dbo].[basket] CHECK CONSTRAINT [FK_basket_showtime]
GO
ALTER TABLE [dbo].[basket]  WITH CHECK ADD  CONSTRAINT [FK_basket_viewer] FOREIGN KEY([id_viewer])
REFERENCES [dbo].[viewer] ([id_viewer])
GO
ALTER TABLE [dbo].[basket] CHECK CONSTRAINT [FK_basket_viewer]
GO
ALTER TABLE [dbo].[inventory]  WITH CHECK ADD  CONSTRAINT [FK_inventory_inventory_item_name] FOREIGN KEY([id_iin])
REFERENCES [dbo].[inventory_item_name] ([id_iin])
GO
ALTER TABLE [dbo].[inventory] CHECK CONSTRAINT [FK_inventory_inventory_item_name]
GO
ALTER TABLE [dbo].[inventory]  WITH CHECK ADD  CONSTRAINT [FK_inventory_theater] FOREIGN KEY([id_theater_building])
REFERENCES [dbo].[theater] ([id_theater_building])
GO
ALTER TABLE [dbo].[inventory] CHECK CONSTRAINT [FK_inventory_theater]
GO
ALTER TABLE [dbo].[performance]  WITH CHECK ADD  CONSTRAINT [FK_performance_photo] FOREIGN KEY([id_photo])
REFERENCES [dbo].[photo] ([id_photo])
GO
ALTER TABLE [dbo].[performance] CHECK CONSTRAINT [FK_performance_photo]
GO
ALTER TABLE [dbo].[review]  WITH CHECK ADD  CONSTRAINT [FK_review_showtime] FOREIGN KEY([id_showtime])
REFERENCES [dbo].[showtime] ([id_showtime])
GO
ALTER TABLE [dbo].[review] CHECK CONSTRAINT [FK_review_showtime]
GO
ALTER TABLE [dbo].[role]  WITH CHECK ADD  CONSTRAINT [FK_role_performance] FOREIGN KEY([id_performance])
REFERENCES [dbo].[performance] ([id_performance])
GO
ALTER TABLE [dbo].[role] CHECK CONSTRAINT [FK_role_performance]
GO
ALTER TABLE [dbo].[showtime]  WITH CHECK ADD  CONSTRAINT [FK_showtime_performance] FOREIGN KEY([id_performanсe])
REFERENCES [dbo].[performance] ([id_performance])
GO
ALTER TABLE [dbo].[showtime] CHECK CONSTRAINT [FK_showtime_performance]
GO
ALTER TABLE [dbo].[showtime]  WITH CHECK ADD  CONSTRAINT [FK_showtime_photo] FOREIGN KEY([id_photo])
REFERENCES [dbo].[photo] ([id_photo])
GO
ALTER TABLE [dbo].[showtime] CHECK CONSTRAINT [FK_showtime_photo]
GO
ALTER TABLE [dbo].[sponsor]  WITH CHECK ADD  CONSTRAINT [FK_sponsor_photo] FOREIGN KEY([id_photo])
REFERENCES [dbo].[photo] ([id_photo])
GO
ALTER TABLE [dbo].[sponsor] CHECK CONSTRAINT [FK_sponsor_photo]
GO
ALTER TABLE [dbo].[tickets]  WITH CHECK ADD  CONSTRAINT [FK_tickets_showtime] FOREIGN KEY([id_showtime])
REFERENCES [dbo].[showtime] ([id_showtime])
GO
ALTER TABLE [dbo].[tickets] CHECK CONSTRAINT [FK_tickets_showtime]
GO
ALTER TABLE [dbo].[tickets]  WITH CHECK ADD  CONSTRAINT [FK_tickets_theater] FOREIGN KEY([id_theater_building])
REFERENCES [dbo].[theater] ([id_theater_building])
GO
ALTER TABLE [dbo].[tickets] CHECK CONSTRAINT [FK_tickets_theater]
GO
ALTER TABLE [dbo].[tickets]  WITH CHECK ADD  CONSTRAINT [FK_tickets_viewer] FOREIGN KEY([id_viewer])
REFERENCES [dbo].[viewer] ([id_viewer])
GO
ALTER TABLE [dbo].[tickets] CHECK CONSTRAINT [FK_tickets_viewer]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_Users_user_role] FOREIGN KEY([id_user_role])
REFERENCES [dbo].[user_role] ([id_user_role])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK_Users_user_role]
GO
ALTER TABLE [dbo].[viewer]  WITH CHECK ADD  CONSTRAINT [FK_viewer_Users] FOREIGN KEY([id_user])
REFERENCES [dbo].[users] ([id_user])
GO
ALTER TABLE [dbo].[viewer] CHECK CONSTRAINT [FK_viewer_Users]
GO
USE [master]
GO
ALTER DATABASE [Theater] SET  READ_WRITE 
GO
