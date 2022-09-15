@echo off

echo == ZzzLab Framework Sync ===========================

cd ZzzLab.Core
git pull --progress &
git commit -a -m "Auto Sync : "%DATE%" "%TIME%" &
git push --progress 
cd ..

echo == End of Document ===========================
timeout /t 20