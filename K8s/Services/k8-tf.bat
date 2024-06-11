::===============================================================
:: The code below uses batch script to convert k8s yaml/yml files to 
:: tf files using tfk8s, https://github.com/jrhouston/tfk8s
::
::===============================================================

@echo off

CALL :ProcessFile yaml
CALL :ProcessFile yml
EXIT /B %ERRORLEVEL%

:ProcessFile
setlocal enabledelayedexpansion 
set arr[1]=void
for %%f in (.\*.%1) do (
    set /A index=index+1
    set arr[!index!]=%%f
    )
if !arr[1]!==void EXIT /B 0

echo Files in the directory:
for /l %%n in (1,1,%index%) do ( 
   set tempVar=!arr[%%n]!
   set convVar=!tempVar:%1=tf!
   set opArr[%%n]=!convVar!
   echo !tempVar!
)
echo Converted files are:
for /l %%n in (1,1,%index%) do ( 
   echo !opArr[%%n]!
   tfk8s -f !arr[%%n]! -o !opArr[%%n]!
)
ENDLOCAL

EXIT /B 0
