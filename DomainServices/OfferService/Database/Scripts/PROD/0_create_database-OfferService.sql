USE [master]
GO

/****** Object:  Database [OfferService]    Script Date: 05.04.2022 19:24:51 ******/
CREATE DATABASE [OfferService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OfferService', FILENAME = N'S:\Data_Silver_18_SQL\OfferService.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OfferService_log', FILENAME = N'S:\Data_Silver_16_SQL\OfferService_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OfferService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [OfferService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [OfferService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [OfferService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [OfferService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [OfferService] SET ARITHABORT OFF 
GO

ALTER DATABASE [OfferService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [OfferService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [OfferService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [OfferService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [OfferService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [OfferService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [OfferService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [OfferService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [OfferService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [OfferService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [OfferService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [OfferService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [OfferService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [OfferService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [OfferService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [OfferService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [OfferService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [OfferService] SET RECOVERY FULL 
GO

ALTER DATABASE [OfferService] SET  MULTI_USER 
GO

ALTER DATABASE [OfferService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [OfferService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [OfferService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [OfferService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [OfferService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [OfferService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [OfferService] SET QUERY_STORE = OFF
GO

ALTER DATABASE [OfferService] SET  READ_WRITE 
GO

