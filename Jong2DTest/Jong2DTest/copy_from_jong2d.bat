set platformName=%1
set outDir=%2
set projectDir=%3

echo .
echo .
echo start copy_from_jong2d.bat

echo platformName=%platformName% 
echo projectDir=%projectDir%
echo .

:: 프로젝트 위치로 주소 변경합니다.
echo cd %projectDir%
cd %projectDir%

:: Jong2D 폴더를 지정해줍니다.
set sdlpath=..\..\JongLib\Jong2D\output\%platformName%
echo sdlPath : %sdlpath%

:: SDL 위치에서 복사 합니다.
echo copy /Y %sdlpath% %outDir%
copy /Y %sdlpath% %outDir% 

echo end copy_from_jong2d.bat
echo .
echo .