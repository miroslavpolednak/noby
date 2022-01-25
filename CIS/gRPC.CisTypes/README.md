# CIS.Infrastructure.gRPC.CisTypes

Tato knihovna obsahuje custom *Protobuf* typy, které mohou používat všechny služby v prostředí CIS.  
V adresáři *Protos* jsou Protobuf *.proto soubory.  
V adresáři *CisType* jsou případné partial classes doplňující tyto typy - např. přidávající implicitní konverze z a na známý .NET typ.

## Seznam typů
 - **GrpcDate** (C# *System.DateOnly*). Obsahuje implicitní konverze z a na System.DateOnly a z a na System.DateTime.
 - **NullableGrpcDate** stejné jako GrpcDate, pouze C# type je System.DateOnly?.
 - **GrpcDateTime** (C# *System.DateTime*). Obsahuje implicitní konverze z a na System.DateTime.
 - **GrpcDecimal** (C# *System.Decimal*). Obsahuje implicitní konverze z a na System.Decimal.
 - **ModificationStamp** složený typ sloužící k identifikaci času a uživatele, který provedl změnu/založení objektu.
 - **Identity** složený typ sloužící pro identifikaci klienta/uživatele/poradce atd.
 - **PaginationRequest** společný typ pro podporu stránkování seznamu záznamů - vstupní typ.
 - **PaginationResponse** společný typ pro podporu stránkování seznamu záznamů - výstupní typ.
 - **UserInfo** razítko uživatele (poradce) - obsahuje jméno a Id

