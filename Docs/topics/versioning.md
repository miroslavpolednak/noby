# Verzování FE API (NOBY.Api)
Verzujeme per endpoint, tj. nikdy nebude existovat jedna jediná aktuální verze aplikace.
Každý endpoint má svoji verzi
Novou verzi vytváříme pouze v případě, že se mění byznys logika za endpointem nebo se mění kontrakt takovým způsobem, že FE danou změnu nedokáže zpracovat.

Staré verze endpointů nemažeme dokud nebude nasazena kompaktibilní verze FE na produkci. 
Do té doby jsou staré verze endpointů odekorovány atributem `[Obsolete]`.



# Verzování doménových služeb
