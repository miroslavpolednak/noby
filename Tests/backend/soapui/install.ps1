
$ProgramFiles="C:\Program Files"
$SoapUIVersion="5.7.0"
$SoapUIPath=$ProgramFiles+"\SmartBear\SoapUI-"+$SoapUIVersion;
$SoapUIJar="soapui-"+$SoapUIVersion+".jar";
$SoapUIJarFullPath=$SoapUIPath+'/bin/'+$SoapUIJar
$SoapUIPathToExt=$SoapUIPath+'/bin/ext/'


#$guavaVersion="31.1-jre"
$guavaVersion="23.0"
$guavaPath="~/.m2/repository/com/google/guava/guava/"+$guavaVersion
$guavaJarFullPath=$guavaPath+"/guava-"+$guavaVersion+".jar"
$now=Get-Date -Format "yyyy-MM-dd--HH-mm-ss"


#Will also download Guava to Maven repository
mvn
cp .\target\reflectioncall-0.1-SNAPSHOT-jar-with-dependencies.jar $SoapUIPathToExt


echo $ProgramFiles
echo $SoapUIPath
echo $SoapUIJar
echo $SoapUIJarFullPath


mkdir tmp
Push-Location
cd tmp
mkdir soapui
unzip $SoapUIJarFullPath -d soapui


mkdir guava
# unzip can't handle this path. Probably ~ is the problem. Why?????
cp $guavaJarFullPath guava.jar
unzip guava.jar -d guava


mkdir soapui\com\google
cp -r guava\com\google\common soapui\com\google\

Push-Location
cd soapui
zip -r ../soapui.jar ./*
Pop-Location

$backupFile=$SoapUIJarFullPath+"-back-"+$now
cp $SoapUIJarFullPath $backupFile

cp soapui.jar $SoapUIJarFullPath

Pop-Location
rm tmp




