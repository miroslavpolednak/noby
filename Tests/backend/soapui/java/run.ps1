rm reflectioncall-0.1-SNAPSHOT-jar-with-dependencies.jar
rm main.class
Push-Location
cd ..
mvn
Pop-Location
cp ..\target\reflectioncall-0.1-SNAPSHOT-jar-with-dependencies.jar .
javac -cp ".;./reflectioncall-0.1-SNAPSHOT-jar-with-dependencies.jar"  .\main.java
java -cp ".;./reflectioncall-0.1-SNAPSHOT-jar-with-dependencies.jar"  main
