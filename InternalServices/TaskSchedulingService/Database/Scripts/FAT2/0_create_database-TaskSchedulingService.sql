USE [master]
GO

/****** Object:  Database [TaskSchedulingService]    Script Date: 01.04.2023 0:05:27 ******/
CREATE DATABASE [TaskSchedulingService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TaskSchedulingService', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL15.FAT2\MSSQL\Data_Silver_04_SQL\TaskSchedulingService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TaskSchedulingService_log', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL15.FAT2\MSSQL\Data_Silver_01_SQL\TaskSchedulingService_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TaskSchedulingService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TaskSchedulingService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET ARITHABORT OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TaskSchedulingService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TaskSchedulingService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [TaskSchedulingService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TaskSchedulingService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET RECOVERY FULL 
GO

ALTER DATABASE [TaskSchedulingService] SET  MULTI_USER 
GO

ALTER DATABASE [TaskSchedulingService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TaskSchedulingService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TaskSchedulingService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TaskSchedulingService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [TaskSchedulingService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [TaskSchedulingService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [TaskSchedulingService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [TaskSchedulingService] SET  READ_WRITE 
GO


