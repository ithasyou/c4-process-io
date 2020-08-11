#!/bin/bash

git checkout -b release/$1

/usr/bin/mvnversion.sh $1

echo "Adding all pom files"
find . -name 'pom.xml' | xargs git add

echo "Committing with message 'Close version $1'"
git commit -m "Close version $1"

git tag $1

if [ -z "$2" ]
then
  echo "no second version detectod"
else
  git checkout develop

  /usr/bin/mvnversion.sh $2-SNAPSHOT

  echo "Adding all pom files"
  find . -name 'pom.xml' | xargs git add

  echo "Committing with message 'Begin version version $2'"
  git commit -m "Begin version $2"
  
  git checkout master
  git merge -Xtheirs $1
  
  git checkout develop
fi


echo "DONE"
