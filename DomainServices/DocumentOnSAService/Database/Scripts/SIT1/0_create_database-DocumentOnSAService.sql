USE [master]
GO

/****** Object:  Database [DocumentOnSAService]    Script Date: 01.04.2023 0:05:04 ******/
CREATE DATABASE [DocumentOnSAService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DocumentOnSAService', FILENAME = N'F:\MSSQL15.SIT\MSSQL\DATA\DocumentOnSAService.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DocumentOnSAService_log', FILENAME = N'G:\MSSQL15.SIT\MSSQL\Data\DocumentOnSAService_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DocumentOnSAService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [DocumentOnSAService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET ARITHABORT OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [DocumentOnSAService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [DocumentOnSAService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [DocumentOnSAService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [DocumentOnSAService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET RECOVERY FULL 
GO

ALTER DATABASE [DocumentOnSAService] SET  MULTI_USER 
GO

ALTER DATABASE [DocumentOnSAService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [DocumentOnSAService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [DocumentOnSAService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [DocumentOnSAService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [DocumentOnSAService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [DocumentOnSAService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [DocumentOnSAService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [DocumentOnSAService] SET  READ_WRITE 
GO


