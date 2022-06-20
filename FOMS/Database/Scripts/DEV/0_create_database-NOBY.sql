USE [master]
GO

/****** Object:  Database [NOBY]    Script Date: 05.04.2022 19:14:31 ******/
CREATE DATABASE [NOBY]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NOBY', FILENAME = N'F:\MSSQL15.MSSQLSERVER\MSSQL\DATA\NOBY.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NOBY_log', FILENAME = N'G:\MSSQL15.MSSQLSERVER\MSSQL\Data\NOBY_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NOBY].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [NOBY] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [NOBY] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [NOBY] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [NOBY] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [NOBY] SET ARITHABORT OFF 
GO

ALTER DATABASE [NOBY] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [NOBY] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [NOBY] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [NOBY] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [NOBY] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [NOBY] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [NOBY] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [NOBY] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [NOBY] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [NOBY] SET  DISABLE_BROKER 
GO

ALTER DATABASE [NOBY] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [NOBY] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [NOBY] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [NOBY] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [NOBY] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [NOBY] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [NOBY] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [NOBY] SET RECOVERY FULL 
GO

ALTER DATABASE [NOBY] SET  MULTI_USER 
GO

ALTER DATABASE [NOBY] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [NOBY] SET DB_CHAINING OFF 
GO

ALTER DATABASE [NOBY] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [NOBY] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [NOBY] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [NOBY] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [NOBY] SET QUERY_STORE = OFF
GO

ALTER DATABASE [NOBY] SET  READ_WRITE 
GO


