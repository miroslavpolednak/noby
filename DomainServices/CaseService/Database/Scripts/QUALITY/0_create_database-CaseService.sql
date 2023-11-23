USE [master]
GO

/****** Object:  Database [CaseService]    Script Date: 05.04.2022 0:37:57 ******/
CREATE DATABASE [CaseService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CaseService', FILENAME = N'H:\MSSQL15.QUALITY\MSSQL\Data_Silver_18_QUA\CaseService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CaseService_log', FILENAME = N'H:\MSSQL15.QUALITY\MSSQL\Data_Silver_16_QUA\CaseService_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CaseService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [CaseService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [CaseService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [CaseService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [CaseService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [CaseService] SET ARITHABORT OFF 
GO

ALTER DATABASE [CaseService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [CaseService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [CaseService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [CaseService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [CaseService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [CaseService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [CaseService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [CaseService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [CaseService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [CaseService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [CaseService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [CaseService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [CaseService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [CaseService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [CaseService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [CaseService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [CaseService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [CaseService] SET RECOVERY FULL 
GO

ALTER DATABASE [CaseService] SET  MULTI_USER 
GO

ALTER DATABASE [CaseService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [CaseService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [CaseService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [CaseService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [CaseService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [CaseService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [CaseService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [CaseService] SET  READ_WRITE 
GO


