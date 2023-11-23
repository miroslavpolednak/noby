USE [master]
GO

/****** Object:  Database [DocumentArchiveService]    Script Date: 19.01.2023 1:26:26 ******/
CREATE DATABASE [DocumentArchiveService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DocumentArchiveService', FILENAME = N'H:\MSSQL15.QUALITY\MSSQL\Data_Silver_18_QUA\DocumentArchiveService.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DocumentArchiveService_log', FILENAME = N'H:\MSSQL15.QUALITY\MSSQL\Data_Silver_16_QUA\DocumentArchiveService_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DocumentArchiveService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [DocumentArchiveService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET ARITHABORT OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [DocumentArchiveService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [DocumentArchiveService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [DocumentArchiveService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [DocumentArchiveService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET RECOVERY FULL 
GO

ALTER DATABASE [DocumentArchiveService] SET  MULTI_USER 
GO

ALTER DATABASE [DocumentArchiveService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [DocumentArchiveService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [DocumentArchiveService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [DocumentArchiveService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [DocumentArchiveService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [DocumentArchiveService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [DocumentArchiveService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [DocumentArchiveService] SET  READ_WRITE 
GO


