mkdir %1
echo %1
"C:\Program Files (x86)\Unity\Editor\Data\Tools\7z.exe" "a" "-r" "-ttar" "-y" "-bd" %1\archtemp.tar "Assets"
"C:\Program Files (x86)\Unity\Editor\Data\Tools\7z.exe" "a" "-tgzip" "-bd" "-y" %1\CentiliUnity.unitypackage %1\archtemp.tar
del %1\archtemp.tar