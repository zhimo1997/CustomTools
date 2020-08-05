@echo off
%1 mshta vbscript:CreateObject("Shell.Application").ShellExecute("cmd.exe","/c %~s0 ::","","runas",1)(window.close)&&exit

for %%d in (%~dp0.) do set Directory=%%~fd
set Directory = %Directory%
for %%d in (%~dp0..) do set ParentDirectory=%%~fd
set ParentDirectory = %ParentDirectory%
for %%d in (%~dp0../..) do set RootDirectory=%%~fd

REM echo %Directory%
REM echo %ParentDirectory%
REM echo %RootDirectory%

set linDir=\.git\hooks\pre-commit
set srcDir=\Tools\GitHook\hook\pre-commit

set linAbsoluteDir=%RootDirectory%%linDir%
set srcAbsoluteDir=%RootDirectory%%srcDir%

REM echo %linAbsoluteDir%
REM echo %srcAbsoluteDir%

Rem hook 软链接------------------------------------------------------------------------
del %linAbsoluteDir%
mklink %linAbsoluteDir% %srcAbsoluteDir%

set regpath=HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
set evname=BAT_HOME
set batpath=%~dp0luacheck
REM echo %ENV_PATH%

REM set ENV_PATH=%PATH%
REM set envpath=%ENV_PATH%;%batpath%
REM reg add "%regpath%" /v Path /d "%envpath%" /f

set str1="%PATH%"
set batpath=%~dp0luacheck;
set envpath=%PATH%%batpath%
set regpath=HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment

Rem 环境变量检测和设置------------------------------------------------------------------------
if x%str1:luacheck=%==x%str1% (
    REM echo "%envpath%"
    echo no check!
    reg add "%regpath%" /v backupPath /d "%path%" /f
    reg add "%regpath%" /v path /d "%envpath%" /f
) else (
    echo luacheck env have been added!
)

set datapath = %LOCALAPPDATA%
REM echo %LOCALAPPDATA%

Rem 创建luacheck文件夹------------------------------------------------------------------------
md "%LOCALAPPDATA%\Luacheck"

set linCheckCfgDir=\Luacheck\.luacheckrc
set srcCheckCfgDir=\luacheck\.luacheckrc
set linCheckCfgAbsoluteDir=%LOCALAPPDATA%%linCheckCfgDir%
set srcCheckCfgAbsoluteDir=%Directory%%srcCheckCfgDir%
REM echo %linCheckCfgAbsoluteDir%
REM echo %srcCheckCfgAbsoluteDir%

Rem luacheck配置软链接------------------------------------------------------------------------
del %linCheckCfgAbsoluteDir%
mklink %linCheckCfgAbsoluteDir% %srcCheckCfgAbsoluteDir%

pause