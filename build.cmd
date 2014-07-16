@echo off
@title Build IFME and Release!
set BUILDDIR=_build
cls
echo.
echo This script allowing publish IFME after compile. Using Debug build.
echo because "Release" got some issue with x265...
echo                   1. ifme.exe
echo                   2. ifme.hitoha.dll
echo                   3. ifme.hitoha.kawaii.dll
echo.
echo IFME require some file, download and save it to "%BUILDDIR%" folder after complete.
echo                   1. MediaInfo.dll (64bit)
echo                   2. 7za.exe (rename to za.dll)
echo.
echo IFME will create empty folder:
echo                   1. addons
echo                   2. lang
echo.
echo IFME will use dummy addon, you need actual addon, get from webpage
echo.
echo Press any key to start making (existing folder will be removed!)...
pause >nul
echo.
echo.
echo.
echo DELETEING %BUILDDIR%!
rmdir /s %BUILDDIR%
mkdir %BUILDDIR%
mkdir %BUILDDIR%\addons
mkdir %BUILDDIR%\lang
copy installer\text_gpl2.txt %BUILDDIR%\LICENSE
copy ifme\bin\x64\Debug\lang\*.* %BUILDDIR%\lang
copy ifme\bin\x64\Debug\ifme.exe %BUILDDIR%\
copy ifme\bin\x64\Debug\ifme.hitoha.dll %BUILDDIR%\
copy ifme\bin\x64\Debug\ifme.hitoha.kawaii.dll %BUILDDIR%\
copy ifme\bin\x64\Debug\iso.gg %BUILDDIR%\
echo.
echo DONE! Now copy "7za.exe" (rename to za.dll) 
echo and copy "MediaInfo.dll" to "%BUILDDIR%" folder
echo.
echo After that, download IFME addons and extract to "addons" folder.
echo Then can be release via Installer or Archive :)
echo.
pause