# gRPC služby - Doménové služby a Infrastrukturní služby

gRPC služby jsou provozovány formou Windows Service. 
Je to proto, že staré Windows servery neumí plný rozsah HTTP2 (hlavně Trailers), takže služby není možné provozovat standardně v IISku.
Některé gRPC služby mohou poskytovat zároveň REST rozhraní pomocí *gRPC Transcoding*.

Anatomie projektu gRPC služby

Startup gRPC služby


