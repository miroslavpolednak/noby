USE [master]
GO


CREATE DATABASE [NobyAudit]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NobyAudit', FILENAME = N'F:\MSSQL15.UAT1\MSSQL\DATA\NobyAudit.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NobyAudit_log', FILENAME = N'G:\MSSQL15.UAT1\MSSQL\Data\NobyAudit_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NobyAudit].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [NobyAudit] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [NobyAudit] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [NobyAudit] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [NobyAudit] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [NobyAudit] SET ARITHABORT OFF 
GO

ALTER DATABASE [NobyAudit] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [NobyAudit] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [NobyAudit] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [NobyAudit] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [NobyAudit] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [NobyAudit] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [NobyAudit] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [NobyAudit] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [NobyAudit] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [NobyAudit] SET  DISABLE_BROKER 
GO

ALTER DATABASE [NobyAudit] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [NobyAudit] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [NobyAudit] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [NobyAudit] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [NobyAudit] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [NobyAudit] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [NobyAudit] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [NobyAudit] SET RECOVERY FULL 
GO

ALTER DATABASE [NobyAudit] SET  MULTI_USER 
GO

ALTER DATABASE [NobyAudit] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [NobyAudit] SET DB_CHAINING OFF 
GO

ALTER DATABASE [NobyAudit] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [NobyAudit] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [NobyAudit] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [NobyAudit] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [NobyAudit] SET QUERY_STORE = OFF
GO

ALTER DATABASE [NobyAudit] SET  READ_WRITE 
GO


