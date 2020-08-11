echo delete $1
rm -r $1
echo unzip $1
unzip $1*.zip
echo configure conf
mv $1/conf/desa/* $1/conf/
chmod +x $1/bin/*.sh
rm $1*.zip


