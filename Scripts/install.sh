rm -rf bin
rm -rf conf
rm -rf lib
dirName=${PWD##*/}
mv ../$dirName*.jar .
mv ../$dirName*.tar.gz .
../deploy.sh -p $dirName/ -t *.tar.gz
rm *.tar.gz
rm *.jar
