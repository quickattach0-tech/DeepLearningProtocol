@echo off
REM Deep Learning Protocol Windows Installer Batch Script
REM Version: 3.1
REM This script installs Deep Learning Protocol on Windows systems

setlocal enabledelayedexpansion

set VERSION=3.2
set INSTALL_DIR=%ProgramFiles%\DeepLearningProtocol
set BIN_DIR=%INSTALL_DIR%\bin
set DATA_DIR=%APPDATA%\DeepLearningProtocol
set CONFIG_DIR=%DATA_DIR%\config
set LOG_DIR=%DATA_DIR%\logs

REM Check for admin privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo This installer must be run as Administrator
    echo Please right-click and select "Run as Administrator"
    pause
    exit /b 1
)

echo.
echo Deep Learning Protocol Windows Installer v%VERSION%
echo ===================================================
echo.

REM Check system requirements
echo Checking system requirements...
where dotnet >nul 2>&1
if %errorLevel% neq 0 (
    echo Warning: .NET Runtime 10.0 is not installed.
    echo Please install .NET Runtime from https://dotnet.microsoft.com/download
    set /p CONTINUE="Continue anyway? (y/n): "
    if /i not "!CONTINUE!"=="y" exit /b 1
) else (
    for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
    echo [OK] .NET Runtime found: !DOTNET_VERSION!
)

REM Create necessary directories
echo.
echo Creating directories...
if not exist "%INSTALL_DIR%" mkdir "%INSTALL_DIR%"
if not exist "%BIN_DIR%" mkdir "%BIN_DIR%"
if not exist "%DATA_DIR%" mkdir "%DATA_DIR%"
if not exist "%CONFIG_DIR%" mkdir "%CONFIG_DIR%"
if not exist "%LOG_DIR%" mkdir "%LOG_DIR%"
echo [OK] Directories created

REM Install application files
echo.
echo Installing application files...
xcopy /E /I /Y "bin\Release\net10.0\win-x64\publish\*" "%BIN_DIR%\" >nul
if exist "appsettings.json" (
    copy /Y "appsettings.json" "%CONFIG_DIR%\appsettings.json" >nul
)
echo [OK] Application files installed

REM Create Start Menu shortcut
echo.
echo Creating Start Menu shortcut...
set DESKTOP=%USERPROFILE%\Desktop
set STARTMENU=%APPDATA%\Microsoft\Windows\Start Menu\Programs
if not exist "%STARTMENU%" mkdir "%STARTMENU%"

REM Create shortcut using PowerShell
powershell -Command ^
    "$WshShell = New-Object -ComObject WScript.Shell; ^
    $Shortcut = $WshShell.CreateShortcut('%STARTMENU%\Deep Learning Protocol.lnk'); ^
    $Shortcut.TargetPath = '%BIN_DIR%\DeepLearningProtocol.exe'; ^
    $Shortcut.WorkingDirectory = '%DATA_DIR%'; ^
    $Shortcut.Save()"
echo [OK] Start Menu shortcut created

REM Create Desktop shortcut
powershell -Command ^
    "$WshShell = New-Object -ComObject WScript.Shell; ^
    $Shortcut = $WshShell.CreateShortcut('%DESKTOP%\Deep Learning Protocol.lnk'); ^
    $Shortcut.TargetPath = '%BIN_DIR%\DeepLearningProtocol.exe'; ^
    $Shortcut.WorkingDirectory = '%DATA_DIR%'; ^
    $Shortcut.Save()"
echo [OK] Desktop shortcut created

REM Add to Windows Registry for uninstall
echo.
echo Registering application...
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\DeepLearningProtocol" /v "DisplayName" /d "Deep Learning Protocol" /f >nul
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\DeepLearningProtocol" /v "UninstallString" /d "%INSTALL_DIR%\uninstall.bat" /f >nul
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\DeepLearningProtocol" /v "DisplayVersion" /d "%VERSION%" /f >nul
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\DeepLearningProtocol" /v "InstallLocation" /d "%INSTALL_DIR%" /f >nul
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\DeepLearningProtocol" /v "Publisher" /d "quickattach0-tech" /f >nul
echo [OK] Application registered

REM Create uninstall script
echo.
echo Creating uninstall script...
(
    echo @echo off
    echo echo Uninstalling Deep Learning Protocol...
    echo rmdir /S /Q "%INSTALL_DIR%" 2>nul
    echo rmdir /S /Q "%DATA_DIR%" 2>nul
    echo del "%%APPDATA%%\Microsoft\Windows\Start Menu\Programs\Deep Learning Protocol.lnk" 2>nul
    echo del "%%USERPROFILE%%\Desktop\Deep Learning Protocol.lnk" 2>nul
    echo reg delete "HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\DeepLearningProtocol" /f 2>nul
    echo echo Uninstallation complete.
    echo pause
) > "%INSTALL_DIR%\uninstall.bat"
echo [OK] Uninstall script created

REM Display installation summary
echo.
echo ================================
echo Deep Learning Protocol v%VERSION%
echo Installation Complete!
echo ================================
echo.
echo Installation Details:
echo   Install Directory: %INSTALL_DIR%
echo   Binary Location: %BIN_DIR%\DeepLearningProtocol.exe
echo   Data Directory: %DATA_DIR%
echo   Config Directory: %CONFIG_DIR%
echo   Log Directory: %LOG_DIR%
echo.
echo Next Steps:
echo   1. Run the application from Start Menu or Desktop shortcut
echo   2. Configure the application in %CONFIG_DIR%\appsettings.json
echo.
echo To uninstall:
echo   Run: %INSTALL_DIR%\uninstall.bat
echo.
pause
