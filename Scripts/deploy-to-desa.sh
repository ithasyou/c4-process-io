clear
mvn clean install -U -DskipTests
sshpass -p '******' scp Assembly/target/*.jar USER@DESAHOST:~
sshpass -p '******' scp Assembly/target/*.tar.gz USER@DESAHOST:~
echo DONE
