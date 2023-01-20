#   Automatické mazání případných (zbylých) souborů

##  Na jednotlivých serverech (adpra191, adpra192) založen task v TaskScheduleru:
    - name: 	Noby temp storage cleaner.
    - description:	Regulary removes old files from folder ´D:\www\noby_temp_storage´
    - trigger:	Daily at 3:00
    - action:	powershell -File D:\www\noby-temp-storage-cleaner.ps1
