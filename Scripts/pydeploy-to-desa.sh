clear
./gradlew clean build
sshpass -p '******' scp build/distributions/*.zip USER@DESAHOST:~
echo DONE
