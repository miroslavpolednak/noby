USE [master]
GO
/****** Object:  Database [RealEstateValuationService]    Script Date: 28.06.2023 13:57:04 ******/
CREATE DATABASE [RealEstateValuationService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RealEstateValuationService', FILENAME = N'S:\Data_Silver_18_SQL\RealEstateValuationService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RealEstateValuationService_log', FILENAME = N'S:\Data_Silver_16_SQL\RealEstateValuationService_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RealEstateValuationService] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RealEstateValuationService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RealEstateValuationService] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET ARITHABORT OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RealEstateValuationService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RealEstateValuationService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RealEstateValuationService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RealEstateValuationService] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET RECOVERY FULL 
GO
ALTER DATABASE [RealEstateValuationService] SET  MULTI_USER 
GO
ALTER DATABASE [RealEstateValuationService] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RealEstateValuationService] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RealEstateValuationService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RealEstateValuationService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RealEstateValuationService] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RealEstateValuationService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RealEstateValuationService', N'ON'
GO
ALTER DATABASE [RealEstateValuationService] SET QUERY_STORE = OFF
GO
ALTER DATABASE [RealEstateValuationService] SET  READ_WRITE 
GO