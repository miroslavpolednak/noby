USE [master]
GO

/****** Object:  Database [NotificationService]    Script Date: 01.04.2023 0:05:43 ******/
CREATE DATABASE [NotificationService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotificationService', FILENAME = N'S:\Data_Silver_18_SQL\NotificationService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotificationService_log', FILENAME = N'S:\Data_Silver_16_SQL\NotificationService_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotificationService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [NotificationService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [NotificationService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [NotificationService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [NotificationService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [NotificationService] SET ARITHABORT OFF 
GO

ALTER DATABASE [NotificationService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [NotificationService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [NotificationService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [NotificationService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [NotificationService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [NotificationService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [NotificationService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [NotificationService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [NotificationService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [NotificationService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [NotificationService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [NotificationService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [NotificationService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [NotificationService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [NotificationService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [NotificationService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [NotificationService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [NotificationService] SET RECOVERY FULL 
GO

ALTER DATABASE [NotificationService] SET  MULTI_USER 
GO

ALTER DATABASE [NotificationService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [NotificationService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [NotificationService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [NotificationService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [NotificationService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [NotificationService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [NotificationService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [NotificationService] SET  READ_WRITE 
GO


