@echo off

 set "ports=4430 4431 4432 27017" REM port numbers to open mongod service

 for %%p in (%ports%) do (
	telnet -e p 127.0.0.1 %%p 
	IF %ERRORLEVEL% equ 0 (
		IF NOT EXIST C:\data\%%p (MKDIR C:\data\%%p)
		START CALL "C:\Program Files\MongoDB\Server\3.2\bin\mongod.exe"  --port %%p  --dbpath C:\data\%%p 
		
	) ELSE (
		echo "Service is already running."
	)
 )

