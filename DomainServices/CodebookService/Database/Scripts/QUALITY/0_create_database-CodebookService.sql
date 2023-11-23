USE [master]
GO

/****** Object:  Database [CodebookService]    Script Date: 05.04.2022 0:43:50 ******/
CREATE DATABASE [CodebookService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CodebookService', FILENAME = N'H:\MSSQL15.QUALITY\MSSQL\Data_Silver_18_QUA\CodebookService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CodebookService_log', FILENAME = N'H:\MSSQL15.QUALITY\MSSQL\Data_Silver_16_QUA\CodebookService_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CodebookService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [CodebookService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [CodebookService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [CodebookService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [CodebookService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [CodebookService] SET ARITHABORT OFF 
GO

ALTER DATABASE [CodebookService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [CodebookService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [CodebookService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [CodebookService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [CodebookService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [CodebookService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [CodebookService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [CodebookService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [CodebookService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [CodebookService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [CodebookService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [CodebookService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [CodebookService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [CodebookService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [CodebookService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [CodebookService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [CodebookService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [CodebookService] SET RECOVERY FULL 
GO

ALTER DATABASE [CodebookService] SET  MULTI_USER 
GO

ALTER DATABASE [CodebookService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [CodebookService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [CodebookService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [CodebookService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [CodebookService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [CodebookService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [CodebookService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [CodebookService] SET  READ_WRITE 
GO


