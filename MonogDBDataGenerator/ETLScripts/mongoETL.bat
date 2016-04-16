@echo off
for /f "tokens=1-7 delims=:/-, " %%i in ('echo exit^|cmd /q /k"prompt $d $t"') do (
   for /f "tokens=2-4 delims=/-,() skip=1" %%a in ('echo.^|date') do (
      set dow=%%i
      set %%a=%%j
      set %%b=%%k
      set %%c=%%l
      set hh=%%m
      set min=%%n
      set ss=%%o
   )
)

REM Let's see the result.
set fulldate=%dd%/%mm%/%yy% %hh%:%min%:%ss%
set datestr=%mm%_%yy%
set logFile=log_%dd%_%mm%_%yy%.txt
set coll=zarkov_%datestr%
set fileName=%coll%.json
set port=%1
REM set port=4430

REM Step 1: Check if the data is being inserted before locking the database
REM Step 2: Export the collection in localhost
REM Step 3: Drop the collection in localhost
REM Step 4: Import the collection in remote server
REM Step 5: Remove the exported file
echo -------------------------START OF PROCESS------------------------------------- >> C:\data\outfiles\%logFile%
:insertionCheck
for /f %%i in ('call "C:\Program Files\MongoDB\Server\3.2\bin\mongo.exe" --host 127.0.0.1 --port %port%   agilent --eval "db.getCollection('Locks').count({"lock":'Y'})"') do set RESULT=%%i
if %RESULT% equ 0 (	
	:startExport
	"C:\Program Files\MongoDB\Server\3.2\bin\mongo.exe" --host 127.0.0.1 --port %port%  agilent --eval "db.fsyncUnlock()" >> C:\data\outfiles\%logFile% 2>&1 
	echo [%fulldate%]:Info - Obtaining lock in the database. >> C:\data\outfiles\%logFile%
	"C:\Program Files\MongoDB\Server\3.2\bin\mongo.exe" --host 127.0.0.1 --port %port%  agilent --eval "db.fsyncLock()" >> C:\data\outfiles\%logFile% 2>&1 
	echo [%fulldate%]:Info - Exporting the data started. >> C:\data\outfiles\%logFile%
	"C:\Program Files\MongoDB\Server\3.2\bin\mongoexport.exe" --host 127.0.0.1 --port %port% --db agilent --collection %coll% --out C:\data\outfiles\%port%\%fileName% >> C:\data\outfiles\%logFile% 2>&1
	if %errorlevel% equ 0 (
		echo [%fulldate%]:Info - Export complete.>> C:\data\outfiles\%logFile% 
		goto :dropCollection
	} else (
		echo [%fulldate%]:Error - Errors encountered during export.>> C:\data\outfiles\%logFile%
		REM sleep 5 mins before next try.
		TIMEOUT /T 300 
		echo [%fulldate%]:Info - Restarting the export process.>> C:\data\outfiles\%logFile%
		goto :startExport
	)

	:dropCollection
	echo [%fulldate%]:Info - Releasing lock in the database. >> C:\data\outfiles\%logFile%
	"C:\Program Files\MongoDB\Server\3.2\bin\mongo.exe" --host 127.0.0.1 --port %port%  agilent --eval "db.fsyncUnlock()" >> C:\data\outfiles\%logFile% 2>&1
	echo [%fulldate%]:Info - Clearing the used collection. >> C:\data\outfiles\%logFile%
	"C:\Program Files\MongoDB\Server\3.2\bin\mongo.exe" --host 127.0.0.1 --port %port%  agilent --eval "db."%coll%".drop()" >> C:\data\outfiles\%logFile% 2>&1
	if %errorlevel% equ 0 (
		echo [%fulldate%]:Info - Collection drop complete.>> C:\data\outfiles\%logFile% 
		goto :mongoImport 
	) else (
		echo [%fulldate%]:Error - Error encountered during dropCollection.>> C:\data\outfiles\%logFile%
		REM sleep 5 mins before next try.
		TIMEOUT /T 300 
		echo [%fulldate%]:Info - Restarting the collection drop process.>> C:\data\outfiles\%logFile%
		goto :dropCollection
	)

	:mongoImport
	echo [%fulldate%]:Info - Starting import to the main server.>> C:\data\outfiles\%logFile%
	"C:\Program Files\MongoDB\Server\3.2\bin\mongoimport.exe" --host 127.0.0.1 --port 27017  --db agilent --collection %coll% --file C:\data\outfiles\%port%\%fileName% >> C:\data\outfiles\%logFile% 2>&1
	if %errorlevel% equ 0 (
		echo [%fulldate%]:Info - Import to the main server complete.>> C:\data\outfiles\%logFile% 
		goto :deleteFile
	) else (
		echo [%fulldate%]:Error - Error encountered during mongoImport. >> C:\data\outfiles\%logFile%
		REM sleep 5 mins before next try.
		TIMEOUT /T 300 
		echo [%fulldate%]:Info - Restarting the import process.>> C:\data\outfiles\%logFile%
		goto :mongoImport
	)

	:deleteFile
	echo [%fulldate%]:Info - File being deleted: %fileName%.>> C:\data\outfiles\%logFile%
		DEL C:\data\outfiles\%port%\%fileName% >> C:\data\outfiles\%logFile% 2>&1
	if %errorlevel% equ 0 (
		goto :endScript
	) else (
		echo [%fulldate%]:Error - Error encountered during deleteFile.>> C:\data\outfiles\%logFile%
		REM sleep 5 mins before next try.
		TIMEOUT /T 300 
		echo [%fulldate%]:Info - Restarting the delete process.>> C:\data\outfiles\%logFile%
		goto :deleteFile
	)

) else (
	echo [%fulldate%]:Info - Collection Currently in use. Waiting for insertion operation to complete.>> C:\data\outfiles\%logFile%
	REM sleep 5 mins before next try.
	TIMEOUT /T 300 
	echo [%fulldate%]:Info - Restarting the ETL process.>> C:\data\outfiles\%logFile%
	goto :insertionCheck
)

	:endScript
	echo ------------------------END OF PROCESS-------------------------------------- >> C:\data\outfiles\%logFile%
  